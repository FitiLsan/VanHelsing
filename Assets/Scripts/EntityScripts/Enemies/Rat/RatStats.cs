using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct RatStats
    {
        #region Properties

        [Tooltip("Default: 5")]
        public float RunningRadius;

        [Tooltip("Default: 1")]
        public float MoveSpeed;

        [Tooltip("Default: 1")]
        public float JumpHeight;

        [Tooltip("Default: 3")]
        public float ViewRadius;

        [Tooltip("Default: 140")]
        [Range(0.0f, 180.0f)]
        public float ViewAngle;

        public bool CanIdle;

        #endregion
    }
}
