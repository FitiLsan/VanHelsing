using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class Damage
    {
        #region Fields

        [Header("Basic information")]

        [Tooltip("Physical Damage between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        public float PhysicalDamage;

        [Tooltip("Stun probability between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        public float StunProbability;





        #endregion
    }
}

