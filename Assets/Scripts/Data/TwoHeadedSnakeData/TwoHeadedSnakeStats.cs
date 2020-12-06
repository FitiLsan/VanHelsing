using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct TwoHeadedSnakeStats
    {
        #region Properties
        [Header("Scene information fields. Default: 0, 0, 0")]
        [Tooltip("Vector 3 prefab position on the scene, ")]
        public Vector3 InstantiatePosition;

        #endregion


    }
}
