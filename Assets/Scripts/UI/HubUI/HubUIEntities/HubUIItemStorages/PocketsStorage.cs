using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class PocketsStorage : ItemStorage
    {
        #region Properties

        public Action OnChangeSlotsAmountHandler { get; set; }

        #endregion


        #region ClassLifeCycle

        public PocketsStorage() : base(0, ItemStorageType.PocketsStorage) { }

        #endregion


        #region Methods

        public void AddPockets(int amount)
        {
            if(amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    _items.Add(null);
                }

                OnChangeSlotsAmountHandler?.Invoke();
            }
        }

        public bool IsEnoughFreeSlots(int amount)
        {
            if (amount > 0)
            {
                int freeSlots = 0;
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i] == null)
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
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i] == null)
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
                        _items.RemoveAt(slotsToDelete[i]);
                    }
                    OnChangeSlotsAmountHandler?.Invoke();
                    return true;
                }
            }
            return true;
        }

        public override bool PutItem(int slotIndex, BaseItemModel item)
        {
            if (item != null)
            {
                if (item.ItemType == ItemType.PocketItem)
                {
                    return base.PutItem(slotIndex, item);
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
