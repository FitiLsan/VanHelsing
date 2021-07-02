using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    ///<summary>
    /// Important note to understand what this quest system is and why it is here.
    /// 1) This quest system was not created in accordance with the game design doc!
    /// 2) This system was done simply because the programmer was interested in trying something like that. 
    /// 3) This system allows to create simple quests from citizens in cities.
    /// 4) And the most important:
    ///    Most likely system has flaws that need to be fixed OR it might be better in the future to just cut that system out from code.
    /// </summary>

    public class QuestController
    {
        #region PrivateData

        private enum RequirementCheckType
        {
            None = 0,
            CityReputation = 1,
            QuestComplete = 2,
        }

        #endregion


        #region Fields

        private HubUIContext _context;
        private List<QuestModel> _notActiveQuests;
        private List<QuestModel> _activeQuests;
        private List<QuestModel> _completedQuests;

        #endregion


        #region ClassLifeCycle

        public QuestController(QuestSO[] quests ,HubUIContext context)
        {
            _context = context;

            _notActiveQuests = new List<QuestModel>();
            _activeQuests = new List<QuestModel>();
            _completedQuests = new List<QuestModel>();

            for (int i = 0; i < quests.Length; i++)
            {
                _notActiveQuests.Add(new QuestModel(quests[i], QuestStatus.NotActive));
            }

            for (int i = 0; i < _notActiveQuests.Count; i++)
            {
                if (CheckQuestForAllRequirements(_notActiveQuests[i], RequirementCheckType.QuestComplete))
                {
                    QuestActivate(_notActiveQuests[i]);
                }
            }

            for (int i = 0; i < _context.Cities.Count; i++)
            {
                _context.Cities[i].OnChangePlayerReputationHandler += OnChangePlayerReputation;
            }
        }

        #endregion


        #region Methods

        private void OnChangePlayerReputation(CityModel city)
        {
            for (int i = 0; i < _notActiveQuests.Count; i++)
            {
                if (_notActiveQuests[i].IsEnoughCityReputation(city))
                {
                    if (CheckQuestForAllRequirements(_notActiveQuests[i], RequirementCheckType.CityReputation))
                    {
                        QuestActivate(_notActiveQuests[i]);
                    }
                }
            }
        }

        private QuestModel GetQuestModel(QuestSO questData)
        {
            QuestModel result;
            result = _activeQuests.Find(quest => quest.Data == questData);
            if (result != null)
            {
                return result;
            }

            result = _notActiveQuests.Find(quest => quest.Data == questData);
            if (result != null)
            {
                return result;
            }

            result = _completedQuests.Find(quest => quest.Data == questData);
            return result;
        }

        private void QuestActivate(QuestModel quest)
        {
            if (quest.Status == QuestStatus.NotActive)
            {
                    quest.Status = QuestStatus.Active;

                    _activeQuests.Add(quest);
                    _notActiveQuests.Remove(quest);

                    OnQuestActivated(quest);
                    Debug.Log("Change quest status on active");
            }
        }

        private void QuestComplete(QuestModel quest)
        {
            if (quest.Status == QuestStatus.Active)
            {
                quest.Status = QuestStatus.Completed;

                _activeQuests.Remove(quest);
                _completedQuests.Add(quest);

                OnQuestComplete(quest);
                Debug.Log("Change quest status on completed");
            }
        }

        private void QuestProgressing(QuestModel quest)
        {
            DeactivateQuestAnswers(quest);
            SetMarkerTypeToCitizen(quest, QuestMarkerType.None);

            bool isTaskConditionCompleted =
                IsGiveItemToPlayerTaskConditionComplete(quest) &&
                IsTakeItemFromPlayerTaskConditionComplete(quest);

            if (isTaskConditionCompleted)
            {
                quest.NextTask();
            }

            if (quest.IsLastTaskComplete())
            {
                QuestComplete(quest);
            }
            else
            {
                SetQuestDialogToTargetCitizen(quest);
                ActivateQuestAnswers(quest);
                SetMarkerTypeToCitizen(quest, QuestMarkerType.Question);
            }
        }

        public bool IsTakeItemFromPlayerTaskConditionComplete(QuestModel quest)
        {
            if (quest.CurrentTask.TakenItemData != null)
            {
                if (_context.Player.Inventory.RemoveFirstItem(quest.CurrentTask.TakenItemData))
                {
                    Debug.Log("The player gave the item " + quest.CurrentTask.TakenItemData.Name);
                    return true;
                }
                else
                {
                    Debug.Log("The player does not have item for quest");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public bool IsGiveItemToPlayerTaskConditionComplete(QuestModel quest)
        {
            if (quest.CurrentTask.GivenItemData != null)
            {
                BaseItemModel itemModel = HubUIServices.SharedInstance.
                    ItemInitializeService.InitializeItemModel(quest.CurrentTask.GivenItemData);
                if (_context.Player.Inventory.PutElementToFirstEmptySlot(itemModel))
                {
                    Debug.Log("The player received the item " + quest.CurrentTask.GivenItemData.Name);
                    return true;
                }
                else
                {
                    Debug.Log("The player inventory is full for get the quest item");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private bool CheckQuestForAllRequirements(QuestModel quest, RequirementCheckType excludeCheckType = RequirementCheckType.None)
        {
            if (excludeCheckType != RequirementCheckType.CityReputation)
            {
                CitySO citySO = quest.Data.RequiredReputation.City;
                bool checkReputationRequirement = quest.IsEnoughCityReputation(_context.GetCityById(citySO.CityData.MapObjectData.InstanceId));

                if (!checkReputationRequirement)
                {
                    return false;
                }
            }

            if (excludeCheckType != RequirementCheckType.QuestComplete)
            {
                QuestSO questData = quest.Data.RequiredQuest;
                bool checkQuestRequirement = quest.IsRequirementQuestComleted(GetQuestModel(questData));

                if (!checkQuestRequirement)
                {
                    return false;
                }
            }

            return true;
        }

        private void OnQuestActivated(QuestModel quest)
        {
            SetQuestDialogToTargetCitizen(quest);
            ActivateQuestAnswers(quest);
            SetMarkerTypeToCitizen(quest, QuestMarkerType.Exclamation);
        }

        private void OnQuestComplete(QuestModel quest)
        {
            for (int i = 0; i < _notActiveQuests.Count; i++)
            {
                if (_notActiveQuests[i].IsRequirementQuestComleted(quest))
                {
                    if (CheckQuestForAllRequirements(_notActiveQuests[i], RequirementCheckType.QuestComplete))
                    {
                        QuestActivate(_notActiveQuests[i]);
                    }
                }
            }
        }

        private void SetQuestDialogToTargetCitizen(QuestModel quest)
        {
            if (quest.CurrentTask.IsCitizenInitiateDialog)
            {
                _context.GetCitizenById(quest.CurrentTask.TargetCitizen.InstanceId).
                    SetCurrentDialogNode(quest.CurrentTask.InitiatedDialogId);
            }
        }

        private void ActivateQuestAnswers(QuestModel quest)
        {
            QuestAnswer questAnswer = _context.GetCitizenById(quest.CurrentTask.TargetCitizen.InstanceId).
                GetQuestAnswerById(quest.CurrentTask.TargetQuestAnswerId);

            if (questAnswer != null)
            {
                questAnswer.IsActivated = true;
                questAnswer.SetIntractableHandler += (answer) => SetInteractableQuestAnswer(answer, quest);
            }
            else
            {
                Debug.LogError(quest.CurrentTask.TargetCitizen.Name + " does not have requested quest answer id " + quest.CurrentTask.TargetQuestAnswerId);
            }

            questAnswer.Answer.OnAnswerSelectByPlayerHandler += (handlerValue) => QuestProgressing(quest);

            for (int i = 0; i < quest.CurrentTask.AdditionalCitizensAnswers.Length; i++)
            {
                questAnswer = _context.GetCitizenById(quest.CurrentTask.AdditionalCitizensAnswers[i].Citizen.InstanceId).
                    GetQuestAnswerById(quest.CurrentTask.AdditionalCitizensAnswers[i].QuestAnswerId);
                
                if (questAnswer != null)
                {
                    questAnswer.IsActivated = true;
                }
                else
                {
                    Debug.LogError(quest.CurrentTask.AdditionalCitizensAnswers[i].Citizen.Name + " does not have requested quest answer id " + quest.CurrentTask.AdditionalCitizensAnswers[i].QuestAnswerId);
                }
            }
        }

        private void DeactivateQuestAnswers(QuestModel quest)
        {
            QuestAnswer questAnswer = _context.GetCitizenById(quest.CurrentTask.TargetCitizen.InstanceId).
                        GetQuestAnswerById(quest.CurrentTask.TargetQuestAnswerId);
            questAnswer.IsActivated = false;
            questAnswer.Answer.OnAnswerSelectByPlayerHandler -= (handlerValue) => QuestProgressing(quest);

            for (int i = 0; i < quest.CurrentTask.AdditionalCitizensAnswers.Length; i++)
            {
                questAnswer = _context.GetCitizenById(quest.CurrentTask.AdditionalCitizensAnswers[i].Citizen.InstanceId).
                    GetQuestAnswerById(quest.CurrentTask.AdditionalCitizensAnswers[i].QuestAnswerId);
                questAnswer.IsActivated = false;
            }
        }

        private void SetMarkerTypeToCitizen(QuestModel quest, QuestMarkerType questMarker)
        {
            _context.GetCitizenById(quest.CurrentTask.TargetCitizen.InstanceId).QuestMarkerType = questMarker;
        }

        private void SetInteractableQuestAnswer(QuestAnswer questAnswer, QuestModel quest)
        {
            questAnswer.Answer.IsInteractable =
                (quest.CurrentTask.GivenItemData == null ||
                quest.CurrentTask.GivenItemData != null && !_context.Player.Inventory.IsFull())
                &&
                (quest.CurrentTask.TakenItemData == null ||
                quest.CurrentTask.TakenItemData != null && _context.Player.Inventory.IsContainItem(quest.CurrentTask.TakenItemData));
        }

        #endregion
    }
}
