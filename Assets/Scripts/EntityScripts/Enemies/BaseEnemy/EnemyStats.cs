using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct EnemyStats
    {
        #region Properties

        public float MaxHealth;
        public BaseStatsClass BaseStats;
        public GameObject Prefab;
        
        #endregion
    }
}
