using System;

namespace Events.Args
{
    /// <summary>
    ///     Параметры события об обновлении части задания
    /// </summary>
    public class TaskUpdatedArgs : EventArgs
    {
        public TaskUpdatedArgs(int id, string description, int currentAmount, int neededAmount)
        {
            Description = description;
            CurrentAmount = currentAmount;
            NeededAmount = neededAmount;
            Id = id;
        }

        /// <summary>
        ///     Описание задачи
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Текущий прогресс
        /// </summary>
        public int CurrentAmount { get; }

        /// <summary>
        ///     Сколько надо
        /// </summary>
        public int NeededAmount { get; }
        /// <summary>
        /// Id задачи
        /// </summary>
        public int Id { get; }
    }
}