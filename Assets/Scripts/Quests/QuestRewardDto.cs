namespace Quests
{
    /// <summary>
    ///     Этот класс используется ТОЛЬКО для передачи данных
    /// </summary>
    public class QuestRewardDto
    {
        public QuestRewardTypes Type { get; set; }
        public QuestRewardObjectTypes ObjectType { get; set; }
        public int ObjectId { get; set; }
        public int ObjectCount { get; set; }
    }
}