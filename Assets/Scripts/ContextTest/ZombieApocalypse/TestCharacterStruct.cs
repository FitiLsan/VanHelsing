using System;
using UnityEngine;


namespace BeastHunter
{
    
    [Serializable]
    public struct TestCharacterStruct
    {
        #region Fields

        public Vector3 StartPosition;
        
        public float MoveSpeed;

        public float TargetDistance;

        public Material Material;

        public GameObject Prefab;

        #endregion
    }
}