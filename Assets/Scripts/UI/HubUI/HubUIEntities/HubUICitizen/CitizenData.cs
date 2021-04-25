using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "Citizen", menuName = "CreateData/HubUIData/Citizen", order = 0)]
    public class CitizenData : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private int _firstDialogId;

        [SerializeField, ContextMenuItem("Reset ids", "DialogListResetIds")]
        private List<DialogNode> _dialogs;
        [SerializeField, ContextMenuItem("Reset ids", "QuestAnswersListResetIds")]
        private List<QuestAnswer> _questAnswers;

#if UNITY_EDITOR
        private int _dialogsListCount;
        private int _questAnswersListCount;

        private int _nextDialogsListId;
        private int _nextQuestAnswersListId;
#endif

        #endregion


        #region Properties

        public string Name => _name;
        public Sprite Portrait => _portrait;
        public int FirstDialogId => _firstDialogId;
        public List<DialogNode> Dialogs => _dialogs;
        public List<QuestAnswer> QuestAnswers => _questAnswers;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            #if UNITY_EDITOR
            _dialogsListCount = _dialogs.Count;
            _questAnswersListCount = _questAnswers.Count;
            _nextDialogsListId = NextDialogsListId();
            _nextQuestAnswersListId = NextQuestAnswersListId();
            #endif
        }

        private void OnValidate()
        {
            OnChangeDialogListIdValidate();
            OnChangeQuestAnswersListIdValidate();
        }

        #endregion


        #region Methods

        #if UNITY_EDITOR
        private void OnChangeDialogListIdValidate()
        {
            if (_dialogsListCount != _dialogs.Count)
            {
                if (_dialogsListCount < _dialogs.Count)
                {
                    for (int i = 1; i < _dialogs.Count; i++)
                    {
                        if (_dialogs[i].Id == _dialogs[i - 1].Id)
                        {
                            _dialogs[i].SetId(_nextDialogsListId++);
                        }
                    }
                }
                else if (_dialogsListCount > _dialogs.Count)
                {
                    _nextDialogsListId = NextDialogsListId();
                }
                _dialogsListCount = _dialogs.Count;
            }
        }

        private void OnChangeQuestAnswersListIdValidate()
        {
            if (_questAnswersListCount != _questAnswers.Count)
            {
                if (_questAnswersListCount < _questAnswers.Count)
                {
                    for (int i = 1; i < _questAnswers.Count; i++)
                    {
                        if (_questAnswers[i].Id == _questAnswers[i - 1].Id)
                        {
                            _questAnswers[i].SetId(_nextQuestAnswersListId++);
                        }
                    }
                }
                else if (_questAnswersListCount > _questAnswers.Count)
                {
                    _nextQuestAnswersListId = NextQuestAnswersListId();
                }
                _questAnswersListCount = _questAnswers.Count;
            }
        }

        private int NextDialogsListId()
        {
            _nextDialogsListId = 0;
            for (int i = 0; i < _dialogs.Count; i++)
            {
                if (_dialogs[i].Id > _nextDialogsListId) _nextDialogsListId = _dialogs[i].Id;
            }
            return ++_nextDialogsListId;
        }

        private int NextQuestAnswersListId()
        {
            _nextQuestAnswersListId = 0;
            for (int i = 0; i < _questAnswers.Count; i++)
            {
                if (_questAnswers[i].Id > _nextQuestAnswersListId) _nextQuestAnswersListId = _questAnswers[i].Id;
            }
            return ++_nextQuestAnswersListId;
        }

        private void DialogListResetIds() //ContextMenuItem
        {
            for (int i = 0; i < _dialogs.Count; i++) _dialogs[i].SetId(i);
            _nextDialogsListId = _dialogs.Count;
        }

        private void QuestAnswersListResetIds() //ContextMenuItem
        {
            for (int i = 0; i < _questAnswers.Count; i++) _questAnswers[i].SetId(i);
            _nextQuestAnswersListId = _questAnswers.Count;
        }
        #endif

        #endregion
    }
}
