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

        [SerializeField] private float _viewRadius;
        [Range(0.0f, 180.0f)]
        [SerializeField] private float _viewAngle;

        //OBSOLETE
        [Header("Basic stats (obsolete)")]

        [Tooltip("Health points")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _healthPoints;

        [Tooltip("Physical power between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _physicalPower;

        [Tooltip("Magical power between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _magicalPower;

        [Tooltip("Physical damage resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _physicalDamageResistance;

        [Tooltip("Stun resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _stunProbabilityResistance;

        [Tooltip("Fire resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _fireDamageResistance;
        //OBSOLETE

        #endregion


        #region Properties

        public float HealthPoints => _healthPoints;
        public float MaxHealth => _maxHealth;
        public EnemyType EnemyType => _enemyType;
        public float ViewRadius => _viewRadius;
        public float ViewAngle => _viewAngle;

        //OBSOLETE
        public float PhysicalPower => _physicalPower;
        public float MagicalPower => _magicalPower;
        public float PhysicalResistance => _physicalDamageResistance;
        public float StunResistance => _stunProbabilityResistance;
        public float FireResistance => _fireDamageResistance;
        //OBSOLETE

        #endregion
    }
}
