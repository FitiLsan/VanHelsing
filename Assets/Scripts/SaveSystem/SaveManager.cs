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
        private List<int> _completedQuests;
        private List<Quest> _activeQuests;
        private readonly ISaveFileWrapper _saveFileWrapper;
        
        #endregion
        

        #region Methods

        public SaveManager(ISaveFileWrapper wrapper)
        {
            _saveFileWrapper = wrapper;
        }
        
        public void SaveGame(string filename)//= null)
        {
            _saveFileWrapper.CreateNewSave(filename ?? DateTime.Now.ToString("s").Replace(':','-')+".bytes");
            EventManager.TriggerEvent(GameEventTypes.Saving, null);
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
            _completedQuests = _saveFileWrapper.GetCompletedQuests().ToList();
            _activeQuests = LoadQuestLog();
        }
        
        public void SaveQuestLog(List<Quest> quests)
        {
            _saveFileWrapper.SaveQuestLog(quests, _completedQuests);           
        }

        public List<Quest> LoadQuestLog()
        {           
            var res = new List<Quest>();
            var qd = _saveFileWrapper.GetActiveQuests();
            var od = _saveFileWrapper.GetActiveObjectives();
            foreach (var i in qd)
            {
                var quest = new Quest(QuestRepository.GetById(i.Key));
                if (quest.IsTimed) quest.ReduceTime(quest.TimeAllowed-i.Value);
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

        public void QuestCompleted(int id)
        {
            if (_completedQuests.Contains(id))
            {
                Debug.LogWarning($"SaveManager::QuestComplete: Quest[{id}] already completed!");
                return;
            }
            _completedQuests.Add(id);
        }

        public List<int> GetAllCompletedQuests()
        {
            return _completedQuests ?? (_completedQuests = _saveFileWrapper.GetCompletedQuests().ToList());
        }
       
        public List<Quest> GetAllActiveQuests()
        {
            return _activeQuests ?? (_activeQuests = LoadQuestLog());
        }

        #endregion
    }
}