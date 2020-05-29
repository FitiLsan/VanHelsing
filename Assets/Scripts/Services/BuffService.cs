using UnityEngine;


namespace BeastHunter
{
    public sealed class BuffService : Service
    {
        #region Fields

        private StatsClass firstStats;

        #endregion


        #region ClassLifeCycles

        public BuffService(Contexts contexts) : base(contexts)
        {
            firstStats = new StatsClass();
            firstStats._healthRegenPerSecond = 0.5f;
            firstStats._maximalHealthPoints = 100f;

            firstStats = HealthRegen(HealthMax(firstStats, 10), 12);
        }

        #endregion


        #region Methods

        public StatsClass HealthRegen(StatsClass stats, float healtRegenChange)
        {
            stats._healthRegenPerSecond += healtRegenChange;
            return stats;
        }

        public StatsClass HealthMax(StatsClass stats, float healthMax)
        {
            stats._maximalHealthPoints += healthMax;
            return stats;
        }

        #endregion
    }
}

