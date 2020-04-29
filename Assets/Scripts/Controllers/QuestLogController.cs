using System;
using System.Collections.Generic;
using System.Linq;
using BaseScripts;
using Events;
using Events.Args;
using Interfaces;
using Quests;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    ///     Управление квестлогом
    /// </summary>
    public class QuestLogController : BaseController
    {
        /// <summary>
        ///     List of currently accepted quests
        /// </summary>
        private readonly List<Quest> _quests;

        private readonly List<int> _completedQuests;
        /// <summary>
        ///     Data access layer for quests
        /// </summary>
        private readonly IQuestStorage _questStorage;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="questStorage">Прослойка для получения данных о квестах</param>
        public QuestLogController(IQuestStorage questStorage)
        {
            _questStorage = questStorage;
            _quests = _questStorage.LoadQuestLog();
            _completedQuests = _questStorage.GetAllCompletedQuests();
            EventManager.StartListening(GameEventTypes.QuestAccepted, OnQuestAccept);
            EventManager.StartListening(GameEventTypes.NpcDie, OnNpcDie);
            EventManager.StartListening(GameEventTypes.AreaEnter, OnAreaEnter);
            EventManager.StartListening(GameEventTypes.Saving, OnProgressSaving);
            EventManager.StartListening(GameEventTypes.QuestAbandoned, OnQuestAbandon);
            EventManager.StartListening(GameEventTypes.QuestReported, OnQuestReport);
            EventManager.StartListening(GameEventTypes.ItemAcquired, OnItemAcquired);
            EventManager.StartListening(GameEventTypes.DialogStarted, OnDialogEnter);
            EventManager.StartListening(GameEventTypes.ItemUsed, OnItemUse);
            EventManager.StartListening(GameEventTypes.ObjectUsed, OnObjectUse);
            EventManager.StartListening(GameEventTypes.DialogAnswerSelect, OnDialogAnswerSelect);
        }

        /// <summary>
        ///     Количество квестов в логе
        /// </summary>
        public int QuestCount => _quests.Count;

        /// <summary>
        ///     List of currently accepted quests
        /// </summary>
        public IEnumerable<Quest> Quests => _quests.AsReadOnly();
        public List<int> CompletedQuests => _completedQuests;
        /// <summary>
        ///     When world object used
        /// </summary>
        /// <param name="arg0">IdArgs of object</param>
        private void OnObjectUse(EventArgs arg0)
        {
            if (!(arg0 is IdArgs itemArgs)) return;
            QuestUpdate(QuestTaskTypes.UseObject, itemArgs.Id);
        }

        /// <summary>
        ///     When quest item used
        /// </summary>
        /// <param name="arg0">ItemArgs</param>
        private void OnItemUse(EventArgs arg0)
        {
            if (!(arg0 is ItemArgs itemArgs)) return;
            QuestUpdate(QuestTaskTypes.UseItem, itemArgs.Item.Id);
        }

        /// <summary>
        ///     When Dialog Started
        /// </summary>
        /// <param name="arg0">DialogArgs</param>
        private void OnDialogEnter(EventArgs arg0)
        {
            if (!(arg0 is DialogArgs dialogArgs)) return;
            Debug.Log($"QuestLogController>>> Quest dialogue talknpc {dialogArgs.NpcId}");
            QuestUpdate(QuestTaskTypes.TalkWithNpc, dialogArgs.NpcId);
        }
        /// <summary>
        ///     When Dialog Answer Select
        /// </summary>
        /// <param name="arg0">DialogArgs</param>
        private void OnDialogAnswerSelect(EventArgs arg0)
        {
            if (!(arg0 is DialogArgs dialogArgs)) return;
           // Debug.Log($"QuestLogController>>> Quest dialogue talknpc {dialogArgs.Id}");
            QuestUpdate(QuestTaskTypes.AnswerSelect, dialogArgs.Id);
        }

        /// <summary>
        ///     When item acquired
        /// </summary>
        /// <param name="arg0">ItemArgs</param>
        private void OnItemAcquired(EventArgs arg0)
        {
            if (!(arg0 is ItemArgs itemArgs)) return;
            QuestUpdate(QuestTaskTypes.CollectItem, itemArgs.Item.Id);
        }

        /// <summary>
        ///     When quest reported for reward
        /// </summary>
        /// <param name="arg0">id of quest</param>
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
            StartScript.GetStartScript._saveManager.SaveGame("TestSave.bytes");
#endif
        }

        /// <summary>
        ///     When player abandons quest
        /// </summary>
        /// <param name="arg0">id of quest</param>
        private void OnQuestAbandon(EventArgs arg0)
        {
            if (!(arg0 is IdArgs idArgs)) return;
            var t = _quests.Find(x => x.Id == idArgs.Id);
            if (t != null)
                _quests.Remove(t);
        }

        /// <summary>
        ///     When game saved
        /// </summary>
        /// <param name="arg0">---</param>
        private void OnProgressSaving(EventArgs arg0)
        {
            _questStorage.SaveQuestLog(_quests);
        }

        /// <summary>
        ///     When accepting quest
        /// </summary>
        /// <param name="args">id of quest</param>
        private void OnQuestAccept(EventArgs args)
        {
            if (!(args is IdArgs idArgs)) return;
            var t = _questStorage.GetQuestById(idArgs.Id);
            if (t != null)
                _quests.Add(t);
            #if UNITY_EDITOR
            Debug.Log($"QuestLogController>>> Quest [{idArgs.Id}] Accepted");
            #endif
        }

        /// <summary>
        ///     Returns all currently accepted quests for specific zone
        /// </summary>
        /// <param name="zoneId">id of zone</param>
        /// <returns></returns>
        public List<Quest> GetByZone(int zoneId)
        {
            return _quests.FindAll(x => x.ZoneId == zoneId);
        }

        /// <summary>
        ///     Returns all currently accepted quests with specific type of objective
        /// </summary>
        /// <param name="type">type of objective</param>
        /// <returns></returns>
        public List<Quest> GetByTaskType(QuestTaskTypes type)
        {
            return _quests.FindAll(x => x.Tasks.Any(y => y.Type == type));
        }

        /// <summary>
        ///     Returns all accepted and tracked quests
        /// </summary>
        /// <returns></returns>
        public List<Quest> GetTracked()
        {
            return _quests.FindAll(x => x.IsTracked);
        }

        /// <summary>
        ///     updates status of quests
        /// </summary>
        /// <param name="eventType">Which event triggered (task type)</param>
        /// <param name="targetId">Id of target</param>
        /// <param name="amount">Amount of targets</param>
        private void QuestUpdate(QuestTaskTypes eventType, int targetId, int amount = 1)
        {
            foreach (var quest in GetByTaskType(eventType))
            {
                foreach (var task in quest.Tasks)
                {
                    if (task.Type != eventType || task.TargetId != targetId) continue;
                    task.AddAmount(amount);
#if UNITY_EDITOR
                    Debug.Log($"QuestLogController>>> Task [{task.Id}] [{quest.Tasks[0].CurrentAmount}] from quest [{quest.Id}] updated");
#endif
                }

                if (quest.IsComplete)
                {
                    EventManager.TriggerEvent(GameEventTypes.QuestCompleted, new IdArgs(quest.Id));
                    Debug.Log($"QuestLogController>>> Quest [{quest.Id}] Complete");
                }
            }
        }

        /// <summary>
        ///     When enemy dies
        /// </summary>
        /// <param name="args">id of enemy</param>
        private void OnNpcDie(EventArgs args)
        {
            if (!(args is EnemyDieArgs dieArgs)) return;
            QuestUpdate(QuestTaskTypes.KillNpc, dieArgs.Id);
            QuestUpdate(QuestTaskTypes.KillEnemyFamily, dieArgs.FamilyId);
        }

        /// <summary>
        ///     When character enters in specific zone
        /// </summary>
        /// <param name="args">id of zone</param>
        private void OnAreaEnter(EventArgs args)
        {
            if (!(args is IdArgs idArgs)) return;
            QuestUpdate(QuestTaskTypes.FindLocation, idArgs.Id);
        }

        /// <summary>
        ///     Main Update Loop
        /// </summary>
        //TODO: receive deltaTime from StartScript 
        public override void ControllerUpdate()
        {
            foreach (var quest in _quests)
            {
                if (!quest.IsTimed) continue;
                //TODO: Time.deltaTime -> deltaTime from StartScript
                quest.ReduceTime(Time.deltaTime);
            }
        }
    }
}