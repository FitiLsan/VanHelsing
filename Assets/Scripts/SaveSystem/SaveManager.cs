using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseWrapper;
using Events;
using Interfaces;
using Items;
using Quests;
using SaveSystem.SaveDto;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : ISaveManager
    {
        #region private members
        private int _newEntry = 1;
        private List<int> _completedQuests;
        private readonly ISaveFileWrapper _saveFileWrapper;
        private readonly IItemStorage _itemStorage;
        
        #endregion
        
        #region "Public" methods

        public SaveManager(ISaveFileWrapper wrapper, IItemStorage itemStorage)
        {
            _saveFileWrapper = wrapper;
            _itemStorage = itemStorage;
        }
        
        /// <summary>
        /// Logic: Call this function to prepare save file, then this class triggers saving event for other systems
        /// </summary>
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
        }
        
        #endregion
        
        #region quest
        public void SaveQuestLog(List<Quest> quests)
        {
            _saveFileWrapper.SaveQuestLog(quests, _completedQuests);           
        }

        public List<Quest> LoadQuestLog()
        {
            LoadGame("TestSave.bytes");
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
        
        #endregion

        #region Items

        public void NewEntry(Item item)
        {
            if (item.EntryId == 0)
            {
                item.SetEntryId(_newEntry);
                _newEntry++;
                return;
            }

            if (item.EntryId >= _newEntry)
            {
                _newEntry = item.EntryId + 1;
            }
        }

        public List<Item> LoadInventory()
        {
            var res = new List<Item>();
            foreach (var dto in _saveFileWrapper.LoadInventory())
            {
                var t = _itemStorage.GetItemById(dto.ItemId);
                t.SetEntryId(dto.Entry);
                t.AddItemCount(dto.Count-1);
                t.ScriptUsed = dto.ScriptUsed;
                t.ReduceDuration(t.MaxDuration-dto.TimeLeft);
                t.ReduceDurability(t.MaxDurability-dto.Durability);
                res.Add(t);
            }

            return res;
        }

        public Dictionary<int, Item> LoadEquipment()
        {
            var res = new Dictionary<int, Item>();
            foreach (var dto in _saveFileWrapper.LoadInventory())
            {
                var t = _itemStorage.GetItemById(dto.ItemId);
                t.SetEntryId(dto.Entry);
                t.AddItemCount(dto.Count-1);
                t.ScriptUsed = dto.ScriptUsed;
                t.ReduceDuration(t.MaxDuration-dto.TimeLeft);
                t.ReduceDurability(t.MaxDurability-dto.Durability);
                res.Add(dto.Slot, t);
            }

            return res;
        }

        public void SaveEquipment(Dictionary<int, Item> equipment)
        {
            var dto = equipment.Select(i => new SaveItemDto
            {
                Count = i.Value.Count,
                Durability = i.Value.CurrentDurability,
                Entry = i.Value.EntryId,
                ItemId = i.Value.Id,
                ScriptUsed = i.Value.ScriptUsed,
                //SpellCharges1 
                //SpellCharges2 
                TimeLeft = (int)i.Value.CurrentDuration,
                Slot = i.Key
            });
            _saveFileWrapper.SaveEquipment(dto);
        }

        public void SaveInventory(List<Item> inventory)
        {
            var dto = inventory.Select(i => new SaveItemDto
            {
                Count = i.Count,
                Durability = i.CurrentDurability,
                Entry = i.EntryId,
                ItemId = i.Id,
                ScriptUsed = i.ScriptUsed,
                //SpellCharges1 
                //SpellCharges2 
                TimeLeft = (int)i.CurrentDuration
            });
            _saveFileWrapper.SaveInventory(dto);
        }
        #endregion
    }
}