using UnityEngine;
using System;


namespace BeastHunter
{
    [Serializable]
    public struct ButterflyStruct
    {
        #region Feelds

        public float MaxMoveSpeed;

        public float MoveSpeed;

        public float Height;

        public float FlyingDistance;

        public GameObject Prefab;

        public Transform Target;

        public bool IsScared;

        #endregion
    }
}
