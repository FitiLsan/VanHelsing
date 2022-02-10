using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public sealed class Damage
    {
        #region Fields

        [Header("Basic information")]

        public PhysicalDamageType PhysicalDamageType;
        [Tooltip("Damage between 0 and 1000.")]
        [Range(0.0f, 1000.0f)]
        public float PhysicalDamageValue;

        public ElementDamageType ElementDamageType;
        [Tooltip("Damage between 0 and 1000.")]
        [Range(0.0f, 1000.0f)]
        public float ElementDamageValue;

        public bool IsEffectDamage { get; set; }
        #endregion


        #region Methods

        public float GetTotalDamage()
        {
            return PhysicalDamageValue + ElementDamageValue;
        }

        #endregion
    }
}

