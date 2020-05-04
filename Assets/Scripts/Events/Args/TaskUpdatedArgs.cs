using System;


namespace BeastHunter
{
    public sealed class TaskUpdatedArgs : EventArgs
    {
        #region Properties

        public string Description { get; }
        public int CurrentAmount { get; }
        public int NeededAmount { get; }
        public int Id { get; }

        #endregion


        #region ClassLifeCycle

        public TaskUpdatedArgs(int id, string description, int currentAmount, int neededAmount)
        {
            Description = description;
            CurrentAmount = currentAmount;
            NeededAmount = neededAmount;
            Id = id;
        }

        #endregion
    }
}