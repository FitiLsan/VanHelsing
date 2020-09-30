using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct BossStats
    {
        #region Fields

        [SerializeField] private BossIdleType _idlePattern;

        #endregion


        #region Properties

        public BossIdleType IdlePattern => _idlePattern;

        #endregion
    }
}
