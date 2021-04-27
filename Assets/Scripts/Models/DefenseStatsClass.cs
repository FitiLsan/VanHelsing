using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public sealed class DefenceStatsClass
    {
        #region Fields
        [Header("Physical Damage resistance")]

        [Tooltip(" between -2 and 2.")]
        [Range(-2.0f, 2.0f)]
        [SerializeField] private float _cuttingDamageResistance;

        [Tooltip(" between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _piercingDamageResistance;

        [Tooltip(" between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _choppingDamageResistance;

        [Tooltip(" between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _crushingDamageResistance;

        [Tooltip(" between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _penetratingDamageResistance;

        [Header("Element Damage Resistance")]

        [Tooltip("resistance between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _fireDamageResistance;

        [Tooltip("resistance between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _waterDamageResistance;

        [Tooltip("resistance between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _iceDamageResistance;

        [Tooltip("resistance between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _electricityDamageResistance;

        [Tooltip("resistance between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _oilDamageResistance;

        [Tooltip("resistance between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _toxinDamageResistance;

        [Tooltip("resistance between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _gasDamageResistance;

        [Tooltip("resistance between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _smokeAndSteamDamageResistance;

        [Header("Effects Probability Resistance")]

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _burningProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _weetingProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freezingProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _electrizationProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _oilingProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _poisoningProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _gassingProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _suffocationProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _bleedingProbabilityResistance;

        [Tooltip("resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _stunningProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _slowingProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _overturningProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _contusionProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _intoxicationProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _blindingProbabilityResistance;

        [Tooltip(" resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _explosionProbabilityResistance;




        #endregion


        #region Properties

        public float PhysicalDamageResistance
        {
            get
            {
                return _cuttingDamageResistance;
            }
            set
            {
                _cuttingDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }

        public float StunProbabilityResistance
        {
            get
            {
                return _stunningProbabilityResistance;
            }
            set
            {
                _stunningProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }

        public float FireDamageResistance
        {
            get
            {
                return _fireDamageResistance;
            }
            set
            {
                _fireDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }

        #endregion
    }
}

