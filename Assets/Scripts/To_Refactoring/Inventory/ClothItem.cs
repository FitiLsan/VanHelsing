using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewCloth", menuName = "CreateItem/CreateCloth", order = 0)]
    public class ClothItem : BaseItem
    {
        #region Fields

        public BodyParts BodyParts = BodyParts.None;
        public Mesh[] MeshView;
        public Mesh[] MeshAccessories;
        public PocketInfo[] PocketInfos;

        #endregion


        #region Properties

        public bool HasEmptyPocket
        {
            get
            {
                bool foundEmptyPocket = false;

                foreach (var pocket in PocketInfos)
                {
                    if (pocket.ItemInPocket == null) foundEmptyPocket = true;
                }

                return foundEmptyPocket;
            }
        }

        public bool HasItems
        {
            get
            {
                bool foundItem = false;

                foreach (var pocket in PocketInfos)
                {
                    if (pocket.ItemInPocket != null) foundItem = true;
                }

                return foundItem;
            }
        }

        public bool HasWeapons
        {
            get
            {
                return WeaponQuantity > 0;
            }
        }

        public int WeaponQuantity
        {
            get
            {
                int weaponQuantity = 0;

                foreach (var pocket in PocketInfos)
                {
                    if (pocket.ItemInPocket?.ItemStruct.ItemType == ItemType.Weapon)
                    {
                        weaponQuantity++;
                    }
                }

                return weaponQuantity;
            }
        }

        #endregion


        #region Methods

        public bool SetItemInEmptyPocket(BaseItem item)
        {
            bool isItemSet = false;

            if (HasEmptyPocket)
            {
                for (int pocket = 0; pocket < PocketInfos.Length - 1; pocket++)
                {
                    if (PocketInfos[pocket].ItemTypeInPocket == item.ItemStruct.ItemType &&
                        PocketInfos[pocket].SlotEnum == item.ItemStruct.SlotSize &&
                            PocketInfos[pocket].ItemInPocket == null)
                    {
                        PocketInfos[pocket].ItemInPocket = item;
                        isItemSet = true;
                    }
                }
            }

            return isItemSet;
        }

        public WeaponItem[] GetWeapons()
        {
            if (HasWeapons)
            {
                    WeaponItem[] weaponItems = new WeaponItem[WeaponQuantity];
                    int enumerator = 0;

                    foreach (var pocket in PocketInfos)
                    {
                        if (pocket.ItemInPocket.ItemStruct.ItemType == ItemType.Weapon)
                        {
                            weaponItems[enumerator] = pocket.ItemInPocket as WeaponItem;
                            enumerator++;
                        }
                    }

                    return weaponItems;
            }         
            else
            {
                return new WeaponItem[0];
            }
        }

        #endregion
    }
}

