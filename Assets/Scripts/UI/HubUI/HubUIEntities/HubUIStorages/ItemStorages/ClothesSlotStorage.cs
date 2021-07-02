using UnityEngine;
using System.Collections.Generic;
using System;


namespace BeastHunterHubUI
{
    public class ClothesSlotStorage : BaseItemSlotStorage
    {
        #region Fields

        private ClothesType[] _slotTypes;

        #endregion


        #region ClassLifeCycle

        public ClothesSlotStorage(ClothesType[] clothTypes) : base(clothTypes.Length, ItemStorageType.ClothesEquipment)
        {
            _slotTypes = clothTypes;
        }

        #endregion


        #region Methods

        public Func<int, bool> IsEnoughEmptyPocketsFunc { get; set; }

        public override bool RemoveElement(int slotIndex)
        {
            ClothesItemModel itemInSlot = _elementSlots[slotIndex] as ClothesItemModel;

            if (itemInSlot != null)
            {
                if (IsEnoughEmptyPocketsFunc.Invoke(itemInSlot.PocketsAmount))
                {
                    return base.RemoveElement(slotIndex);
                }
                HubUIServices.SharedInstance.GameMessages.Notice($"For taking off that clothes {itemInSlot.PocketsAmount} pockets slots need to be emptied");
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool PutElement(int slotIndex, BaseItemModel item)
        {
            bool isSucceful = false;

            if (item != null)
            {
                if (item.ItemType == ItemType.Clothes)
                {
                    if ((item as ClothesItemModel).ClothesType == _slotTypes[slotIndex])
                    {
                        if (_elementSlots[slotIndex] == null)
                        {
                            _elementSlots[slotIndex] = item;
                            isSucceful = true;
                        }
                        else
                        {
                            isSucceful = PutElementToFirstEmptySlot(item);
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

        public override bool PutElementToFirstEmptySlot(BaseItemModel item)
        {
            if (item != null)
            {
                if (item.ItemType == ItemType.Clothes)
                {
                    ClothesItemModel clothItem = item as ClothesItemModel;
                    for (int i = 0; i < _elementSlots.Count; i++)
                    {
                        if (_slotTypes[i] == clothItem.ClothesType)
                        {
                            if (_elementSlots[i] == null)
                            {
                                return PutElement(i, item);
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
            for (int i = 0; i < _elementSlots.Count; i++)
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
