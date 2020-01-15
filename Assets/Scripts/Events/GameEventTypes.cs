namespace Events
{
    /// <summary>
    ///     Типы событий внутри игры
    /// </summary>
    public enum GameEventTypes
    {
        None = 0, //not for use
        /// <summary>
        /// EnemyDieArgs with npc data
        /// </summary>
        NpcDie,
        /// <summary>
        /// IdArgs with area id
        /// </summary>
        AreaEnter,
        /// <summary>
        /// TaskUpdatedArgs with Task data
        /// </summary>
        QuestTaskUpdated,
        /// <summary>
        /// IdArgs with quest Id
        /// </summary>
        QuestAccepted,
        /// <summary>
        /// IdArgs with quest Id
        /// </summary>
        QuestDeclined,
        /// <summary>
        /// IdArgs with quest Id
        /// </summary>
        QuestAbandoned,
        /// <summary>
        /// IdArgs with quest Id
        /// </summary>
        QuestCompleted, //when all objectives completed
        /// <summary>
        /// IdArgs with quest Id
        /// </summary>
        QuestReported, // when returned to npc and received reward
        Saving,
        GameExit,
        /// <summary>
        /// ItemArgs
        /// </summary>
        ItemDrop,
        /// <summary>
        /// null
        /// </summary>
        EquipmentChanged,
        /// <summary>
        /// ItemArgs
        /// </summary>
        ItemAcquired,
        /// <summary>
        /// IdArgs with dialog id
        /// </summary>
        DialogStarted,
        /// <summary>
        /// IdArgs with dialog id
        /// </summary>
        DialogFinished,
        /// <summary>
        /// ItemArgs
        /// </summary>
        ItemUsed,
        /// <summary>
        /// IdArgs with object Id
        /// </summary>
        ObjectUsed,
        /// <summary>
        /// NpcAreaEnterArgs with Npc Id and area id
        /// </summary>
        NpcAreaEnter,
        /// <summary>
        /// NpcAreaEnterArgs with enemy npc id area id
        /// </summary>
        EnemyAreaEnter,
        /// <summary>
        /// When DayTime changes (ex.: morning->day)
        /// DayTimeArgs with new DayTime
        /// </summary>
        DayTimeChanged,
        /// <summary>
        /// DayTimeArgs with Current DayTime
        /// </summary>
        MidOfDayTimeComes,
        /// <summary>
        /// DialigArgs with nps id and dialog id
        /// </summary>
        DialogAnswerSelect,
        /// <summary>
        /// Когда перерасчет характеристик окончен
        /// </summary>
        StatsRecalculated
    }
}