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

        public abstract bool PutItem(int slotIndex, HubMapUIBaseItemModel item);
        public abstract bool PutItemToFirstEmptySlot(HubMapUIBaseItemModel item);

        public virtual bool RemoveItem(int slotIndex)
        {
            HubMapUIBaseItemModel takenItem = _items[slotIndex];
            _items[slotIndex] = null;
            OnTakeItemFromSlot(slotIndex, takenItem);
            return true;
        }

        public virtual HubMapUIBaseItemModel GetItemBySlot(int slotIndex)
        {
            return _items[slotIndex];
        }

        public virtual Sprite GetItemIconBySlot(int slotIndex)
        {
            if (_items[slotIndex] != null)
            {
                return _items[slotIndex].Icon;
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
                    RemoveItem(i);
                }
            }
        }

        protected virtual void OnPutItemToSlot(int slotIndex, HubMapUIBaseItemModel newItem)
        {
            OnPutItemToSlotHandler?.Invoke(StorageType, slotIndex, newItem);
        }

        protected virtual void OnTakeItemFromSlot(int slotIndex, HubMapUIBaseItemModel takedItem)
        {
            OnTakeItemFromSlotHandler?.Invoke(StorageType, slotIndex, takedItem);
        }

        #endregion
    }
}
