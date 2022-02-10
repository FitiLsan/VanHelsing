using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class RabbitStats
    {
        #region Fields

        [SerializeField] private float _viewRadius;

        [Range(0.0f, 180.0f)]
        [SerializeField] private float _viewAngle;

        [Tooltip("Default: 10")]
        [SerializeField] private float _runningRadius;

        [Tooltip("Default: 0.5")]
        [SerializeField] private float _moveSpeed;

        [Tooltip("Default: 2")]
        [SerializeField] private float _jumpHeight;

        [SerializeField] private bool _canIdle;

        #endregion


        #region Properties

        public float ViewRadius => _viewRadius;
        public float ViewAngle => _viewAngle;
        public float RunningRadius => _runningRadius;
        public float MoveSpeed => _moveSpeed;
        public float JumpHeight => _jumpHeight;
        public bool CanIdle => _canIdle;

        #endregion
    }
}
