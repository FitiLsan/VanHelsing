using UnityEngine;
using System;


namespace BeastHunter
{
    [Serializable]
    public sealed class AttackStats
    {
        #region Fields
        [Header("Physical Damage Power")]

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _cuttingPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _piercingPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _choppingPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _crushingPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _penetrationPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _explosionPower;
        

        [Header("Element Damage Power")]

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _firePower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _waterPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _icePower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _electricityPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _oilPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _toxinPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _gasPower;

        [Tooltip("Damage power.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _smokeAndSteamPower;


        #endregion


        #region Properties

        public float CuttingPower
        {
            get
            {
                return _cuttingPower;
            }
            set
            {
                _cuttingPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }

        public float PiersingPower
        {
            get
            {
                return _piercingPower ;
            }
            set
            {
                _piercingPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }

        public float ChoppingPower
        {
            get
            {
                return _choppingPower;
            }
            set
            {
                _choppingPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float CrushingPower
        {
            get
            {
                return _crushingPower;
            }
            set
            {
                _crushingPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float PenetrationPower
        {
            get
            {
                return _penetrationPower;
            }
            set
            {
                _penetrationPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float FirePower
        {
            get
            {
                return _firePower;
            }
            set
            {
                _firePower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float WaterPower
        {
            get
            {
                return _waterPower;
            }
            set
            {
               _waterPower  = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float IcePower
        {
            get
            {
                return _icePower;
            }
            set
            {
               _icePower  = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float ElectricityPower
        {
            get
            {
                return _electricityPower;
            }
            set
            {
               _electricityPower  = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float OilPower
        {
            get
            {
                return _oilPower;
            }
            set
            {
                _oilPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float ToxinPower
        {
            get
            {
                return _toxinPower;
            }
            set
            {
                _toxinPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float GasPower
        {
            get
            {
                return _gasPower;
            }
            set
            {
                _gasPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        public float SmokeAndSteamPower
        {
            get
            {
                return _smokeAndSteamPower;
            }
            set
            {
                _smokeAndSteamPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }

        public float ExplosionPower
        {
            get
            {
                return _explosionPower;
            }
            set
            {
               _explosionPower  = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }
        #endregion
    }
}

