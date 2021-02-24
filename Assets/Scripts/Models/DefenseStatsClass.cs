using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public sealed class DefenceStatsClass
    {
        #region Fields

        [Tooltip("Physical damage resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _physicalDamageResistance;

        [Tooltip("Stun resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _stunProbabilityResistance;

        [Tooltip("Fire resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _fireDamageResistance;

        #endregion


        #region Properties

        public float PhysicalDamageResistance
        {
            get
            {
                return _physicalDamageResistance;
            }
            set
            {
                _physicalDamageResistance = Mathf.Clamp(value, 0f, 1f);
            }
        }

        public float StunProbabilityResistance
        {
            get
            {
                return _stunProbabilityResistance;
            }
            set
            {
                _stunProbabilityResistance = Mathf.Clamp(value, 0f, 1f);
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

