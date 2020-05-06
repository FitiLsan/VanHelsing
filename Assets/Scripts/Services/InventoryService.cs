using UnityEngine;
using System.Collections.Generic;

namespace BeastHunter
{
    public sealed class InventoryService : Service
    {
        #region Fields

        public ClothItem Head;
        public ClothItem Torso;
        public ClothItem Arms;
        public ClothItem Legs;

        #endregion


        #region ClassLifeCycles

        public InventoryService(Contexts contexts) : base(contexts)
        {
            // TODO make here a standartInventory
        }

        #endregion


        #region Methods

        public WeaponItem[] GetWeapons()
        {
            List<WeaponItem> weaponItems = new List<WeaponItem>();

            var weapons = GetWeaponFromColth(Head);
            if (weapons != null)
                weaponItems.AddRange(weapons);

            weapons = GetWeaponFromColth(Torso);
            if (weapons != null)
                weaponItems.AddRange(weapons);

            weapons = GetWeaponFromColth(Arms);
            if (weapons != null)
                weaponItems.AddRange(weapons);

            weapons = GetWeaponFromColth(Legs);
            if (weapons != null)
                weaponItems.AddRange(weapons);

            return weaponItems.ToArray();
        }

        private WeaponItem[] GetWeaponFromColth(ClothItem cloth)
        {
            List<WeaponItem> weaponItems = new List<WeaponItem>();

            for (int i = 0; i < cloth.pocketInfos.Length; i++)
            {
                if (cloth.pocketInfos[i].ItemInPocket.ItemStruct.ItemType == ItemType.Weapon)
                    weaponItems.Add(cloth.pocketInfos[i].ItemInPocket as WeaponItem);
            }

            return weaponItems.ToArray();
        }

        //TODO make a void methods "SetClothHead" with dictionary etc. and "SetItemInRandomPocket"

        #endregion
    }
}
