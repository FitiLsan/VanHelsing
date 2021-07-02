using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public abstract class BaseItemSlotStorage : BaseStorage<BaseItemModel, ItemStorageType>
    {
        #region Properties

        public Action<ItemStorageType, int, BaseItemModel> OnPutItemToSlotHandler { get; set; }
        public Action<ItemStorageType, int, BaseItemModel> OnTakeItemFromSlotHandler { get; set; }

        #endregion


        #region ClassLifeCycle

        public BaseItemSlotStorage(int slotsAmount, ItemStorageType storageType) : base(storageType)
        {
            _elementSlots = new List<BaseItemModel>();

            for (int i = 0; i < slotsAmount; i++)
            {
                _elementSlots.Add(default);
            }
        }

        #endregion


        #region Methods

        public override bool RemoveElement(int slotIndex)
        {
            if (_elementSlots[slotIndex] != null)
            {
                BaseItemModel takenElement = _elementSlots[slotIndex];
                _elementSlots[slotIndex] = default;
                OnTakeItemFromSlot(slotIndex, takenElement);
            }
            return true;
        }

        public override void ClearSlots()
        {
            for (int i = 0; i < _elementSlots.Count; i++)
            {
                if (_elementSlots[i] != null)
                {
                    RemoveElement(i);
                }
            }
        }

        protected void OnPutItemToSlot(int slotIndex, BaseItemModel newElement)
        {
            OnPutItemToSlotHandler?.Invoke(StorageType, slotIndex, newElement);
        }

        protected void OnTakeItemFromSlot(int slotIndex, BaseItemModel takedElement)
        {
            OnTakeItemFromSlotHandler?.Invoke(StorageType, slotIndex, takedElement);
        }

        #endregion
    }
}
