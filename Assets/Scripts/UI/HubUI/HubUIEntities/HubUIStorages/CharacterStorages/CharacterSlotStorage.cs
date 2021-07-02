using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class CharacterSlotStorage : BaseCharacterStorage
    {

        #region Properties

        public Action<CharacterStorageType, int, CharacterModel> OnPutElementToSlotHandler { get; set; }
        public Action<CharacterStorageType, int, CharacterModel> OnTakeElementFromSlotHandler { get; set; }
        public Action<CharacterStorageType> OnStorageResizeHandler { get; set; }


        #endregion


        #region ClassLifeCycle

        public CharacterSlotStorage(int slotsAmount, CharacterStorageType storageType) : base(storageType)
        {
            _elementSlots = new List<CharacterModel>();

            for (int i = 0; i < slotsAmount; i++)
            {
                _elementSlots.Add(default);
            }
        }

        #endregion


        #region Methods

        public override bool PutElement(int slotIndex, CharacterModel character)
        {
            bool isSucceful = false;

            if (_elementSlots[slotIndex] == null)
            {
                _elementSlots[slotIndex] = character;
                isSucceful = true;
            }
            else
            {
                isSucceful = PutElementToFirstEmptySlot(character);
            }

            if (isSucceful)
            {
                if(character != null)
                {
                    OnPutElementToSlot(slotIndex, character);
                }
            }
            else
            {
                HubUIServices.SharedInstance.GameMessages.Notice(StorageType + " is full");
            }

            return isSucceful;
        }

        public override bool PutElementToFirstEmptySlot(CharacterModel character)
        {
            for (int i = 0; i < _elementSlots.Count; i++)
            {
                if (_elementSlots[i] == null)
                {
                    return PutElement(i, character);
                }
            }
            return false;
        }

        public override bool RemoveElement(int slotIndex)
        {
            if (_elementSlots[slotIndex] != null)
            {
                CharacterModel takenElement = _elementSlots[slotIndex];
                _elementSlots[slotIndex] = default;
                OnTakeElementFromSlot(slotIndex, takenElement);
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

        public virtual void AddSlots(int slotAmount)
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

        private void OnPutElementToSlot(int slotIndex, CharacterModel newElement)
        {
            OnPutElementToSlotHandler?.Invoke(StorageType, slotIndex, newElement);
        }

        private void OnTakeElementFromSlot(int slotIndex, CharacterModel takedElement)
        {
            OnTakeElementFromSlotHandler?.Invoke(StorageType, slotIndex, takedElement);
        }

        private void OnStorageResize()
        {
            OnStorageResizeHandler?.Invoke(StorageType);
        }

        #endregion
    }
}
