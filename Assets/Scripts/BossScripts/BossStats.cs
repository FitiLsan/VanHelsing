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

        public BossIdlePattern IdlePattern => _idlePattern;

        #endregion
    }
}
