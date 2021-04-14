using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public abstract class HubMapUIBaseItemStorage
    {
        #region Fields

        protected List<HubMapUIBaseItemModel> _items;

        #endregion


        #region Properties

        public virtual Action<HubMapUIItemStorageType, int, HubMapUIBaseItemModel> OnPutItemToSlotHandler { get; set; }
        public virtual Action<HubMapUIItemStorageType, int, HubMapUIBaseItemModel> OnTakeItemFromSlotHandler { get; set; }

        public abstract HubMapUIItemStorageType StorageType { get; protected set; }

        #endregion


        #region Methods

        public abstract bool PutItem(int slotNumber, HubMapUIBaseItemModel item);
        public abstract bool PutItemToFirstEmptySlot(HubMapUIBaseItemModel item);

        public virtual HubMapUIBaseItemModel TakeItem(int slotNumber)
        {
            HubMapUIBaseItemModel takedItem = _items[slotNumber];
            _items[slotNumber] = null;
            OnTakeItemFromSlot(slotNumber, takedItem);
            return takedItem;
        }

        public virtual HubMapUIBaseItemModel GetItemBySlot(int slotNumber)
        {
            return _items[slotNumber];
        }

        public virtual Sprite GetItemIconBySlot(int slotNumber)
        {
            if (_items[slotNumber] != null)
            {
                return _items[slotNumber].Icon;
            }
            else
            {
                return null;
            }
        }

        public virtual int GetSlotsCount()
        {
            return _items.Count;
        }

        public virtual void Clear()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] != null)
                {
                    TakeItem(i);
                }
            }
        }

        protected virtual void OnPutItemToSlot(int slotNumber, HubMapUIBaseItemModel newItem)
        {
            OnPutItemToSlotHandler?.Invoke(StorageType, slotNumber, newItem);
        }

        protected virtual void OnTakeItemFromSlot(int slotNumber, HubMapUIBaseItemModel takedItem)
        {
            OnTakeItemFromSlotHandler?.Invoke(StorageType, slotNumber, takedItem);
        }

        #endregion
    }
}
