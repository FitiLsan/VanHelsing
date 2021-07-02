using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class PlayerData
    {
        #region Fields

        [SerializeField] private int _rank;
        [SerializeField] private int _goldAmount;
        [SerializeField] private int _inventorySlotsAmount;
        [SerializeField] private BaseItemSO[] _inventoryItems;
        [SerializeField] private CharacterSO[] _availableCharacters;

        #endregion


        #region Properties

        public int Rank => _rank;
        public int GoldAmount => _goldAmount;
        public int InventorySlotsAmount => _inventorySlotsAmount;
        public BaseItemSO[] InventoryItems => (BaseItemSO[])_inventoryItems?.Clone();
        public CharacterSO[] AvailableCharacters => (CharacterSO[])_availableCharacters?.Clone();

        #endregion
    }
}
