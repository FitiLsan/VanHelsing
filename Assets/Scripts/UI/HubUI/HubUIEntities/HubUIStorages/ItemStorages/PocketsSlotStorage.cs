using System;
using System.Collections.Generic;


namespace BeastHunterHubUI
{
    public class PocketsSlotStorage : ItemSlotStorage
    {
        #region Properties

        public Action OnChangeSlotsAmountHandler { get; set; }

        #endregion


        #region ClassLifeCycle

        public PocketsSlotStorage() : base(0, ItemStorageType.PocketsStorage) { }

        #endregion


        #region Methods

        public void AddPockets(int amount)
        {
            if(amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    _elementSlots.Add(null);
                }

                OnChangeSlotsAmountHandler?.Invoke();
            }
        }

        public bool IsEnoughFreeSlots(int amount)
        {
            if (amount > 0)
            {
                int freeSlots = 0;
                for (int i = 0; i < _elementSlots.Count; i++)
                {
                    if (_elementSlots[i] == null)
                    {
                        freeSlots++;
                        if (freeSlots >= amount)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            return true;
        }

        public bool RemovePockets(int amount)
        {
            if (amount > 0)
            {
                List<int> slotsToDelete = new List<int>();
                for (int i = 0; i < _elementSlots.Count; i++)
                {
                    if (_elementSlots[i] == null)
                    {
                        slotsToDelete.Add(i);
                        if (slotsToDelete.Count >= amount)
                        {
                            break;
                        }
                    }
                }

                if (slotsToDelete.Count < amount)
                {
                    return false;
                }
                else
                {
                    for (int i = slotsToDelete.Count - 1; i >= 0; i--)
                    {
                        _elementSlots.RemoveAt(slotsToDelete[i]);
                    }
                    OnChangeSlotsAmountHandler?.Invoke();
                    return true;
                }
            }
            return true;
        }

        public override bool PutElement(int slotIndex, BaseItemModel item)
        {
            if (item != null)
            {
                if (item.ItemType == ItemType.PocketItem)
                {
                    return base.PutElement(slotIndex, item);
                }
                else
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("This is not a pocket thing");
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
