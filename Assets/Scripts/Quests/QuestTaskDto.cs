namespace Quests
{
    /// <summary>
    ///     Этот класс используется ТОЛЬКО для передачи данных
    /// </summary>
    public class QuestTaskDto
    {
        public QuestTaskTypes Type { get; set; }

        public int Id { get; set; }

        public int TargetId { get; set; }

        public int NeededAmount { get; set; }

        public string Description { get; set; }

        public bool IsOptional { get; set; }
    }
}