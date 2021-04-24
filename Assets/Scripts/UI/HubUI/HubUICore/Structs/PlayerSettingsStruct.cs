using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class PlayerSettingsStruct
    {
        #region Fields

        [SerializeField] private int _goldAmount;
        [SerializeField] private int _inventorySlotsAmount;
        [SerializeField] private BaseItemData[] _startInventoryItems;
        [SerializeField] private CharacterData[] _startHiredCharacters;

        #endregion


        #region Properties

        public int GoldAmount => _goldAmount;
        public int InventorySlotsAmount => _inventorySlotsAmount;
        public BaseItemData[] StartInventoryItems => (BaseItemData[])_startInventoryItems.Clone();
        public CharacterData[] StartHiredCharacters => _startHiredCharacters;

        #endregion
    }
}
