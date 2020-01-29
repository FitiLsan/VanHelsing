namespace Quests
{
    /// <summary>
    ///     Класс награды
    /// </summary>
    public class QuestReward
    {
        public QuestReward(QuestRewardDto dto)
        {
            ObjectId = dto.ObjectId;
            ObjectCount = dto.ObjectCount;
        }

        /// <summary>
        ///     ИД предмета награды
        /// </summary>
        public int ObjectId { get; }

        /// <summary>
        ///     Сколько предметов
        /// </summary>
        public int ObjectCount { get; }
    }
}