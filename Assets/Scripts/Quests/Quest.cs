using System.Collections.Generic;
using System.Linq;


namespace BeastHunter
{
    public sealed class Quest
    {
        #region Properties

        public int Id { get; }
        public int MinLevel { get; }
        public int RewardExp { get; }
        public int StartDialogId { get; }
        public int EndDialogId { get; }
        public int IsRepetable { get; }
        public int RewardMoney { get; }
        public int QuestLevel { get; }
        public List<int> RequiredQuests { get; }
        public string Title { get; }
        public int TimeAllowed { get; }
        public bool IsTimed => TimeAllowed > 0;
        public bool IsComplete
        {
            get
            {
                if (TimeAllowed != -1 && RemainingTime <= 0)
                    return false;

                foreach (var task in Tasks)
                {
                    if (!task.IsCompleted)
                    {
                        if (!task.IsOptional)
                        {
                            return false;
                        }
                    }//test
                        
                }

                return true;
            }
        }
        public bool IsFailed => TimeAllowed != -1 && RemainingTime <= 0;
        public string Description { get; }
        public List<QuestTask> Tasks { get; } = new List<QuestTask>();
        //TODO: public QuestReward Reward { get; }
        public bool IsTracked { get; set; } = false;
        public int ZoneId { get; }
        public List<QuestMarker> MapMarkers { get; }
        public QuestMarker StartMarker { get; }
        public QuestMarker EndMarker { get; }
        public float RemainingTime { get; private set; }
        public List<QuestRewardGroup> Rewards { get; } = new List<QuestRewardGroup>();
        public bool HasOptionalTasks { get; }

        #endregion


        #region ClassLifeCycle

        public Quest(QuestDto dto)
        {
            Id = dto.Id;
            Title = dto.Title;
            Description = dto.Description;
            ZoneId = dto.ZoneId;
            MapMarkers = new List<QuestMarker>();
            foreach (var mapMarker in dto.MapMarkers) MapMarkers.Add(new QuestMarker(mapMarker));
            //TODO: Убрать заглушки на ноль
            StartMarker = new QuestMarker(dto.QuestStart ?? new QuestMarkerDto { MapId = 0, X = 0f, Y = 0f });
            EndMarker = new QuestMarker(dto.QuestEnd ?? new QuestMarkerDto { MapId = 0, X = 0f, Y = 0f });
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
            IsRepetable = dto.IsRepetable;
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

        #endregion


        #region Methods

        public void ReduceTime(float deltaTime)
        {
            RemainingTime -= deltaTime;
        }

        #endregion
    }
}