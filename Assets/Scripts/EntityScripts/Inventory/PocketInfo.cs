using System;


namespace BeastHunter
{
    [Serializable]
    public struct PocketInfo
    {
        #region Fields

        public SlotSize SlotEnum;
        public ItemType ItemTypeInPocket;
        public BaseItem ItemInPocket;

        #endregion
    }
}
