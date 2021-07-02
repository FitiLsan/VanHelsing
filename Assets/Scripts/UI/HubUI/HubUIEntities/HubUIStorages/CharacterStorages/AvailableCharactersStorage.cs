using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class AvailableCharactersStorage : BaseCharacterStorage
    {
        public Action<int, CharacterModel> OnAddCharacterHandler { get; set; }
        public Action<int> OnRemoveCharacterHandler { get; set; }
        public Action<int, CharacterModel> OnReplaceCharacterHandler { get; set; }


        public AvailableCharactersStorage(CharacterStorageType storageType) : base(storageType)
        {
            _elementSlots = new List<CharacterModel>();
        }


        public override void ClearSlots()
        {
            _elementSlots.Clear();
        }

        public override bool PutElement(int slotIndex, CharacterModel character)
        {
            if(character != null)
            {
                _elementSlots.Insert(slotIndex, character);
                OnAddCharacterHandler?.Invoke(slotIndex, character);
            }
            return true;
        }

        public override bool PutElementToFirstEmptySlot(CharacterModel character)
        {
            if(character != null)
            {
                _elementSlots.Add(character);
                OnAddCharacterHandler?.Invoke(_elementSlots.Count-1, character);
            }
            return true;
        }

        public override bool RemoveElement(int slotIndex)
        {
            if(slotIndex < _elementSlots.Count)
            {
                _elementSlots.RemoveAt(slotIndex);
                OnRemoveCharacterHandler?.Invoke(slotIndex);
            }
            else
            {
                Debug.LogError(this + "Incorrect input parameter: slot index exceeds the number of slots in list");
            }
            return true;
        }

        public bool RemoveElement(CharacterModel character)
        {
            if (_elementSlots.Contains(character))
            {
                int index = _elementSlots.IndexOf(character);
                _elementSlots.Remove(character);
                OnRemoveCharacterHandler?.Invoke(index);
            }
            else
            {
                Debug.LogError(this + "Incorrect input parameter: character is not found");
            }
            return true;
        }

        public override void SwapElementsWithOtherStorage(int currentStorageSlotIndex, BaseStorage<CharacterModel, CharacterStorageType> otherStorage, int otherStorageSlotIndex)
        {
            CharacterModel currentStorageElement = this.GetElementBySlot(currentStorageSlotIndex);
            CharacterModel otherStorageElement = otherStorage.GetElementBySlot(otherStorageSlotIndex);

            if (otherStorage.RemoveElement(otherStorageSlotIndex))
            {
                if (otherStorage.PutElement(otherStorageSlotIndex, currentStorageElement))
                {
                    if (otherStorageElement == null)
                    {
                        this.RemoveElement(currentStorageSlotIndex);
                    }
                    else
                    {
                        this.ReplaceElement(currentStorageSlotIndex, otherStorageElement);
                    }
                }
                else
                {
                    Debug.LogError("otherStorage.PutElement() is not successful");
                    otherStorage.PutElement(otherStorageSlotIndex, otherStorageElement);
                }
            }
            else
            {
                Debug.LogError("otherStorage.RemoveElement() is not successful");
            }
        }

        private void ReplaceElement(int slotIndex, CharacterModel character)
        {
            _elementSlots[slotIndex] = character;
            OnReplaceCharacterHandler?.Invoke(slotIndex, character);
        }
    }
}
