using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public class BossStats
    {
        #region Fields

        [SerializeField] private BossIdlePattern _idlePattern;

        #endregion


        #region Properties

        public BossIdleType IdlePattern => _idlePattern;

        #endregion
    }
}
