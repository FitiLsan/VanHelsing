using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class BaseStatsClass
    {
        #region Fields

        [Header("Basic stats")]

        [Tooltip("Physical power between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _physicalPower;

        [Tooltip("Physical damage resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _physicalDamageResistance;

        [Tooltip("Stun resistance between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _stunProbabilityResistance;

        #endregion


        #region Properties

        public float PhysicalPower => _physicalPower;
        public float PhysicalResistance => _physicalDamageResistance;
        public float StunResistance => _stunProbabilityResistance;

        #endregion
    }
}
