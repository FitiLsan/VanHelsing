namespace BeastHunter
{
    public sealed class QuestTaskDto
    {
        #region Properties

        public QuestTaskTypes Type { get; set; }
        public int Id { get; set; }
        public int TargetId { get; set; }
        public int NeededAmount { get; set; }
        public string Description { get; set; }
        public bool IsOptional { get; set; }

        #endregion
    }
}