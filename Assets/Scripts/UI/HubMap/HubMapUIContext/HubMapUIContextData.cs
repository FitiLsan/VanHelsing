using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HubMapUIContextData", menuName = "CreateData/HubMapUIData/HubMapUIContextData", order = 0)]
    public class HubMapUIContextData : ScriptableObject
    {
        #region Fields

        [SerializeField] private int _shopsSlotsAmount;
        [SerializeField] private HubMapUIPlayerData _player;
        [SerializeField] private int _charactersInventorySlotAmount;
        [SerializeField] private HubMapUIClothesType[] _charactersClothSlots;
        [SerializeField] private HubMapUICharacterData[] _characters;
        [SerializeField] private HubMapUICityData[] _cities;
        [SerializeField] private HubMapUILocationData[] _locations;
        [SerializeField] private HubMapUIQuestData[] _quests;

        #endregion


        #region Properties

        public int ShopsSlotsAmount => _shopsSlotsAmount;
        public HubMapUIPlayerData Player => _player;
        public int CharactersInventorySlotAmount => _charactersInventorySlotAmount;
        public HubMapUIClothesType[] ClothSlots => (HubMapUIClothesType[])_charactersClothSlots.Clone();
        public HubMapUICharacterData[] Characters => (HubMapUICharacterData[])_characters.Clone();
        public HubMapUICityData[] Cities => (HubMapUICityData[])_cities.Clone();
        public HubMapUILocationData[] Locations => (HubMapUILocationData[])_locations.Clone();
        public HubMapUIQuestData[] Quests => (HubMapUIQuestData[])_quests.Clone();

        #endregion
    }
}
