using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class BaseStatsClass
    {
        #region Fields

        [Tooltip("Maximum Hp value")]
        [SerializeField] private float _maxHealth;
        [SerializeField] private EnemyType _enemyType;
        //_enemySubtype

        //_moveSpeed
        //_jumpHeight
        //etc.

        //OBSOLETE
        [Header("Basic stats")]

        [Tooltip("Health points")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _healthPoints;

        [Tooltip("Physical power between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _physicalPower;

        [Tooltip("Physical damage resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _physicalDamageResistance;

        [Tooltip("Stun resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _stunProbabilityResistance;
        //OBSOLETE

        #endregion


        #region Properties

        public float HealthPoints => _healthPoints;
        public float MaxHealth => _maxHealth;
        public EnemyType EnemyType => _enemyType;

        //OBSOLETE
        public float PhysicalPower => _physicalPower;
        public float PhysicalResistance => _physicalDamageResistance;
        public float StunResistance => _stunProbabilityResistance;
        //OBSOLETE

        #endregion
    }
}
