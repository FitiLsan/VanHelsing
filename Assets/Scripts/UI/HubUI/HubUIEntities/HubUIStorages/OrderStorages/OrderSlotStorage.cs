using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class OrderSlotStorage : BaseSlotStorage<ItemOrderModel, OrderStorageType>
    {
        #region Properties

        public Action<OrderStorageType> OnStorageResizeHandler { get; set; }

        #endregion


        #region ClassLifeCycle

        public OrderSlotStorage(int slotsAmount) : base(slotsAmount, OrderStorageType.None) { }

        #endregion


        #region Methods

        public void AddSlots(int slotAmount)
        {
            if (slotAmount > 0)
            {
                for (int i = 0; i < slotAmount; i++)
                {
                    _elementSlots.Add(default);
                }
                OnStorageResize();
            }
            else
            {
                Debug.LogError($"Incorrect input parameter: parameter less or equal zero (input value: {slotAmount})");
            }
        }

        public bool HasFreeSlots()
        {
            for (int i = 0; i < _elementSlots.Count; i++)
            {
                if (_elementSlots[i] == null)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool PutElement(int slotIndex, ItemOrderModel order)
        {
            bool isSucceful = false;

            if (slotIndex < _elementSlots.Count)
            {
                if (_elementSlots[slotIndex] == null)
                {
                    _elementSlots[slotIndex] = order;
                    isSucceful = true;
                }

                if (isSucceful)
                {
                    OnPutElementToSlot(slotIndex, order);
                }
            }
            else
            {
                Debug.LogError($"Incorrect input parameter: slot index {slotIndex} outside the list");
            }

            return isSucceful;
        }

        public override bool PutElementToFirstEmptySlot(ItemOrderModel order)
        {
            for (int i = 0; i < _elementSlots.Count; i++)
            {
                if (_elementSlots[i] == null)
                {
                    return PutElement(i, order);
                }
            }
            return false;
        }

        private void OnStorageResize()
        {
            OnStorageResizeHandler?.Invoke(StorageType);
        }

        #endregion
    }
}
