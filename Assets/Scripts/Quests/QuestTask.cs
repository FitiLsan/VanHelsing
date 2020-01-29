using Events;
using Events.Args;

namespace Quests
{
    /// <summary>
    ///     Задача квеста
    /// </summary>
    public class QuestTask
    {
        public QuestTask(QuestTaskDto dto)
        {
            Type = dto.Type;
            TargetId = dto.TargetId;
            NeededAmount = dto.NeededAmount;
            Description = dto.Description;
            Id = dto.Id;
            IsOptional = dto.IsOptional;
        }

        public int Id { get; }

        /// <summary>
        ///     Type of this objective
        /// </summary>
        public QuestTaskTypes Type { get; }

        /// <summary>
        ///     Target's id (enemy, item, zone etc)
        /// </summary>
        public int TargetId { get; }

        /// <summary>
        ///     Amount of targets needed for completing this objective
        /// </summary>
        public int NeededAmount { get; }

        /// <summary>
        ///     Current amount of targets
        /// </summary>
        public int CurrentAmount { get; private set; }

        public bool IsCompleted => CurrentAmount >= NeededAmount;

        /// <summary>
        ///     Description of this objective for quest log and tracking ui
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Flag for optional tasks in quest
        /// </summary>
        public bool IsOptional { get; }

        /// <summary>
        ///     Updates objective status
        /// </summary>
        /// <param name="amount"></param>
        public void AddAmount(int amount)
        {
            CurrentAmount += amount;
            EventManager.TriggerEvent(GameEventTypes.QuestTaskUpdated,
                new TaskUpdatedArgs(Id, Description, CurrentAmount, NeededAmount));
        }
    }
}