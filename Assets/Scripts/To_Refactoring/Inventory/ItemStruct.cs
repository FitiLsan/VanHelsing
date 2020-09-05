using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct ItemStruct
    {
        #region Fields

        public string Name;
        public string Discription;

        public SlotSize SlotSize;
        public ItemType ItemType;

        public Mesh MeshOfObject;
        public Sprite Icon;

        #endregion
    }
}

