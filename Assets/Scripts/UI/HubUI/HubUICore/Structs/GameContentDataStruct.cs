using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public struct GameContentDataStruct
    {
        #region Fields

        [Space]
        [SerializeField] private LocationData[] _locations;
        [SerializeField] private QuestData[] _questsPool;

        [Space(10, order = 1), Header("PLAYER", order = 2)]
        public PlayerSettingsStruct PlayerSettings;

        [Space(10, order = 1), Header("CITIES", order = 2)]
        [SerializeField] private CityData[] _cities;
        [SerializeField] private int _shopsSlotsAmount;

        [Space(10, order = 1), Header("CHARACTERS", order = 2)]
        [SerializeField] private CharacterData[] _charactersPool; //TODO: instead pool planned random generator for characters
        public CharactersSettingsStruct CharacterSettings;

        [Space(10, order = 1), Header("GAME TIME", order = 2)]
        public HubUITimeSettingsStruct TimeSettings;

        #endregion


        #region Properties

        public int ShopsSlotsAmount => _shopsSlotsAmount;
        public CharacterData[] CharactersPool => (CharacterData[])_charactersPool.Clone();
        public CityData[] Cities => (CityData[])_cities.Clone();
        public LocationData[] Locations => (LocationData[])_locations.Clone();
        public QuestData[] QuestsPool => (QuestData[])_questsPool.Clone();

        #endregion
    }
}
