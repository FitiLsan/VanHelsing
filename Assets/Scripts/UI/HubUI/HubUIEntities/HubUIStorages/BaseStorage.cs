using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public abstract class BaseStorage<ElementType, EnumStorageType> where EnumStorageType : Enum
    {
        #region Fields

        protected List<ElementType> _elementSlots;

        #endregion


        #region Properties

        public EnumStorageType StorageType { get; private set; }

        #endregion


        #region ClassLifeCycle

        public BaseStorage(EnumStorageType storageType)
        {
            StorageType = storageType;
        }

        #endregion


        #region Methods

        public abstract bool PutElement(int slotIndex, ElementType element);
        public abstract bool PutElementToFirstEmptySlot(ElementType element);
        public abstract bool RemoveElement(int slotIndex);
        public abstract void ClearSlots();

        public virtual ElementType GetElementBySlot(int slotIndex)
        {
            if (slotIndex < _elementSlots.Count)
            {
                return _elementSlots[slotIndex];
            }
            else
            {
                Debug.LogError(this + "Incorrect input parameter: slot index exceeds the number of slots in list");
                return default;
            }
        }

        public virtual int GetSlotsCount()
        {
            return _elementSlots.Count;
        }

        public virtual void SwapElementsWithOtherStorage(int currentStorageSlotIndex, BaseStorage<ElementType, EnumStorageType> otherStorage, int otherStorageSlotIndex)
        {
            ElementType currentStorageElement = this.GetElementBySlot(currentStorageSlotIndex);
            ElementType otherStorageElement = otherStorage.GetElementBySlot(otherStorageSlotIndex);
            bool isSuccefulTakeElements = false;
            bool isSuccefullPutElements = false;

            if (this.RemoveElement(currentStorageSlotIndex))
            {
                if (otherStorage.RemoveElement(otherStorageSlotIndex))
                {
                    isSuccefulTakeElements = true;
                }
                else
                {
                    this.PutElement(currentStorageSlotIndex, currentStorageElement);
                }
            }

            if (isSuccefulTakeElements)
            {
                isSuccefullPutElements =
                    this.PutElement(currentStorageSlotIndex, otherStorageElement) &&
                    otherStorage.PutElement(otherStorageSlotIndex, currentStorageElement);

                if (!isSuccefullPutElements)
                {
                    this.RemoveElement(currentStorageSlotIndex);
                    this.PutElement(currentStorageSlotIndex, currentStorageElement);

                    otherStorage.RemoveElement(otherStorageSlotIndex);
                    otherStorage.PutElement(otherStorageSlotIndex, otherStorageElement);
                }
            }

            if (!(isSuccefulTakeElements && isSuccefullPutElements))
            {
                Debug.LogWarning("Drag and drop swap elements operation was not successful");
            }
        }

        public virtual bool PutElementToFirstEmptySlotFromOtherStorage(BaseStorage<ElementType, EnumStorageType> otherStorage, int otherStorageSlotIndex)
        {
            ElementType element = otherStorage.GetElementBySlot(otherStorageSlotIndex);
            if (element != null)
            {
                if (this.PutElementToFirstEmptySlot(element))
                {
                    otherStorage.RemoveElement(otherStorageSlotIndex);
                    return true;
                }
                else
                {
                    Debug.Log($"The storage {StorageType} is full");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
