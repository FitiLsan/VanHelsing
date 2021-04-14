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

        public override HubMapUIBaseItemModel TakeItem(int slotNumber)
        {
            HubMapUIClothesItemModel takenItem = _items[slotNumber] as HubMapUIClothesItemModel;

            if (takenItem != null)
            {
                if (IsEnoughEmptyPocketsFunc.Invoke(takenItem.PocketsAmount))
                {
                    return base.TakeItem(slotNumber);
                }
                //todo: UI message about need to free pocket slots
                Debug.LogWarning($"For taking off that clothes {takenItem.PocketsAmount} pockets slots need to be emptied");
                return null;
            }
            return null;
        }

        public override bool PutItem(int slotNumber, HubMapUIBaseItemModel item)
        {
            bool isSucceful = false;

            if (item != null)
            {
                if (item.ItemType == HubMapUIItemType.Cloth)
                {
                    if ((item as HubMapUIClothesItemModel).ClothesType == _slotTypes[slotNumber])
                    {
                        if (_items[slotNumber] == null)
                        {
                            _items[slotNumber] = item;
                            isSucceful = true;
                        }
                        else
                        {
                            isSucceful = PutItemToFirstEmptySlot(item);
                        }
                    }
                    else
                    {
                        Debug.Log("The clothing is not the right type");
                    }
                }
                else
                {
                    Debug.Log("The item is not clothing");
                }
            }
            else
            {
                isSucceful = true;
            }

            if (isSucceful)
            {
                OnPutItemToSlot(slotNumber, item);
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
            }
            else
            {
                return true;
            }

            Debug.Log("No free slot of suitable cloth type found");
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
