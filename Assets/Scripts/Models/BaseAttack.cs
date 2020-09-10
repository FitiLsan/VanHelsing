using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct BaseAttack
    {
        #region Fields

        [Tooltip("Base attack damage")]
        [SerializeField] private float _attackDamage;
        //[Tooltip("Attacking bodypart")] bite, claw, projectile etc.
        //[SerializeField] private AttackType _attackType;
        [Tooltip("Type of physical damage inflicted")]
        [SerializeField] private PhysicalTypeOfDamage _physicalDamageType;
        
        //attack element/non-physical
        //attack debuff probability
        //attack debuff
        //etc.
        
        #endregion


        #region Properties

        public float AttackDamage => _attackDamage;
        public PhysicalTypeOfDamage PhysicalDamageType => _physicalDamageType;

        #endregion
    }
}
