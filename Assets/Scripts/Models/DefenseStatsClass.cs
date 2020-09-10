using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class DefenceStatsClass
    {
        #region Fields

        [Tooltip("Base defense value")]
        [SerializeField] private float _baseDefense;
        // unify antack elements (WeaknessOrImmunityMaterial, WeaknessToDamage, NotPhysicalTypesOfDamage ...)
        //_weakElement list?
        //_resistElement list?
        //_immunityElement list?
        //debuffResist
        //etc.


        #endregion


        #region Properties

        public float BaseDefense => _baseDefense;

        #endregion
    }
}

