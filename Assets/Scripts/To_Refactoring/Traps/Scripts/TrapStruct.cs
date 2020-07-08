using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct TrapStruct
    {
        public GameObject Prefab;
        public float Damage;
        public float Duration;
        public NotPhysicalTypesOfDamage NotPhysicalTypesOfDamage;
        public PhysicalTypesOfDamage PhysicalTypesOfDamage;
    }
}

