using System;

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



        #endregion
        public Stats()
        {
            BaseStats = new BaseStats();
            AttackStats = new AttackStats();
            DefenceStats = new DefenceStatsClass();
        }
    }
}
