using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct TrapStruct
    {
        #region Fields

        public GameObject Prefab;
        public Damage Damage;
        public float Duration;
        public float HeightPlacing;
        public NotPhysicalTypesOfDamage NotPhysicalTypesOfDamage;
        public PhysicalTypesOfDamage PhysicalTypesOfDamage;
        
        #endregion
    }
}

