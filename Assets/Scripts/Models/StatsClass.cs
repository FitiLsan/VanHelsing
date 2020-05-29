using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public sealed class StatsClass
    {
        #region Fields

        [Header("Basic stats")]

        [Tooltip("Maximal health points.")]
        [SerializeField] public float _maximalHealthPoints;

        [Tooltip("Current health points.")]
        [SerializeField] private float _currentHealthPoints;

        [Tooltip("Health regen per second.")]
        [SerializeField] public float _healthRegenPerSecond;

        [Tooltip("Maximal stamina points.")]
        [SerializeField] private float _maximalStaminaPoints;

        [Tooltip("Maximal stamina points.")]
        [SerializeField] private float _currentStaminaPoints;

        [Tooltip("Stamina regen per second.")]
        [SerializeField] private float _staminaRegenPerSecond;

        [Tooltip("Is dead flag.")]
        [SerializeField] private bool _isDead;

        [Tooltip("Is stunned flag.")]
        [SerializeField] private bool _isStunned;

        #endregion


        #region Properties

        public float MaximalHealthPoints => _maximalHealthPoints;
        public float CurrentHealthPoints => _currentHealthPoints;
        public float HealthRegenPerSecond => _healthRegenPerSecond;
        public float MaximalStaminaPoints => _maximalStaminaPoints;
        public float CurrentStaminaPoints => _currentStaminaPoints;
        public float StaminaRegenPerSecond => _staminaRegenPerSecond;

        public bool IsDead => _isDead;
        public bool IsStunned => _isStunned;

        #endregion
    }
}

