using UnityEngine;
using System;


namespace BeastHunter
{
    [Serializable]
    public struct StartDialogueStruct
    {
        #region Fields

        public GameObject Prefab;
        public Transform PlayerTransform;
        public GameObject PrefabCanvasNpc;

        #endregion
    }
}