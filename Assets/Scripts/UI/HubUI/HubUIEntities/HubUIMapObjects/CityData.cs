using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class CityData
    {
        #region Fields

        [SerializeField] private MapObjectData _mapObjectData;
        [SerializeField] private FractionSO _fraction;
        [SerializeField] private CitizenSO[] _citizens;
        [SerializeField] private BaseItemSO[] _shopItemsPool;
        [SerializeField] private int _playerReputation;
        [SerializeField] private int _minItemsAmountInShop;

        #endregion


        #region Properties

        public MapObjectData MapObjectData => _mapObjectData;
        public FractionSO Fraction => _fraction;
        public CitizenSO[] Citizens => (CitizenSO[])_citizens?.Clone();
        public BaseItemSO[] ShopItemsPool => (BaseItemSO[])_shopItemsPool?.Clone();
        public int PlayerReputation => _playerReputation;
        public int MinItemsAmountInShop => _minItemsAmountInShop;

        #endregion
    }
}
