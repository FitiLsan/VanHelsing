using System;
using UnityEngine;
using UnityEngine.AI;

namespace BeastHunter
{
    [Serializable]
    public sealed class BaseStats
    {
        #region Events

        public event Action SpeedUpdate;

        #endregion


        #region Fields

        [Header("Basic stats")]

        [Tooltip("Basic maximal health points.")]
        [Range(0.0f, 100000.0f)]
        [SerializeField] private float _maximalHealthPoints;

        [Tooltip("Current health points.")]
        [Range(0.0f, 100000.0f)]
        [SerializeField] private float _currentHealthPoints;

        [Tooltip("Health regen per second.")]
        [Range(0.0f, 100000.0f)]
        public float HealthRegenPerSecond;

        [Tooltip("Maximal stamina points.")]
        [Range(0.0f, 100000.0f)]
        [SerializeField] private float _maximalStaminaPoints;

        [Tooltip("Maximal stamina points.")]
        [Range(0.0f, 100000.0f)]
        [SerializeField] private float _currentStaminaPoints;

        [Tooltip("Stamina regen per second.")]
        [Range(0.0f, 100000.0f)]
        public float StaminaRegenPerSecond;

        [Tooltip("Is dead flag.")]
        public bool IsDead;

        [Tooltip("Is stunned flag.")]
        public bool IsStunned;

        private float _speedModifier;
        #endregion


        #region Properties

        public float MaximalHealthPoints
        {
            get
            {
                return _maximalHealthPoints;
            }
            set
            {
                _maximalHealthPoints = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }

        public float CurrentHealthPoints
        {
            get
            {
                return _currentHealthPoints;
            }
            set
            {
                _currentHealthPoints = Mathf.Clamp(value, 0f, MaximalHealthPoints);
            }
        }

        public float MaximalStaminaPoints
        {
            get
            {
                return _maximalStaminaPoints;
            }
            set
            {
                _maximalStaminaPoints = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }

        public float CurrentStaminaPoints
        {
            get
            {
                return _currentStaminaPoints;
            }
            set
            {
                _currentStaminaPoints = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }

        public float CurrentHealthPart
        {
            get
            {
                return CurrentHealthPoints / MaximalHealthPoints;
            }
        }

        public float CurrentStaminaPart
        {
            get
            {
                return CurrentStaminaPoints / MaximalStaminaPoints;
            }
        }

        public float SpeedModifier
        {
            get
            {
                return _speedModifier;
            }
            set
            {
                _speedModifier = value;
                SpeedUpdate?.Invoke();
            }
        }

        #endregion
    }
}
