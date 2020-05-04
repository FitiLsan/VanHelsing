namespace BeastHunter
{
    public enum GameEventTypes
    {
        None = 0,
        NpcDie,
        AreaEnter,
        QuestTaskUpdated,
        QuestAccepted,
        QuestDeclined,
        QuestAbandoned,
        QuestCompleted,
        QuestReported,
        Saving,
        GameExit,
        ItemDrop,
        EquipmentChanged,
        ItemAcquired,
        DialogStarted,
        DialogFinished,
        ItemUsed,
        ObjectUsed,
        NpcAreaEnter,
        EnemyAreaEnter,
        DayTimeChanged,
        MidOfDayTimeComes,
        DialogAnswerSelect,
        StatsRecalculated
    }
}