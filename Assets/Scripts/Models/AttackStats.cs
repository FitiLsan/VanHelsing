using UnityEngine;
using System;


namespace BeastHunter
{
    [Serializable]
    public sealed class AttackStats
    {
        #region Fields

        [Tooltip("Physical power.")]
        [Range(0.0f, 100000.0f)]
        [SerializeField] private float _physicalPower;

        [Tooltip("Magical power.")]
        [Range(0.0f, 100000.0f)]
        [SerializeField] private float _magicalPower;

        #endregion


        #region Properties

        public float PhysicalPower
        {
            get
            {
                return _physicalPower;
            }
            set
            {
                _physicalPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }

        public float MagicalPower
        {
            get
            {
                return _magicalPower;
            }
            set
            {
                _magicalPower = Mathf.Clamp(value, 0f, float.PositiveInfinity);
            }
        }

        #endregion
    }
}

