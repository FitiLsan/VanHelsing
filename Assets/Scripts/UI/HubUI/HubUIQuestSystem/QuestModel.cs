using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class QuestModel
    {
        #region Properties

        public QuestData Data { get; private set; }
        public QuestStatus Status { get; set; }
        public QuestTaskData CurrentTask { get; private set; }

        #endregion


        #region ClassLifeCycle

        public QuestModel(QuestData data, QuestStatus status)
        {
            Data = data;
            Status = status;
            CurrentTask = data.Tasks.Find(task => task.Id == data.FirstTaskId);
        }

        #endregion


        #region Methods

        public bool IsRequirementQuestComleted(QuestModel requirementQuest)
        {
            if (HasQuestCompleteRequirement(requirementQuest.Data))
            {
                return requirementQuest.Status == QuestStatus.Completed;
            }
            return true;
        }

        public bool IsEnoughCityReputation(CityModel city)
        {
            if (HasCityRequirement(city.DataInstanceID))
            {
                return Data.RequiredReputation.Reputation <= city.PlayerReputation;
            }
            return true;
        }

        public void NextTask()
        {
            QuestTaskData nextTask = Data.Tasks.Find (task => task.Id == CurrentTask.NextQuestTaskId);
            CurrentTask = nextTask;
        }

        public bool IsLastTaskComplete()
        {
            return CurrentTask.Id == Data.EmptyEndTaskId;
        }

        private bool HasQuestCompleteRequirement(QuestData questData)
        {
            if (Data.RequiredQuest != null)
            {
                return Data.RequiredQuest == questData;
            }
            return false;
        }

        private bool HasCityRequirement(int cityDataInstanceID)
        {
            if (Data.RequiredReputation.City != null)
            {
                return Data.RequiredReputation.City.GetInstanceID() == cityDataInstanceID;
            }
            return false;
        }

        #endregion
    }
}
