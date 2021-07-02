using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class WeaponSlotStorage : BaseItemSlotStorage
    {
        #region Constants

        private const int SLOTS_AMOUNT_IN_WEAPON_SET = 2;

        #endregion


        #region Properties

        public Action<int, WeaponItemModel> OnTwoHandedWeaponHandler;

        #endregion


        #region ClassLifeCycle

        public WeaponSlotStorage(int weaponSetsAmount) : base(weaponSetsAmount * SLOTS_AMOUNT_IN_WEAPON_SET, ItemStorageType.WeaponEquipment) { }

        #endregion


        #region Methods

        public override bool PutElement(int slotIndex, BaseItemModel item)
        {
            bool isSucceful = false;
            int putSlotIndex = slotIndex;
            int? adjacendSlotIndex = null;

            if (item != null)
            {
                if (item.ItemType == ItemType.Weapon)
                {
                    WeaponItemModel weapon = item as WeaponItemModel;

                    if (_elementSlots[slotIndex] == null && AdjacentWeapon(slotIndex) == null)
                    {
                        if (IsEvenSlotIndex(slotIndex) || !weapon.IsTwoHanded)
                        {
                            if (weapon.IsTwoHanded)
                            {
                                adjacendSlotIndex = AdjacentSlotIndex(slotIndex);
                            }
                            _elementSlots[slotIndex] = item;
                            isSucceful = true;
                        }
                        else
                        {
                            _elementSlots[AdjacentSlotIndex(slotIndex)] = item;
                            putSlotIndex = AdjacentSlotIndex(slotIndex);
                            adjacendSlotIndex = slotIndex;
                            isSucceful = true;
                        }
                    }
                    else if (_elementSlots[slotIndex] == null && !AdjacentWeapon(slotIndex).IsTwoHanded && !weapon.IsTwoHanded)
                    {
                        _elementSlots[slotIndex] = item;
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
                if (_elementSlots[slotIndex] == null)
                {
                    return true;
                }
                else
                {
                    _elementSlots[slotIndex] = null;
                    isSucceful = true;
                }
            }

            if (isSucceful)
            {
                OnPutItemToSlot(putSlotIndex, _elementSlots[putSlotIndex]);
            }

            if (adjacendSlotIndex.HasValue)
            {
                OnTwoHandedWeapon(adjacendSlotIndex.Value, GetElementBySlot(putSlotIndex) as WeaponItemModel);
            }

            return isSucceful;
        }

        public override bool PutElementToFirstEmptySlot(BaseItemModel item)
        {
            if (item != null)
            {
                if (item.ItemType == ItemType.Weapon)
                {
                    WeaponItemModel weapon = item as WeaponItemModel;

                    if (FindFirstEmptySlotByWeaponGripType(weapon.IsTwoHanded, out int? emptySlot))
                    {
                        return PutElement(emptySlot.Value, item);
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

        public override bool RemoveElement(int slotIndex)
        {
            if (_elementSlots[slotIndex] != null && (_elementSlots[slotIndex] as WeaponItemModel).IsTwoHanded)
            {
                OnTwoHandedWeapon(AdjacentSlotIndex(slotIndex), null);
            }
            return base.RemoveElement(slotIndex);
        }

        private bool FindFirstEmptySlotByWeaponGripType(bool isTwoHandedWeapon, out int? slotIndex)
        {
            slotIndex = null;

            if (isTwoHandedWeapon)
            {
                for (int i = 0; i < _elementSlots.Count; i++)
                {
                    if (_elementSlots[i] == null && AdjacentWeapon(i) == null)
                    {
                        slotIndex = i;
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _elementSlots.Count; i++)
                {
                    if (_elementSlots[i] == null && (AdjacentWeapon(i) == null || !AdjacentWeapon(i).IsTwoHanded))
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
            return GetElementBySlot(AdjacentSlotIndex(slotIndex)) as WeaponItemModel;
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

        private void OnTwoHandedWeapon(int adjacentSlotIndex, WeaponItemModel weapon)
        {
            OnTwoHandedWeaponHandler?.Invoke(adjacentSlotIndex, weapon);
        }

        #endregion
    }
}
