namespace BeastHunter
{
    public sealed class QuestReward
    {
        #region Properties

        public int ObjectId { get; }
        public int ObjectCount { get; }

        #endregion


        #region ClassLifeCycle

        public QuestReward(QuestRewardDto dto)
        {
            ObjectId = dto.ObjectId;
            ObjectCount = dto.ObjectCount;
        }

        #endregion
    }
}