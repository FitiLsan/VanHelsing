using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BeastHunter
{
    public sealed class QuestModel
    {
        #region Fields

        public static event Action<EventArgs> ReceiveCardEvent;

        private readonly List<Quest> _quests;
        private readonly List<Quest> _completedQuest;
        private readonly List<QuestTask> _completedTasks;
        private readonly List<Quest> _generatedQuest;
        public readonly IQuestStorage QuestStorage;

        public int QuestCount => _quests.Count;
        public IEnumerable<Quest> Quests => _quests.AsReadOnly();
        public List<Quest> QuestsList => _quests;
        public GameContext Context;
        public List<int> AllTaskCompletedInQuests = new List<int>();
        public List<int> AllTaskCompletedInQuestsWithOptional = new List<int>();
        public List<int> CompletedTasksById = new List<int>();
        public List<int> IsOptionalTasks = new List<int>();
        public Quest TempQuest;
        public int lastGeneratedQuestId;
        public List<QuestChain> questChainList = new List<QuestChain>();

        #endregion


        #region Properties

        public List<int> CompletedQuests { get; }
        public List<int> ActiveQuests { get; }

        #endregion


        #region ClassLifeCycle

        public QuestModel(IQuestStorage questStorage, GameContext context)
        {

            Context = context;
            QuestStorage = questStorage;
            QuestStorage.LoadGame("TestSave.bytes");
            _quests = QuestStorage.GetAllActiveQuests();
            _generatedQuest = QuestStorage.GetAllGeneratedQuest();
            _completedQuest = QuestStorage.GetAllCompletedQuests();
            _completedTasks = GetCompleteTasks(_completedQuest,_quests);
            ActiveQuests = QuestStorage.GetAllActiveQuestsById();
            CompletedQuests = QuestStorage.GetAllCompletedQuestsById();
            CheckLastGeneratedQuestId();
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestAccepted, OnQuestAccept);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.NpcDie, OnNpcDie);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.AreaEnter, OnAreaEnter);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.Saving, OnProgressSaving);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestAbandoned, OnQuestAbandon);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestReported, OnQuestReport);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.DialogStarted, OnDialogEnter);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.ObjectUsed, OnObjectUse);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.DialogAnswerSelect, OnDialogAnswerSelect);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.SaveGeneratedQuest, OnSaveGeneratedQuest);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.ItemAcquired, OnItemAcquired);
            //  EventManager.StartListening(GameEventTypes.ItemUsed, OnItemUse);
            // не удалять, события для предметов
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
            //test cardindicator
          //  Services.SharedInstance.EventManager.StartListening(GameEventTypes.ReceivingNewCard, CardReceiveIndication.GetCard);
        }

        #endregion


        #region Methods

        private void OnProgressSaving(EventArgs arg0)
        {   
            QuestStorage.SaveQuestLog(_quests);
           //QuestStorage.SaveGeneratedQuest(QuestRepository.GetById(QuestGeneration.GetTempQuest().Id));
        }

        private void OnDialogEnter(EventArgs arg0)
        {
            if (!(arg0 is DialogArgs dialogArgs)) return;
            QuestUpdate(QuestTaskTypes.TalkWithNpc, dialogArgs.NpcId);
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
        }

        private void OnDialogAnswerSelect(EventArgs arg0)
        {
            if (!(arg0 is DialogArgs dialogArgs)) return;
            QuestUpdate(QuestTaskTypes.AnswerSelect, dialogArgs.Id);
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
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
            QuestStorage.QuestCompleted(t.Id);
            AllTaskCompletedInQuests.Remove(t.Id);
            AllTaskCompletedInQuestsWithOptional.Remove(t.Id);
            _quests.Remove(t);
            ActiveQuests.Remove(t.Id);
            _completedQuest.Add(t);
            if (t.IsRepetable != 0)
            {
                SetQuestIsNotComplete(t.Id);
                _completedQuest.Remove(t);
                CompletedQuests.Remove(t.Id);
                foreach(var task in t.Tasks)
                {
                    CompletedTasksById.Remove(task.Id);
                    _completedTasks.Remove(task);
                }              
            }
#if UNITY_EDITOR
            Debug.Log($"QuestLogController>>> Quest [{idArgs.Id}] Reported");
            Debug.Log("Game saved");
#endif            
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.ReceivingNewCard, new CardArgs(1));
           // ReceiveCardEvent?.Invoke(new CardArgs(1));
            QuestStorage.SaveGame("TestSave.bytes");
            if (QuestGeneration.GetTempQuest() != null)
            {
                if (QuestGeneration.GetTempQuest().Id.Equals(idArgs.Id))
                {
                    QuestGeneration.ClearTempQuest();
                    TempQuest = null;
                }
            }
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
        }

        private void OnQuestAbandon(EventArgs arg0)
        {           
            if (!(arg0 is IdArgs idArgs)) return;
            var t = _quests.Find(x => x.Id == idArgs.Id);
            if (t != null)
                _quests.Remove(t);
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
        }

        private void OnQuestAccept(EventArgs args)
        {         
            if (!(args is IdArgs idArgs)) return;
             TempQuest = idArgs.Id.Equals(666)?new Quest(QuestGeneration.GetTempQuest()) : QuestStorage.GetQuestById(idArgs.Id);
            if (TempQuest != null)
            {
                if (_quests.Count != 0)
                {
                    foreach (Quest quest in _quests)
                    {
                        if (quest.Id == TempQuest.Id)
                        {
                            Debug.Log($"QuestLogController>>> Quest [{idArgs.Id}] Already Accepted");
                            return;
                        }              
                    }
                }
                foreach (var requiredQuest in TempQuest.RequiredQuests)
                {
                    if (!CompletedQuests.Contains(requiredQuest))
                    {
                        Debug.Log($"QuestLogController>>> to accept Quest [{idArgs.Id}] need to Complete Quest [{requiredQuest}]");
                        return;
                    }
                }
                foreach (var forbiddenQuest in TempQuest.ForbiddenQuests)
                {
                    if (CompletedQuests.Contains(forbiddenQuest))
                    {
                        Debug.Log($"QuestLogController>>> can't accept Quest [{idArgs.Id}] forbidden by Quest [{forbiddenQuest}]");
                        return;
                    }
                }


                _quests.Add(TempQuest);
                ActiveQuests.Add(TempQuest.Id);
                var hasChain = false;
                foreach(var questChain in questChainList)
                {
                    if (questChain.ChainId == TempQuest.ChainId)
                    {
                        questChain.QuestListId.Add(TempQuest.Id);
                        hasChain = true;
                        break;
                    }
                }
                if (!hasChain)
                {
                    var tempChain = new QuestChain(TempQuest.ChainId);
                    tempChain.QuestListId.Add(TempQuest.Id);
                    questChainList.Add(tempChain);
                }
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestJournalUpdated, args);
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
#if UNITY_EDITOR
                Debug.Log($"QuestLogController>>> Quest [{idArgs.Id}] Accepted");
#endif
            }
        }

        private void QuestUpdate(QuestTaskTypes eventType, int targetId, int amount = 1)
        {
            foreach (var quest in GetByTaskType(eventType))
            {
                foreach (var task in quest.Tasks)
                {
                    if (task.Type != eventType || task.TargetId != targetId) continue;
                    if (!task.IsCompleted)
                    {
                        task.AddAmount(amount);
                    }
                    if (task.IsCompleted)
                    {
                        CompletedTasksById.Add(task.Id);
                        _completedTasks.Add(task);
                    }
                    if(task.IsOptional)
                    {
                        IsOptionalTasks.Add(task.Id);
                    }
#if UNITY_EDITOR
                    Debug.Log($"QuestLogController>>> Task ID:[{task.Id}] ({quest.Tasks.IndexOf(task) + 1} out of {quest.Tasks.Count}) [{task.CurrentAmount} out of {task.NeededAmount}] from quest ID:[{quest.Id}] updated");
#endif
                }

                if (quest.IsComplete)
                {
                    if (!AllTaskCompletedInQuestsWithOptional.Contains(quest.Id) & quest.HasOptionalTasks)
                    {
                        AllTaskCompletedInQuestsWithOptional.Add(quest.Id);
                    }
                    if (!AllTaskCompletedInQuests.Contains(quest.Id) & !quest.HasOptionalTasks)
                    {
                        AllTaskCompletedInQuests.Add(quest.Id);
                    }
                    Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestCompleted, new IdArgs(quest.Id));
                    Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
#if UNITY_EDITOR
                    Debug.Log($"QuestLogController>>> Quest ID:[{quest.Id}] Complete");
#endif
                }
            }
        }

        private void OnNpcDie(EventArgs args)
        {           
            if (!(args is EnemyDieArgs dieArgs)) return;
            QuestUpdate(QuestTaskTypes.KillNpc, dieArgs.Id);
            QuestUpdate(QuestTaskTypes.KillEnemyFamily, dieArgs.FamilyId);

            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
#if UNITY_EDITOR
            Debug.Log($"NPC with ID:{dieArgs.Id}");
#endif
        }

        private void OnAreaEnter(EventArgs args)
        {
            if (!(args is IdArgs idArgs)) return;
            QuestUpdate(QuestTaskTypes.FindLocation, idArgs.Id);
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
        }

        private void OnObjectUse(EventArgs arg0)
        {
            if (!(arg0 is IdArgs itemArgs)) return;
            QuestUpdate(QuestTaskTypes.UseObject, itemArgs.Id);
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
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

        public Quest GetActualQuestById(int id)
        {
            Quest curQuest = null;
            foreach(Quest quest in _quests)
            {
                if( quest.Id == id)
                {
                    curQuest = quest;
                    break;
                }
            }
            foreach (Quest quest in _completedQuest)
            {
                if (quest.Id == id)
                {
                    curQuest = quest;
                    break;
                }
            }
            return curQuest;
        }
        private void OnItemAcquired(EventArgs arg0)
        {
            if (!(arg0 is ItemArgs itemArgs)) return;
            QuestUpdate(QuestTaskTypes.CollectItem, itemArgs.ItemId);
        }

        //private void OnItemUse(EventArgs arg0)
        //{
        //    if (!(arg0 is ItemArgs itemArgs)) return;
        //    QuestUpdate(QuestTaskTypes.UseItem, itemArgs.Item.Id);
        //}
        //не удалять, события для предметов

        public void QuestTimeChecking()
        {
            foreach (var quest in _quests)
            {
                if (!quest.IsTimed) continue;
                //TODO: Time.deltaTime -> deltaTime from StartScript
                quest.ReduceTime(Time.deltaTime);
            }
        }

        public List<QuestTask> GetCompleteTasks(List<Quest> completedQuests, List<Quest> quests)
        {
            var completedTasks = new List<QuestTask>();

            foreach (var quest in completedQuests)
            {
                foreach (var task in quest.Tasks)
                {
                    completedTasks.Add(task);
                    CompletedTasksById.Add(task.Id);
                }
            }
            foreach (var quest in quests)
            {
                foreach (var task in quest.Tasks)
                {
                    if (!completedTasks.Contains(task) & task.NeededAmount==task.CurrentAmount)
                    {                       
                        completedTasks.Add(task);
                        if (!CompletedTasksById.Contains(task.Id))
                        {
                            CompletedTasksById.Add(task.Id);
                        }
                    }
                }
            }           
            return completedTasks;
        }

        public void SetQuestIsNotComplete(int questId)
        {
            //need del?
        }    
        
        public void OnSaveGeneratedQuest(EventArgs args)
        {
            if (!(args is QuestArgs questArgs)) return;
           // QuestStorage.SaveGeneratedQuest(questArgs.Quest);
            _generatedQuest.Add(new Quest(questArgs.Quest));
        }
        public void CheckLastGeneratedQuestId()
        {
            QuestGeneration.LoadLastGeneratedId();
            if(!CompletedQuests.Contains(QuestGeneration.lastGeneratedQuestId))
            {
                QuestGeneration.SetTempLastGeneratedQuest();
            }
        }

        public void Execute()
        {
            QuestTimeChecking();
        }

        #endregion
    }
}
