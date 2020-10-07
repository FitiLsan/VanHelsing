using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class BossStats
    {
        #region Fields

        [SerializeField] private BossIdleType _idleType;

        #endregion


        #region Properties

        public BossIdleType IdleType => _idleType;

        #endregion
    }
}
