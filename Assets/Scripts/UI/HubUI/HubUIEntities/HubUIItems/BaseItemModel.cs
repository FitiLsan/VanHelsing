using UnityEngine;


namespace BeastHunterHubUI
{
    public class BaseItemModel
    {
        #region Properties

        public ItemType ItemType { get; private set; }
        public int DataInstanceID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }
        public int ShopPrice { get; private set; }
        public int RequiredReputationForSaleInShop { get; private set; }

        #endregion


        public BaseItemModel(BaseItemSO data)
        {
            ItemType = data.ItemType;
            DataInstanceID = data.GetInstanceID();
            Name = data.Name;
            Description = data.Description;
            Icon = data.Icon;
            ShopPrice = data.ShopPrice;
            RequiredReputationForSaleInShop = data.RequiredReputationForSaleInShop;
        }
    }
}
