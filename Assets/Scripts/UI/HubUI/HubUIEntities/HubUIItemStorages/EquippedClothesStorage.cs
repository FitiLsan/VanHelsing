using UnityEngine;
using System.Collections.Generic;
using System;


namespace BeastHunterHubUI
{
    public class EquippedClothesStorage : BaseItemStorage
    {
        #region Fields

        public override ItemStorageType StorageType { get; protected set; }
        private ClothesType[] _slotTypes;

        #endregion


        #region ClassLifeCycle

        public EquippedClothesStorage(ClothesType[] clothTypes)
        {
            StorageType = ItemStorageType.ClothesEquipment;

            _items = new List<BaseItemModel>();
            for (int i = 0; i < clothTypes.Length; i++)
            {
                _items.Add(null);
            }

            _slotTypes = clothTypes;
        }

        #endregion


        #region Methods

        public Func<int, bool> IsEnoughEmptyPocketsFunc { get; set; }

        public override bool RemoveItem(int slotIndex)
        {
            ClothesItemModel itemInSlot = _items[slotIndex] as ClothesItemModel;

            if (itemInSlot != null)
            {
                if (IsEnoughEmptyPocketsFunc.Invoke(itemInSlot.PocketsAmount))
                {
                    return base.RemoveItem(slotIndex);
                }
                HubUIServices.SharedInstance.GameMessages.Notice($"For taking off that clothes {itemInSlot.PocketsAmount} pockets slots need to be emptied");
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool PutItem(int slotIndex, BaseItemModel item)
        {
            bool isSucceful = false;

            if (item != null)
            {
                if (item.ItemType == ItemType.Clothes)
                {
                    if ((item as ClothesItemModel).ClothesType == _slotTypes[slotIndex])
                    {
                        if (_items[slotIndex] == null)
                        {
                            _items[slotIndex] = item;
                            isSucceful = true;
                        }
                        else
                        {
                            isSucceful = PutItemToFirstEmptySlot(item);
                        }
                    }
                    else
                    {
                        HubUIServices.SharedInstance.GameMessages.Notice("The clothes is not the right type");
                    }
                }
                else
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("Putting item is not clothes");
                }
            }
            else
            {
                isSucceful = true;
            }

            if (isSucceful)
            {
                OnPutItemToSlot(slotIndex, item);
            }

            return isSucceful;
        }

        public override bool PutItemToFirstEmptySlot(BaseItemModel item)
        {
            if (item != null)
            {
                if (item.ItemType == ItemType.Clothes)
                {
                    ClothesItemModel clothItem = item as ClothesItemModel;
                    for (int i = 0; i < _items.Count; i++)
                    {
                        if (_slotTypes[i] == clothItem.ClothesType)
                        {
                            if (_items[i] == null)
                            {
                                return PutItem(i, item);
                            }
                        }
                    }
                }
                else
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("Putting item is not clothes");
                    return false;
                }
            }
            else
            {
                return true;
            }

            Debug.Log("No free slot of suitable clothes type found");
            return false;
        }

        public int? GetFirstSlotIndexForItem(ClothesItemModel item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_slotTypes[i] == item.ClothesType)
                {
                    return i;
                }
            }
            return null;
        }

        #endregion
    }
}
