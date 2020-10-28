using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct ButterflyStruct
    {
        #region Properties

        [Tooltip("Default: 10")]
        public float MoveSpeed;

        public Vector3 Edge;

        public GameObject Prefab;

        #endregion
    }
}
