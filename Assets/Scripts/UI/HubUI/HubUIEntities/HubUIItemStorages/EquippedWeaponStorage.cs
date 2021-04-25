using System;
using UnityEngine;
using System.Collections.Generic;


namespace BeastHunterHubUI
{
    public class EquippedWeaponStorage : BaseItemStorage
    {
        #region Constants

        private const int SLOTS_AMOUNT_IN_WEAPON_SET = 2;

        #endregion


        #region Properties

        public Action<int, Sprite> OnTwoHandedWeaponHandler;

        public override ItemStorageType StorageType { get; protected set; }

        #endregion


        #region ClassLifeCycle

        public EquippedWeaponStorage(int weaponSetsAmount)
        {
            StorageType = ItemStorageType.WeaponEquipment;

            _items = new List<BaseItemModel>();
            int slotsAmount = weaponSetsAmount * SLOTS_AMOUNT_IN_WEAPON_SET;
            for (int i = 0; i < slotsAmount; i++)
            {
                _items.Add(null);
            }
        }

        #endregion


        #region Methods

        public override bool PutItem(int slotIndex, BaseItemModel item)
        {
            bool isSucceful = false;
            int putSlotIndex = slotIndex;
            int? adjacendSlotIndex = null;

            if (item != null)
            {
                if (item.ItemType == ItemType.Weapon)
                {
                    WeaponItemModel weapon = item as WeaponItemModel;

                    if (_items[slotIndex] == null && AdjacentWeapon(slotIndex) == null)
                    {
                        if (IsEvenSlotIndex(slotIndex) || !weapon.IsTwoHanded)
                        {
                            if (weapon.IsTwoHanded)
                            {
                                adjacendSlotIndex = AdjacentSlotIndex(slotIndex);
                            }
                            _items[slotIndex] = item;
                            isSucceful = true;
                        }
                        else
                        {
                            _items[AdjacentSlotIndex(slotIndex)] = item;
                            putSlotIndex = AdjacentSlotIndex(slotIndex);
                            adjacendSlotIndex = slotIndex;
                            isSucceful = true;
                        }
                    }
                    else if (_items[slotIndex] == null && !AdjacentWeapon(slotIndex).IsTwoHanded && !weapon.IsTwoHanded)
                    {
                        _items[slotIndex] = item;
                        isSucceful = true;
                    }
                    else
                    {
                        HubUIServices.SharedInstance.GameMessages.Notice("There is already an weapon in the set");
                        isSucceful = false;
                    }
                }
                else
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("Putting item is not weapon");
                    isSucceful = false;
                }
            }
            else
            {
                if (_items[slotIndex] == null)
                {
                    return true;
                }
                else
                {
                    _items[slotIndex] = null;
                    isSucceful = true;
                }
            }

            if (isSucceful)
            {
                OnPutItemToSlot(putSlotIndex, _items[putSlotIndex]);
            }

            if (adjacendSlotIndex.HasValue)
            {
                OnTwoHandedWeapon(adjacendSlotIndex.Value, GetItemIconBySlot(putSlotIndex));
            }

            return isSucceful;
        }

        public override bool PutItemToFirstEmptySlot(BaseItemModel item)
        {
            if (item != null)
            {
                if (item.ItemType == ItemType.Weapon)
                {
                    WeaponItemModel weapon = item as WeaponItemModel;

                    if (FindFirstEmptySlotByWeaponGripType(weapon.IsTwoHanded, out int? emptySlot))
                    {
                        return PutItem(emptySlot.Value, item);
                    }
                }
                else
                {
                    Debug.Log($"PutItemToFirstEmptySlot: {item.Name} is not weapon");
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        public override bool RemoveItem(int slotIndex)
        {
            if (_items[slotIndex] != null && (_items[slotIndex] as WeaponItemModel).IsTwoHanded)
            {
                OnTwoHandedWeapon(AdjacentSlotIndex(slotIndex), null);
            }
            return base.RemoveItem(slotIndex);
        }

        private bool FindFirstEmptySlotByWeaponGripType(bool isTwoHandedWeapon, out int? slotIndex)
        {
            slotIndex = null;

            if (isTwoHandedWeapon)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i] == null && AdjacentWeapon(i) == null)
                    {
                        slotIndex = i;
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i] == null && (AdjacentWeapon(i) == null || !AdjacentWeapon(i).IsTwoHanded))
                    {
                        slotIndex = i;
                        return true;
                    }
                }
            }

            Debug.Log($"No empty slots for {(isTwoHandedWeapon? "two-handed" : "one-handed")} weapon");
            return false;
        }

        public WeaponItemModel AdjacentWeapon(int slotIndex)
        {
            return GetItemBySlot(AdjacentSlotIndex(slotIndex)) as WeaponItemModel;
        }

        private bool IsEvenSlotIndex(int slotIndex)
        {
            return (float)slotIndex % 2 == 0;
        }

        private int AdjacentSlotIndex(int slotIndex)
        {
            if (IsEvenSlotIndex(slotIndex))
            {
                return slotIndex + 1;
            }
            else
            {
                return slotIndex - 1;
            }
        }

        private void OnTwoHandedWeapon(int adjacentSlotIndex, Sprite sprite)
        {
            OnTwoHandedWeaponHandler?.Invoke(adjacentSlotIndex, sprite);
        }

        #endregion
    }
}
