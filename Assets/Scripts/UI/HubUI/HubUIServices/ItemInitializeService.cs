using UnityEngine;


namespace BeastHunterHubUI
{
    public class ItemInitializeService
    {
        #region Methods

        public BaseItemModel InitializeItemModel(BaseItemData data)
        {
            switch (data.ItemType)
            {
                case ItemType.None:
                    return new BaseItemModel(data);

                case ItemType.Clothes:
                    return new ClothesItemModel(data as ClothesItemData);

                case ItemType.PocketItem:
                    return new PocketItemModel(data as PocketItemData);

                case ItemType.Weapon:
                    return new WeaponItemModel(data as WeaponItemData);

                default:
                    Debug.LogError(this + ": incorrect HubMapUIItemType");
                    return null;
            }
        }

        #endregion

    }
}
