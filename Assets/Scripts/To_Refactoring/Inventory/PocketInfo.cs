using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct PocketInfo
    {
        #region Fields

        public GameObject Prefab;
        public HumanBodyBones Attachment;
        public SlotSize SlotEnum;
        public ItemType ItemTypeInPocket;
        public BaseItem ItemInPocket;
        public Vector3 LocalPosition;
        public Vector3 LocalRotation;

        #endregion
    }
}
