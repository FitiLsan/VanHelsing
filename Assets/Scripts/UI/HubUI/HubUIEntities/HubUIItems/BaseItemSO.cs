using UnityEngine;


namespace BeastHunterHubUI
{
    public abstract class BaseItemSO : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _shopPrice;
        [SerializeField] private int _requiredReputationForBuyInShop;
        [SerializeField] private int _rank;

        #endregion


        #region Properties

        public abstract ItemType ItemType { get; }
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public int ShopPrice => _shopPrice;
        public int RequiredReputationForSaleInShop => _requiredReputationForBuyInShop;
        public int Rank => _rank;

        #endregion
    }
}
