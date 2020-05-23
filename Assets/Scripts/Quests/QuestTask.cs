namespace BeastHunter
{
    public sealed class QuestTask
    {
        #region Properties

        public int Id { get; }
        public QuestTaskTypes Type { get; }
        public int TargetId { get; }
        public int NeededAmount { get; }
        public int CurrentAmount { get; private set; }
        public bool IsCompleted => CurrentAmount >= NeededAmount;
        public string Description { get; }
        public bool IsOptional { get; }

        #endregion


        #region ClassLifeCycle

        public QuestTask(QuestTaskDto dto)
        {
            Type = dto.Type;
            TargetId = dto.TargetId;
            NeededAmount = dto.NeededAmount;
            Description = dto.Description;
            Id = dto.Id;
            IsOptional = dto.IsOptional;
        }

        #endregion


        #region Methods

        public void AddAmount(int amount)
        {
            CurrentAmount += amount;
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestTaskUpdated, new TaskUpdatedArgs(Id, Description, CurrentAmount, NeededAmount));
        }

        #endregion
    }
}