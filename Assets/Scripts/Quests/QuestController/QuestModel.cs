using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BeastHunter
{
    public sealed class QuestModel
    {
        #region Fields

        public readonly List<Quest> _quests;
        private readonly IQuestStorage _questStorage;

        public int QuestCount => _quests.Count;
        public IEnumerable<Quest> Quests => _quests.AsReadOnly();
        public GameContext Context;
        public List<int> AllTaskCompletedInQuests = new List<int>();

        #endregion


        #region Properties

        public List<int> CompletedQuests { get; }
    //    public List<int> AllTaskCompletedInQuests { get; }

        #endregion


        #region ClassLifeCycle

        public QuestModel(IQuestStorage questStorage, GameContext context)
        {   
            
            Context = context;
            _questStorage = questStorage;
            _questStorage.LoadGame("TestSave.bytes");
            _quests = _questStorage.GetAllActiveQuests();
            CompletedQuests = _questStorage.GetAllCompletedQuests();
            EventManager.StartListening(GameEventTypes.QuestAccepted, OnQuestAccept);
            EventManager.StartListening(GameEventTypes.NpcDie, OnNpcDie);
            EventManager.StartListening(GameEventTypes.AreaEnter, OnAreaEnter);
            EventManager.StartListening(GameEventTypes.Saving, OnProgressSaving);
            EventManager.StartListening(GameEventTypes.QuestAbandoned, OnQuestAbandon);
            EventManager.StartListening(GameEventTypes.QuestReported, OnQuestReport);          
            EventManager.StartListening(GameEventTypes.DialogStarted, OnDialogEnter);
            EventManager.StartListening(GameEventTypes.ObjectUsed, OnObjectUse);
            EventManager.StartListening(GameEventTypes.DialogAnswerSelect, OnDialogAnswerSelect);
            //  EventManager.StartListening(GameEventTypes.ItemAcquired, OnItemAcquired);
            //  EventManager.StartListening(GameEventTypes.ItemUsed, OnItemUse);
        }

        #endregion


        #region Methods

        private void OnObjectUse(EventArgs arg0)
        {
            if (!(arg0 is IdArgs itemArgs)) return;
            QuestUpdate(QuestTaskTypes.UseObject, itemArgs.Id);
        }

        private void OnDialogEnter(EventArgs arg0)
        {
            if (!(arg0 is DialogArgs dialogArgs)) return;
            QuestUpdate(QuestTaskTypes.TalkWithNpc, dialogArgs.NpcId);
        }

        private void OnDialogAnswerSelect(EventArgs arg0)
        {
            if (!(arg0 is DialogArgs dialogArgs)) return;
            QuestUpdate(QuestTaskTypes.AnswerSelect, dialogArgs.Id);
        }

        private void OnQuestReport(EventArgs arg0)
        {
            if (!(arg0 is IdArgs idArgs)) return;
            var t = _quests.Find(x => x.Id == idArgs.Id);
            if (t == null) return;
            if (!t.IsComplete)
            {
                Debug.Log($"QuestLogController>>> Quest [{idArgs.Id}] Can't Reported, Quest is not complete");
                return;
            }
            _questStorage.QuestCompleted(t.Id);
            _quests.Remove(t);
#if UNITY_EDITOR
            Debug.Log($"QuestLogController>>> Quest [{idArgs.Id}] Reported");
            _questStorage.SaveGame("TestSave.bytes");
            Debug.Log("Game saved");
#endif
        }

        private void OnQuestAbandon(EventArgs arg0)
        {
            if (!(arg0 is IdArgs idArgs)) return;
            var t = _quests.Find(x => x.Id == idArgs.Id);
            if (t != null)
                _quests.Remove(t);
        }

        private void OnProgressSaving(EventArgs arg0)
        {
            _questStorage.SaveQuestLog(_quests);
        }

        private void OnQuestAccept(EventArgs args)
        {
            if (!(args is IdArgs idArgs)) return;
            var t = _questStorage.GetQuestById(idArgs.Id);
            if (t != null)
            {
                if (_quests.Count != 0)
                {
                    foreach (Quest quest in _quests)
                    {
                        if (quest.Id == t.Id)
                        {
                            Debug.Log($"QuestLogController>>> Quest [{idArgs.Id}] Already Accepted");
                            return;
                        }
                    }
                }
                _quests.Add(t);
                Debug.Log($"QuestLogController>>> Quest [{idArgs.Id}] Accepted");

            }
#if UNITY_EDITOR
            
#endif
        }

        public List<Quest> GetByZone(int zoneId)
        {
            return _quests.FindAll(x => x.ZoneId == zoneId);
        }

        public List<Quest> GetByTaskType(QuestTaskTypes type)
        {
            return _quests.FindAll(x => x.Tasks.Any(y => y.Type == type));
        }

        public List<Quest> GetTracked()
        {
            return _quests.FindAll(x => x.IsTracked);
        }

        private void QuestUpdate(QuestTaskTypes eventType, int targetId, int amount = 1)
        {
            foreach (var quest in GetByTaskType(eventType))
            {
                foreach (var task in quest.Tasks)
                {
                    if (task.Type != eventType || task.TargetId != targetId) continue;
                    if (task.CurrentAmount!=task.NeededAmount)
                    {
                        task.AddAmount(amount);
                    }
#if UNITY_EDITOR
                    Debug.Log($"QuestLogController>>> Task ID:[{task.Id}] [{task.CurrentAmount} out of {task.NeededAmount}] from quest ID:[{quest.Id}] updated");
#endif
                }

                if (quest.IsComplete)
                {
                    AllTaskCompletedInQuests.Add(quest.Id);
                    EventManager.TriggerEvent(GameEventTypes.QuestCompleted, new IdArgs(quest.Id));
                    Debug.Log($"QuestLogController>>> Quest ID:[{quest.Id}] Complete");
                }
            }
        }

        private void OnNpcDie(EventArgs args)
        {
            if (!(args is EnemyDieArgs dieArgs)) return;
            QuestUpdate(QuestTaskTypes.KillNpc, dieArgs.Id);
            QuestUpdate(QuestTaskTypes.KillEnemyFamily, dieArgs.FamilyId);
            Debug.Log($"NPC with ID:{dieArgs.Id}");
        }

        private void OnAreaEnter(EventArgs args)
        {
            if (!(args is IdArgs idArgs)) return;
            QuestUpdate(QuestTaskTypes.FindLocation, idArgs.Id);
        }

        //private void OnItemAcquired(EventArgs arg0)
        //{
        //    if (!(arg0 is ItemArgs itemArgs)) return;
        //    QuestUpdate(QuestTaskTypes.CollectItem, itemArgs.Item.Id);
        //}

        //private void OnItemUse(EventArgs arg0)
        //{
        //    if (!(arg0 is ItemArgs itemArgs)) return;
        //    QuestUpdate(QuestTaskTypes.UseItem, itemArgs.Item.Id);
        //}

        public void QuestTimeChecking()
        {
            foreach (var quest in _quests)
            {
                if (!quest.IsTimed) continue;
                //TODO: Time.deltaTime -> deltaTime from StartScript
                quest.ReduceTime(Time.deltaTime);
            }
        }

        public void Execute()
        {
            QuestTimeChecking();
        }
        #endregion
    }
}
