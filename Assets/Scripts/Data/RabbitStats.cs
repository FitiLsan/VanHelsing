using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct RabbitStats
    {
        #region Properties

        [Tooltip("Default: 10")]
        public float RunningRadius;

        [Tooltip("Default: 0.5")]
        public float MoveSpeed;

        [Tooltip("Default: 2")]
        public float JumpHeight;

        public bool CanIdle;

        #endregion
    }
}
