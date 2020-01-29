using System.Collections.Generic;

namespace Quests
{
    /// <summary>
    ///     Группа наград
    /// </summary>
    public class QuestRewardGroup
    {
        public QuestRewardGroup(QuestRewardTypes rt, QuestRewardObjectTypes ot)
        {
            Type = rt;
            ObjectType = ot;
            Rewards = new List<QuestReward>();
        }

        /// <summary>
        ///     Тип награды (на выбор, фиксированная)
        /// </summary>
        public QuestRewardTypes Type { get; }

        /// <summary>
        ///     Что даем в награду (предметы, заклинания, баффы)
        /// </summary>
        public QuestRewardObjectTypes ObjectType { get; }

        /// <summary>
        ///     Список наград
        /// </summary>
        public List<QuestReward> Rewards { get; }

        /// <summary>
        ///     Добавляет награду в группу
        /// </summary>
        /// <param name="dto"></param>
        public void AddReward(QuestRewardDto dto)
        {
            if (dto.Type == Type && dto.ObjectType == ObjectType) Rewards.Add(new QuestReward(dto));
        }
    }
}