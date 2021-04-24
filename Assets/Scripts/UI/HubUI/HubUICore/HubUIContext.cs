using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class HubUIContext
    {
        #region Properties

        public PlayerModel Player { get; private set; }
        public List<CharacterModel> CharactersForHire { get; private set; }
        public CharactersSettingsStruct CharacterSettings { get; private set; }
        public List<CityModel> Cities { get; private set; }
        public int ShopsSlotsAmount { get; private set; }
        public List<LocationModel> Locations { get; private set; }
        public QuestData[] QuestsData { get; private set; }
        public HubUIGameTime GameTime { get; private set; }

        #endregion


        #region ClassLifeCycle

        public HubUIContext()
        {
            CharactersForHire = new List<CharacterModel>();
            Cities = new List<CityModel>();
            Locations = new List<LocationModel>();
        }

        #endregion


        #region Methods

        public void Initialize(GameContentDataStruct data)
        {
            CharacterSettings = data.CharacterSettings;
            ShopsSlotsAmount = data.ShopsSlotsAmount;

            Player = new PlayerModel(data.PlayerSettings, data.CharacterSettings);
            GameTime = new HubUIGameTime(data.TimeSettings);

            for (int i = 0; i < data.CharactersPool.Length; i++)
            {
                CharactersForHire.Add(new CharacterModel(data.CharactersPool[i], data.CharacterSettings));
            }

            for (int i = 0; i < data.Cities.Length; i++)
            {
                Cities.Add(new CityModel(data.Cities[i]));
            }

            for (int i = 0; i < data.Locations.Length; i++)
            {
                Locations.Add(new LocationModel(data.Locations[i]));
            }

            QuestsData = data.QuestsPool;
        }

        public MapObjectModel GetMapObjectModel(MapObjectData mapObjectData)
        {
            switch (mapObjectData.GetMapObjectType())
            {
                case MapObjectType.Location:
                    return GetLocation(mapObjectData as LocationData);

                case MapObjectType.City:
                    return GetCity(mapObjectData as CityData);

                default:
                    return null;
            }
        }

        public CityModel GetCity(CityData cityData)
        {
            if (cityData != null)
            {
                return Cities.Find(city => city.DataInstanceID == cityData.GetInstanceID());
            }
            else
            {
                Debug.LogError(this + ": input parameter is null");
                return null;
            }
        }

        public LocationModel GetLocation(LocationData locationData)
        {
            if (locationData != null)
            {
                return Locations.Find(location => location.DataInstanceID == locationData.GetInstanceID());
            }
            else
            {
                Debug.LogError(this + ": input parameter is null");
                return null;
            }
        }

        public CitizenModel GetCitizen(CitizenData citizenData)
        {
            if (citizenData != null)
            {
                int citizenDataInstanceID = citizenData.GetInstanceID();
                for (int cityIndex = 0; cityIndex < Cities.Count; cityIndex++)
                {
                    for (int citizenIndex = 0; citizenIndex < Cities[cityIndex].Citizens.Count; citizenIndex++)
                    {
                        if (Cities[cityIndex].Citizens[citizenIndex].DataInstanceId == citizenDataInstanceID)
                        {
                            return Cities[cityIndex].Citizens[citizenIndex];
                        }
                    }
                }
                return null;
            }
            else
            {
                Debug.LogError(this + ": input parameter is null");
                return null;
            }
        }

        #endregion
    }
}
