using UnityEngine;
using System.Collections.Generic;
using System;


namespace BeastHunter
{
    public class HubMapUIClothesEquipmentStorage : HubMapUIBaseItemStorage
    {
        #region Fields

        public override HubMapUIItemStorageType StorageType { get; protected set; }
        private HubMapUIClothesType[] _slotTypes;

        #endregion

  
        #region ClassLifeCycle

        public HubMapUIClothesEquipmentStorage(HubMapUIClothesType[] clothTypes, HubMapUIItemStorageType storageType)
        {
            StorageType = storageType;

            _items = new List<HubMapUIBaseItemModel>();
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
            HubMapUIClothesItemModel itemInSlot = _items[slotIndex] as HubMapUIClothesItemModel;

            if (itemInSlot != null)
            {
                if (IsEnoughEmptyPocketsFunc.Invoke(itemInSlot.PocketsAmount))
                {
                    return base.RemoveItem(slotIndex);
                }
                HubMapUIServices.SharedInstance.GameMessages.Notice($"For taking off that clothes {itemInSlot.PocketsAmount} pockets slots need to be emptied");
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool PutItem(int slotIndex, HubMapUIBaseItemModel item)
        {
            bool isSucceful = false;

            if (item != null)
            {
                if (item.ItemType == HubMapUIItemType.Cloth)
                {
                    if ((item as HubMapUIClothesItemModel).ClothesType == _slotTypes[slotIndex])
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
                        HubMapUIServices.SharedInstance.GameMessages.Notice("The clothes is not the right type");
                    }
                }
                else
                {
                    HubMapUIServices.SharedInstance.GameMessages.Notice("Putting item is not clothes");
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

        public override bool PutItemToFirstEmptySlot(HubMapUIBaseItemModel item)
        {
            if (item != null)
            {
                if (item.ItemType == HubMapUIItemType.Cloth)
                {
                    HubMapUIClothesItemModel clothItem = item as HubMapUIClothesItemModel;
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
                    HubMapUIServices.SharedInstance.GameMessages.Notice("Putting item is not clothes");
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

        public int? GetFirstSlotIndexForItem(HubMapUIClothesItemModel item)
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
