using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HubMapUICity", menuName = "CreateData/HubMapUIData/HubMapUICity", order = 0)]
    public class HubMapUICityData : HubMapUIMapObjectData
    {
        #region Fields

        [Header("City data")]
        [SerializeField] private HubMapUIFractionData _fraction;
        [SerializeField] private HubMapUICitizenData[] _citizens;
        [SerializeField] private HubMapUIBaseItemData[] _shopItemsPool;
        [SerializeField] private int _startReputation;
        [SerializeField] private int _shopSlotAmount;
        [SerializeField] private int _minItemsAmountInShop;

        #endregion


        #region Properties

        public HubMapUIFractionData Fraction => _fraction;
        public HubMapUICitizenData[] Citizens => _citizens;
        public HubMapUIBaseItemData[] ShopItemsPool => _shopItemsPool;
        public int StartReputation => _startReputation;
        public int ShopSlotAmount => _shopSlotAmount;
        public int MinItemsAmountInShop => _minItemsAmountInShop;

        #endregion


        #region HubMapUIMapObjectData

        public override HubMapUIMapObjectType GetMapObjectType()
        {
            return HubMapUIMapObjectType.City;
        }

        #endregion
    }
}
