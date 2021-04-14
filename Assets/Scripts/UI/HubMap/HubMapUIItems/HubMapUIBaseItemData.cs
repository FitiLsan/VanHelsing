using UnityEngine;


namespace BeastHunter
{
    public abstract class HubMapUIBaseItemData : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _shopPrice;
        [SerializeField] private int _requiredReputationForBuyInShop;

        #endregion


        #region Properties

        public abstract HubMapUIItemType ItemType { get; protected set; }
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public int ShopPrice => _shopPrice;
        public int RequiredReputationForSaleInShop => _requiredReputationForBuyInShop;

        #endregion
    }
}
