using System;
using System.Collections.Generic;

namespace BeastHunter
{
    [Serializable]
    public sealed class Stats
    {
        #region Fields

        public BaseStats BaseStats;
        public AttackStats AttackStats;
        public DefenceStatsClass DefenceStats;
        public int InstanceID;
        [NonSerialized]
        public BuffHolder BuffHolder;
        public ItemReactions ItemReactions;



        #endregion
        public Stats()
        {
            BaseStats = new BaseStats();
            AttackStats = new AttackStats();
            DefenceStats = new DefenceStatsClass();
            ItemReactions = new ItemReactions();

        }
    }
}
