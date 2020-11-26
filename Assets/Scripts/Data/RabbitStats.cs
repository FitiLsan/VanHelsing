using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class RabbitStats
    {
        #region Fields

        [Tooltip("Default: 10")]
        [SerializeField] private float _runningRadius;

        [Tooltip("Default: 0.5")]
        [SerializeField] private float _moveSpeed;

        [Tooltip("Default: 2")]
        [SerializeField] private float _jumpHeight;

        [SerializeField] private bool _canIdle;

        #endregion


        #region Properties

        public float RunningRadius => _runningRadius;
        public float MoveSpeed => _moveSpeed;
        public float JumpHeight => _jumpHeight;
        public bool CanIdle => _canIdle;

        #endregion
    }
}
