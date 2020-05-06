using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class QuestRewardGroup
    {
        #region Properties

        public QuestRewardTypes Type { get; }

        public QuestRewardObjectTypes ObjectType { get; }

        public List<QuestReward> Rewards { get; }

        #endregion


        #region ClassLifeCycle

        public QuestRewardGroup(QuestRewardTypes rt, QuestRewardObjectTypes ot)
        {
            Type = rt;
            ObjectType = ot;
            Rewards = new List<QuestReward>();
        }

        #endregion


        #region Methods

        public void AddReward(QuestRewardDto dto)
        {
            if (dto.Type == Type && dto.ObjectType == ObjectType) Rewards.Add(new QuestReward(dto));
        }

        #endregion
    }
}