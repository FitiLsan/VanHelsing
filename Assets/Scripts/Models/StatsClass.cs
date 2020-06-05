using UnityEngine;
using System;
using System.Collections.Generic;


namespace BeastHunter
{
    [Serializable]
    public class StatsClass
    {
        #region Fields

        public List<PermanentBuffClass> PermantnsBuffList = new List<PermanentBuffClass>();
        public List<TemporaryBuffClass> TemporaryBuffList = new List<TemporaryBuffClass>();

        [Header("Basic stats")]

        [Tooltip("Maximal health points.")]
        [SerializeField] public float _maximalHealthPoints;

        [Tooltip("Current health points.")]
        [SerializeField] public float _currentHealthPoints;

        [Tooltip("Health regen per second.")]
        [SerializeField] public float _healthRegenPerSecond;

        [Tooltip("Maximal stamina points.")]
        [SerializeField] public float _maximalStaminaPoints;

        [Tooltip("Maximal stamina points.")]
        [SerializeField] public float _currentStaminaPoints;

        [Tooltip("Stamina regen per second.")]
        [SerializeField] public float _staminaRegenPerSecond;

        [Tooltip("Is dead flag.")]
        [SerializeField] public bool _isDead;

        [Tooltip("Is stunned flag.")]
        [SerializeField] public bool _isStunned;

        #endregion


        #region Properties

        //public float MaximalHealthPoints => _maximalHealthPoints;
        //public float CurrentHealthPoints => _currentHealthPoints;
        //public float HealthRegenPerSecond => _healthRegenPerSecond;
        //public float MaximalStaminaPoints => _maximalStaminaPoints;
        //public float CurrentStaminaPoints => _currentStaminaPoints;
        //public float StaminaRegenPerSecond => _staminaRegenPerSecond;

        //public bool IsDead => _isDead;
        //public bool IsStunned => _isStunned;

        #endregion
    }
}

