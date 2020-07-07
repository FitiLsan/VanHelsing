using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct NpcStats
    {
        #region Properties

        public float MaxHealth;
        public BaseStatsClass BaseStats;
        public GameObject Prefab;
        
        #endregion
    }
}
