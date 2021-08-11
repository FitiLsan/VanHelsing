using UnityEngine;


namespace BeastHunter
{
    [System.Serializable]
    public struct ButterflyStats
    {
        #region Properties

        [Tooltip("Default: 10")]
        public float RunningRadius;

        [Tooltip("Default: 0.5")]
        public float MoveSpeed;

        [Tooltip("Default: 2")]
        public float JumpHeight;

        [Tooltip("Default: 3")]
        public float ViewRadius;

        [Tooltip("Default: 100")]
        [Range(0.0f, 180.0f)]
        public float ViewAngle;

        public bool CanIdle;

        #endregion
    }
}