using System.Collections.Generic;


namespace BeastHunter
{
    public class HubMapUIItemStorage : HubMapUIBaseItemStorage
    {
        public override HubMapUIItemStorageType StorageType { get; protected set; }


        #region ClassLifeCycle

        public HubMapUIItemStorage(int slotsAmount, HubMapUIItemStorageType storageType)
        {
            StorageType = storageType;
            _items = new List<HubMapUIBaseItemModel>();
            for (int i = 0; i < slotsAmount; i++)
            {
                _items.Add(null);
            }
        }

        #endregion


        #region Methods

        public override bool PutItem(int slotIndex, HubMapUIBaseItemModel item)
        {
            bool isSucceful = false;

            if (_items[slotIndex] == null)
            {
                _items[slotIndex] = item;
                isSucceful = true;
            }
            else
            {
                isSucceful = PutItemToFirstEmptySlot(item);
            }

            if (isSucceful)
            {
                OnPutItemToSlot(slotIndex, item);
            }
            else
            {
                HubMapUIServices.SharedInstance.GameMessages.Notice(StorageType + " is full");
            }

            return isSucceful;
        }

        public override bool PutItemToFirstEmptySlot(HubMapUIBaseItemModel item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] == null)
                {
                    return PutItem(i, item);
                }
            }
            return false;
        }

        public bool IsFull()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsContainItem(HubMapUIBaseItemData item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].DataInstanceID == item.GetInstanceID())
                {
                    return true;
                }
            }
            return false;
        }

        public bool RemoveFirstItem(HubMapUIBaseItemData item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].DataInstanceID == item.GetInstanceID())
                {
                    RemoveItem(i);
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
