using System.Collections.Generic;


namespace BeastHunterHubUI
{
    public class ItemStorage : BaseItemStorage
    {
        #region Properties

        public override ItemStorageType StorageType { get; protected set; }

        #endregion


        #region ClassLifeCycle

        public ItemStorage(int slotsAmount, ItemStorageType storageType)
        {
            StorageType = storageType;
            _items = new List<BaseItemModel>();
            for (int i = 0; i < slotsAmount; i++)
            {
                _items.Add(null);
            }
        }

        #endregion


        #region Methods

        public override bool PutItem(int slotIndex, BaseItemModel item)
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
                HubUIServices.SharedInstance.GameMessages.Notice(StorageType + " is full");
            }

            return isSucceful;
        }

        public override bool PutItemToFirstEmptySlot(BaseItemModel item)
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

        public bool IsContainItem(BaseItemData item)
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

        public bool RemoveFirstItem(BaseItemData item)
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
