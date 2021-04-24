using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "City", menuName = "CreateData/HubUIData/City", order = 0)]
    public class CityData : MapObjectData
    {
        #region Fields

        [Header("City data")]
        [SerializeField] private FractionData _fraction;
        [SerializeField] private CitizenData[] _citizens;
        [SerializeField] private BaseItemData[] _shopItemsPool;
        [SerializeField] private int _startReputation;
        [SerializeField] private int _shopSlotAmount;
        [SerializeField] private int _minItemsAmountInShop;

        #endregion


        #region Properties

        public FractionData Fraction => _fraction;
        public CitizenData[] Citizens => _citizens;
        public BaseItemData[] ShopItemsPool => _shopItemsPool;
        public int StartReputation => _startReputation;
        public int ShopSlotAmount => _shopSlotAmount;
        public int MinItemsAmountInShop => _minItemsAmountInShop;

        #endregion


        #region HubMapUIMapObjectData

        public override MapObjectType GetMapObjectType()
        {
            return MapObjectType.City;
        }

        #endregion
    }
}
