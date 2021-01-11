using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public sealed class Damage
    {
        #region Fields

        [Header("Basic information")]

        [Tooltip("Physical Damage between 0 and 50.")]
        [Range(0.0f, 50.0f)]
        public float PhysicalDamage;

        [Tooltip("Stun probability between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        public float StunProbability;

        [Tooltip("Fire Damage")]
        public float FireDamage;

        #endregion
    }
}

