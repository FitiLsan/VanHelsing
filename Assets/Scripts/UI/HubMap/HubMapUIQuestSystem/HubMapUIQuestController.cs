using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class HubMapUIQuestController
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

        private HubMapUIContext _context;
        private List<HubMapUIQuestModel> _notActiveQuests;
        private List<HubMapUIQuestModel> _activeQuests;
        private List<HubMapUIQuestModel> _completedQuests;

        #endregion


        #region ClassLifeCycle

        public HubMapUIQuestController(HubMapUIContext context)
        {
            _context = context;

            _notActiveQuests = new List<HubMapUIQuestModel>();
            _activeQuests = new List<HubMapUIQuestModel>();
            _completedQuests = new List<HubMapUIQuestModel>();

            for (int i = 0; i < _context.QuestsData.Length; i++)
            {
                _notActiveQuests.Add(new HubMapUIQuestModel(_context.QuestsData[i], HubMapUIQuestStatus.NotActive));
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

        private void OnChangePlayerReputation(HubMapUICityModel city)
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

        private HubMapUIQuestModel GetQuestModel(HubMapUIQuestData questData)
        {
            HubMapUIQuestModel result;
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

        private void QuestActivate(HubMapUIQuestModel quest)
        {
            if (quest.Status == HubMapUIQuestStatus.NotActive)
            {
                    quest.Status = HubMapUIQuestStatus.Active;

                    _activeQuests.Add(quest);
                    _notActiveQuests.Remove(quest);

                    OnQuestActivated(quest);
                    Debug.Log("Change quest status on active");
            }
        }

        private void QuestComplete(HubMapUIQuestModel quest)
        {
            if (quest.Status == HubMapUIQuestStatus.Active)
            {
                quest.Status = HubMapUIQuestStatus.Completed;

                _activeQuests.Remove(quest);
                _completedQuests.Add(quest);

                OnQuestComplete(quest);
                Debug.Log("Change quest status on completed");
            }
        }

        private void QuestProgressing(HubMapUIQuestModel quest)
        {
            DeactivateQuestAnswers(quest);
            SetMarkerTypeToCitizen(quest, HubMapUIQuestMarkerType.None);

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
                SetMarkerTypeToCitizen(quest, HubMapUIQuestMarkerType.Question);
            }
        }

        public bool IsTakeItemFromPlayerTaskConditionComplete(HubMapUIQuestModel quest)
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

        public bool IsGiveItemToPlayerTaskConditionComplete(HubMapUIQuestModel quest)
        {
            if (quest.CurrentTask.GivenItemData != null)
            {
                HubMapUIBaseItemModel itemModel = HubMapUIServices.SharedInstance.
                    ItemInitializeService.InitializeItemModel(quest.CurrentTask.GivenItemData);
                if (_context.Player.Inventory.PutItemToFirstEmptySlot(itemModel))
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

        private bool CheckQuestForAllRequirements(HubMapUIQuestModel quest, RequirementCheckType excludeCheckType = RequirementCheckType.None)
        {
            if (excludeCheckType != RequirementCheckType.CityReputation)
            {
                HubMapUICityData cityData = quest.Data.RequiredReputation.City;
                bool checkReputationRequirement = quest.IsEnoughCityReputation(_context.GetCity(cityData));

                if (!checkReputationRequirement)
                {
                    return false;
                }
            }

            if (excludeCheckType != RequirementCheckType.QuestComplete)
            {
                HubMapUIQuestData questData = quest.Data.RequiredQuest;
                bool checkQuestRequirement = quest.IsRequirementQuestComleted(GetQuestModel(questData));

                if (!checkQuestRequirement)
                {
                    return false;
                }
            }

            return true;
        }

        private void OnQuestActivated(HubMapUIQuestModel quest)
        {
            SetQuestDialogToTargetCitizen(quest);
            ActivateQuestAnswers(quest);
            SetMarkerTypeToCitizen(quest, HubMapUIQuestMarkerType.Exclamation);
        }

        private void OnQuestComplete(HubMapUIQuestModel quest)
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

        private void SetQuestDialogToTargetCitizen(HubMapUIQuestModel quest)
        {
            if (quest.CurrentTask.IsCitizenInitiateDialog)
            {
                _context.GetCitizen(quest.CurrentTask.TargetCitizen).
                    SetCurrentDialogNode(quest.CurrentTask.InitiatedDialogId);
            }
        }

        private void ActivateQuestAnswers(HubMapUIQuestModel quest)
        {
            HubMapUIQuestAnswer questAnswer = _context.GetCitizen(quest.CurrentTask.TargetCitizen).
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
                questAnswer = _context.GetCitizen(quest.CurrentTask.AdditionalCitizensAnswers[i].Citizen).
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

        private void DeactivateQuestAnswers(HubMapUIQuestModel quest)
        {
            HubMapUIQuestAnswer questAnswer = _context.GetCitizen(quest.CurrentTask.TargetCitizen).
                        GetQuestAnswerById(quest.CurrentTask.TargetQuestAnswerId);
            questAnswer.IsActivated = false;
            questAnswer.Answer.OnAnswerSelectByPlayerHandler -= (handlerValue) => QuestProgressing(quest);

            for (int i = 0; i < quest.CurrentTask.AdditionalCitizensAnswers.Length; i++)
            {
                questAnswer = _context.GetCitizen(quest.CurrentTask.AdditionalCitizensAnswers[i].Citizen).
                    GetQuestAnswerById(quest.CurrentTask.AdditionalCitizensAnswers[i].QuestAnswerId);
                questAnswer.IsActivated = false;
            }
        }

        private void SetMarkerTypeToCitizen(HubMapUIQuestModel quest, HubMapUIQuestMarkerType questMarker)
        {
            _context.GetCitizen(quest.CurrentTask.TargetCitizen).QuestMarkerType = questMarker;
        }

        private void SetInteractableQuestAnswer(HubMapUIQuestAnswer questAnswer, HubMapUIQuestModel quest)
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
