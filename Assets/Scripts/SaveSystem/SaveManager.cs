using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BeastHunter
{
    public sealed class SaveManager : ISaveManager
    {
        #region Fields

        private int _newEntry = 1;
        private List<int> _completedQuestsById;
        private List<Quest> _activeQuests;
        private List<Quest> _completedQuests;
        private readonly ISaveFileWrapper _saveFileWrapper;

        #endregion


        #region Methods

        public SaveManager(ISaveFileWrapper wrapper)
        {
            _saveFileWrapper = wrapper;
        }

        public void SaveGame(string filename)//= null for saving into new file everytime)
        {
            _saveFileWrapper.CreateNewSave(filename ?? DateTime.Now.ToString("s").Replace(':', '-') + ".bytes");
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.Saving, null);
            SaveInfo();
        }

        private void SaveInfo()
        {
            _saveFileWrapper.AddSaveData("NextEntry", _newEntry.ToString());
        }

        public void LoadGame(string filename)
        {
            _saveFileWrapper.LoadSave(filename);
            _newEntry = _saveFileWrapper.GetNextItemEntry();
            _completedQuestsById = _saveFileWrapper.GetCompletedQuestsId().ToList();
            _activeQuests = LoadQuestLog();
        }

        public void SaveQuestLog(List<Quest> quests)
        {
            _saveFileWrapper.SaveQuestLog(quests, _completedQuestsById);
        }

        public List<Quest> LoadQuestLog()
        {
            var res = new List<Quest>();
            var qd = _saveFileWrapper.GetActiveQuests();
            var od = _saveFileWrapper.GetActiveObjectives();
            foreach (var i in qd)
            {
                var quest = new Quest(QuestRepository.GetById(i.Key));
                if (quest.IsTimed) quest.ReduceTime(quest.TimeAllowed - i.Value);
                foreach (var task in quest.Tasks)
                {
                    if (od.ContainsKey(task.Id))
                    {
                        task.AddAmount(od[task.Id]);
                    }
                }
                res.Add(quest);
            }

            return res;
        }

        public List<Quest> LoadCompletedQuestLog()
        {
            var res = new List<Quest>();
            var od = _saveFileWrapper.GetActiveObjectives();
            var cqd = _saveFileWrapper.GetCompletedQuests();
            foreach (var i in cqd)
            {
                var quest = new Quest(QuestRepository.GetById(i.Key));
                foreach (var task in quest.Tasks)
                {
                    task.AddAmount(task.NeededAmount);
                }
                res.Add(quest);
            }

            return res;
        }

        public void QuestCompleted(int id)
        {
            if (_completedQuestsById.Contains(id))
            {
                Debug.LogWarning($"SaveManager::QuestComplete: Quest[{id}] already completed!");
                return;
            }
            _completedQuestsById.Add(id);
        }

        public List<int> GetAllCompletedQuestsById()
        {
            return _completedQuestsById ?? (_completedQuestsById = _saveFileWrapper.GetCompletedQuestsId().ToList());
        }

        public List<Quest> GetAllActiveQuests()
        {
            return _activeQuests ?? (_activeQuests = LoadQuestLog());
        }

        public List<Quest> GetAllCompletedQuests()
        {
            return _completedQuests ?? (_completedQuests = LoadCompletedQuestLog());
        }
        
        public List<int> GetAllActiveQuestsById()
        {
            var IdActiveQuests= _saveFileWrapper.GetActiveQuests();
            return IdActiveQuests.Keys.ToList();
        }

        public void SetQuestIsNotComplete(int id)
        {
            
        }
        #endregion
    }
}