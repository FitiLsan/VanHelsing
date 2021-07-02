using UnityEngine;


namespace BeastHunterHubUI
{
    public class ItemInitializeService
    {
        #region Methods

        public BaseItemModel InitializeItemModel(BaseItemSO data)
        {
            switch (data.ItemType)
            {
                case ItemType.None:
                    return new BaseItemModel(data);

                case ItemType.Clothes:
                    return new ClothesItemModel(data as ClothesItemSO);

                case ItemType.PocketItem:
                    return new PocketItemModel(data as PocketItemSO);

                case ItemType.Weapon:
                    return new WeaponItemModel(data as WeaponItemSO);

                case ItemType.Ingredient:
                    return new IngredientItemModel(data as IngredientItemSO);

                default:
                    Debug.LogError(this + ": incorrect HubMapUIItemType");
                    return null;
            }
        }

        #endregion

    }
}
