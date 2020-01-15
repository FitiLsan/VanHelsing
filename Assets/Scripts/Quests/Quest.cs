using System.Collections.Generic;
using System.Linq;

namespace Quests
{
    /// <summary>
    ///     Класс квеста
    /// </summary>
    public class Quest
    {
        public Quest(QuestDto dto)
        {
            Id = dto.Id;
            Title = dto.Title;
            Description = dto.Description;
            ZoneId = dto.ZoneId;
            MapMarkers = new List<QuestMarker>();
            foreach (var mapMarker in dto.MapMarkers) MapMarkers.Add(new QuestMarker(mapMarker));
            //TODO: Убрать заглушки на ноль
            StartMarker = new QuestMarker(dto.QuestStart ?? new QuestMarkerDto{MapId = 0, X = 0f, Y = 0f});
            EndMarker = new QuestMarker(dto.QuestEnd ?? new QuestMarkerDto{MapId = 0, X = 0f, Y = 0f});
            RequiredQuests = dto.RequiredQuests;
            foreach (var task in dto.Tasks) Tasks.Add(new QuestTask(task));
            MinLevel = dto.MinLevel;
            QuestLevel = dto.QuestLevel;
            TimeAllowed = dto.TimeAllowed;
            RemainingTime = TimeAllowed;
            RewardExp = dto.RewardExp;
            RewardMoney = dto.RewardMoney;
            StartDialogId = dto.StartDialogId;
            EndDialogId = dto.EndDialogId;
            foreach (var reward in dto.Rewards)
            {
                var tmp = Rewards.Find(x => x.Type == reward.Type && x.ObjectType == reward.ObjectType);
                if (tmp != null)
                {
                    tmp.AddReward(reward);
                }
                else
                {
                    tmp = new QuestRewardGroup(reward.Type, reward.ObjectType);
                    tmp.AddReward(reward);
                    Rewards.Add(tmp);
                }
            }

            HasOptionalTasks = Tasks.Any(x => x.IsOptional);
        }

        /// <summary>
        ///     Unique id
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Minimum character level for accepting this quest
        /// </summary>
        public int MinLevel { get; }

        /// <summary>
        ///     Experience points for completing this quest
        /// </summary>
        public int RewardExp { get; }

        /// <summary>
        ///     Id of Dialog, used to start this quest (Not sure that needed)
        /// </summary>
        public int StartDialogId { get; }

        /// <summary>
        ///     Id of Dialog, used to complete this quest (Not sure that needed)
        /// </summary>
        public int EndDialogId { get; }

        /// <summary>
        ///     Money for completing this quest (in min-valued coins)
        /// </summary>
        public int RewardMoney { get; }

        /// <summary>
        ///     Level of Quest as Average planned character level for this quest
        /// </summary>
        public int QuestLevel { get; }

        /// <summary>
        ///     List of ids of quests, that must be completed for accepting this quest
        /// </summary>
        public List<int> RequiredQuests { get; }

        /// <summary>
        ///     Title of quest
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///     If quest limited for some specific time - this is time in seconds for completing this quest
        ///     OR
        ///     -1 if quest not limited with time.
        /// </summary>
        public int TimeAllowed { get; }

        /// <summary>
        ///     Is quest about time?
        /// </summary>
        public bool IsTimed => TimeAllowed > 0;

        /// <summary>
        ///     Completed quest or not?
        /// </summary>
        public bool IsComplete
        {
            get
            {
                if (TimeAllowed != -1 && RemainingTime <= 0)
                    return false;

                foreach (var task in Tasks)
                    if (!task.IsCompleted)
                        return false;

                return true;
            }
        }

        /// <summary>
        ///     Failed or not?
        /// </summary>
        public bool IsFailed => TimeAllowed != -1 && RemainingTime <= 0;

        /// <summary>
        ///     Description of quest that appears in questlog
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     List of objectives to complete quest
        /// </summary>
        public List<QuestTask> Tasks { get; } = new List<QuestTask>();

        //TODO: public QuestReward Reward { get; }

        /// <summary>
        ///     Is quest set for quick tracking in hud?
        /// </summary>
        public bool IsTracked { get; set; } = false;

        /// <summary>
        ///     parent zone for this quest
        /// </summary>
        public int ZoneId { get; }

        /// <summary>
        ///     Mapmarkers for quest
        /// </summary>
        public List<QuestMarker> MapMarkers { get; }

        /// <summary>
        ///     Marker for starting point (not sure that needed)
        /// </summary>
        public QuestMarker StartMarker { get; }

        /// <summary>
        ///     Marker for endpoint of quest
        /// </summary>
        public QuestMarker EndMarker { get; }

        /// <summary>
        ///     Remaining time if quest limited with time
        /// </summary>
        public float RemainingTime { get; private set; }

        /// <summary>
        ///     Список наград за квест
        /// </summary>
        public List<QuestRewardGroup> Rewards { get; } = new List<QuestRewardGroup>();

        /// <summary>
        ///     Are there any optional tasks in this quest
        /// </summary>
        public bool HasOptionalTasks { get; }

        /// <summary>
        ///     onUpdate method. Can be used to set time after loading
        /// </summary>
        /// <param name="deltaTime">Time from last frame</param>
        public void ReduceTime(float deltaTime)
        {
            RemainingTime -= deltaTime;
        }
    }
}