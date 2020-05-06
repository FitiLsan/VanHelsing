namespace BeastHunter
{
    public sealed class QuestRewardDto
    {
        #region Properties

        public QuestRewardTypes Type { get; set; }
        public QuestRewardObjectTypes ObjectType { get; set; }
        public int ObjectId { get; set; }
        public int ObjectCount { get; set; }

        #endregion
    }
}