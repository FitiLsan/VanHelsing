using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public struct CharactersSettingsStruct
    {
        #region Fields

        [SerializeField] private ClothesType[] _clothesSlots;
        [SerializeField] private int _backpuckSlotAmount;
        [SerializeField] private int _weaponSetsAmount;

        #endregion


        #region Properties

        public int BackpuckSlotAmount => _backpuckSlotAmount;
        public int WeaponSetsAmount => _weaponSetsAmount;
        public ClothesType[] ClothesSlots => (ClothesType[])_clothesSlots.Clone();

        #endregion
    }
}
