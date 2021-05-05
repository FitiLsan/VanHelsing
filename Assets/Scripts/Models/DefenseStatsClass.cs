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

        [Tooltip(" between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _explosionDamageResistance;

        [Tooltip(" between -2 and 2.")]
        [Range(-2f, 2.0f)]
        [SerializeField] private float _bleedingDamageResistance;

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

        public float CuttingDamageResistance
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

        public float PiercingDamageResistance
        {
            get
            {
                return _piercingDamageResistance;
            }
            set
            {
                _piercingDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float ChoppingDamageResistance
        {
            get
            {
                return _choppingDamageResistance;
            }
            set
            {
                _choppingDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float CrushingDamageResistance
        {
            get
            {
                return _crushingDamageResistance;
            }
            set
            {
                _crushingDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float PenetrationDamageResistance
        {
            get
            {
                return _penetratingDamageResistance;
            }
            set
            {
                _penetratingDamageResistance = Mathf.Clamp(value, 0f, 1f);
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
        public float WaterDamageResistance
        {
            get
            {
                return _waterDamageResistance;
            }
            set
            {
                _waterDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float IceDamageResistance
        {
            get
            {
                return _iceDamageResistance;
            }
            set
            {
                _iceDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float ElectricityDamageResistance
        {
            get
            {
                return _electricityDamageResistance;
            }
            set
            {
               _electricityDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float OilDamageResistance
        {
            get
            {
                return _oilDamageResistance;
            }
            set
            {
                _oilDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float ToxinDamageResistance
        {
            get
            {
                return _toxinDamageResistance;
            }
            set
            {
                _toxinDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float GasDamageResistance
        {
            get
            {
                return _gasDamageResistance;
            }
            set
            {
                _gasDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float SmokeAndSteamDamageResistance
        {
            get
            {
                return _smokeAndSteamDamageResistance;
            }
            set
            {
                _smokeAndSteamDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float BleedingDamageResistance
        {
            get
            {
                return  _bleedingDamageResistance;
            }
            set
            {
                _bleedingDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }

        public float BurningProbabilityResistance
        {
            get
            {
                return _burningProbabilityResistance;
            }
            set
            {
               _burningProbabilityResistance  = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float WeetingProbabilityResistance
        {
            get
            {
                return _weetingProbabilityResistance;
            }
            set
            {
                _weetingProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float FreezingProbabilityResistance
        {
            get
            {
                return _freezingProbabilityResistance;
            }
            set
            {
               _freezingProbabilityResistance  = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float ElectrizationProbabilityResistance
        {
            get
            {
                return _electrizationProbabilityResistance;
            }
            set
            {
                _electrizationProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float OilingProbabilityResistance
        {
            get
            {
                return _oilingProbabilityResistance;
            }
            set
            {
               _oilingProbabilityResistance  = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float PoisoningProbabilityResistance
        {
            get
            {
                return _poisoningProbabilityResistance;
            }
            set
            {
               _poisoningProbabilityResistance  = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float GassingProbabilityResistance
        {
            get
            {
                return _gassingProbabilityResistance;
            }
            set
            {
                _gassingProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float SuffocationProbabilityResistance
        {
            get
            {
                return _suffocationProbabilityResistance;
            }
            set
            {
               _suffocationProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float BleedingProbabilityResistance
        {
            get
            {
                return _bleedingProbabilityResistance;
            }
            set
            {
                _bleedingProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float StunningProbabilityResistance
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
        public float SlowingProbabilityResistance
        {
            get
            {
                return _slowingProbabilityResistance;
            }
            set
            {
               _slowingProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float OverturningProbabilityResistance
        {
            get
            {
                return _overturningProbabilityResistance;
            }
            set
            {
               _overturningProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float ContusionProbabilityResistance
        {
            get
            {
                return _contusionProbabilityResistance;
            }
            set
            {
                _contusionProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float IntoxicationProbabilityResistance
        {
            get
            {
                return _intoxicationProbabilityResistance;
            }
            set
            {
                _intoxicationProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float BlindingProbabilityResistance
        {
            get
            {
                return _blindingProbabilityResistance;
            }
            set
            {
                _blindingProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }
        public float ExplosionProbabilityResistance
        {
            get
            {
                return _explosionProbabilityResistance;
            }
            set
            {
                 _explosionProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }



        #endregion
    }
}

