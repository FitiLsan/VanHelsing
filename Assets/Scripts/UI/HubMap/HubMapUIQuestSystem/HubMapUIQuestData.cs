using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class HubMapUIQuestData : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _title;
        [SerializeField, ContextMenuItem("Reset ids", "TasksListResetIds")] private List<HubMapUIQuestTaskData> _tasks;
        [SerializeField] private int _firstTaskId;
        [SerializeField] private int _emptyEndTaskId;
        [SerializeField] private HubMapUICityReputation _requiredReputation;
        [SerializeField] private HubMapUIQuestData _requiredQuest;

        #if UNITY_EDITOR
        private int _tasksListCount;
        private int _nextTasksListId;
        #endif

        #endregion


        #region Properties

        public string Title => _title;
        public int FirstTaskId => _firstTaskId;
        public int EmptyEndTaskId => _emptyEndTaskId;
        public HubMapUICityReputation RequiredReputation => _requiredReputation;
        public HubMapUIQuestData RequiredQuest => _requiredQuest;
        public List<HubMapUIQuestTaskData> Tasks => _tasks;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            #if UNITY_EDITOR
            _tasksListCount = _tasks.Count;
            _nextTasksListId = NextTasksListId();
            #endif
        }

        private void OnValidate()
        {
            OnChangeTasksListIdValidate();
        }

        #endregion


        #region Methods

        #if UNITY_EDITOR

        private void OnChangeTasksListIdValidate()
        {
            if (_tasksListCount != _tasks.Count)
            {
                if (_tasksListCount < _tasks.Count)
                {
                    for (int i = 1; i < _tasks.Count; i++)
                    {
                        if (_tasks[i].Id == _tasks[i - 1].Id)
                        {
                            _tasks[i].SetId(_nextTasksListId++);
                        }
                    }
                }
                else if (_tasksListCount > _tasks.Count)
                {
                    _nextTasksListId = NextTasksListId();
                }
                _tasksListCount = _tasks.Count;
            }
        }

        private int NextTasksListId()
        {
            _nextTasksListId = 0;
            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_tasks[i].Id > _nextTasksListId) _nextTasksListId = _tasks[i].Id;
            }
            return ++_nextTasksListId;
        }

        private void TasksListResetIds() //ContextMenuItem
        {
            for (int i = 0; i < _tasks.Count; i++) _tasks[i].SetId(i);
            _nextTasksListId = _tasks.Count;
        }

        #endif

        #endregion
    }
}
