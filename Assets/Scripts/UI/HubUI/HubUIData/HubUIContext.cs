using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class HubUIContext
    {
        #region Properties

        public PlayerModel Player { get; private set; }
        public List<CharacterModel> CharactersForHire { get; private set; }
        public List<LocationModel> Locations { get; private set; }
        public List<CityModel> Cities { get; private set; }
        public GameTimeModel GameTime { get; private set; }
        public List<WorkRoomModel> WorkRooms { get; private set; }

        #endregion


        #region ClassLifeCycle

        public HubUIContext()
        {
            CharactersForHire = new List<CharacterModel>();
            Cities = new List<CityModel>();
            Locations = new List<LocationModel>();
            WorkRooms = new List<WorkRoomModel>();
        }

        #endregion


        #region Methods

        public void InitializeGameContent(GameData gameData)
        {
            HubUIData hubUIData = BeastHunter.Data.HubUIData;

            Player = new PlayerModel(gameData.PlayerData);
            GameTime = new GameTimeModel(gameData.CurrentGameTime);

            CharactersForHire = HubUIServices.SharedInstance.RandomCharacterService.Get(hubUIData.CharactersGlobalData.AmountForHire);

            for (int i = 0; i < CharactersForHire.Count; i++)
            {
                Player.HireCharacter(CharactersForHire[i]); //todo: remove after realize characters hire in UI! (for debug only)
            }

            for (int i = 0; i < gameData.CitiesSO.Length; i++)
            {
                Cities.Add(new CityModel(gameData.CitiesSO[i].CityData));
            }

            for (int i = 0; i < gameData.LocationsSO.Length; i++)
            {
                Locations.Add(new LocationModel(gameData.LocationsSO[i].LocationData));
            }

            for (int i = 0; i < gameData.WorkRoomsSO.Length; i++)
            {
                WorkRooms.Add(new WorkRoomModel(gameData.WorkRoomsSO[i].WorkRoomData));
            }
        }

        public List<MapObjectModel> GetAllMapObjects()
        {
            List<MapObjectModel> list = new List<MapObjectModel>();

            for (int i = 0; i < Cities.Count; i++)
            {
                list.Add(Cities[i]);
            }

            for (int i = 0; i < Locations.Count; i++)
            {
                list.Add(Locations[i]);
            }

            return list;
        }

        public CityModel GetCityById(int instanceId)
        {
            return Cities.Find(city => city.InstanceID == instanceId);
        }

        public CitizenModel GetCitizenById(int instanceId)
        {
            for (int cityIndex = 0; cityIndex < Cities.Count; cityIndex++)
            {
                for (int citizenIndex = 0; citizenIndex < Cities[cityIndex].Citizens.Count; citizenIndex++)
                {
                    if (Cities[cityIndex].Citizens[citizenIndex].InstanceId == instanceId)
                    {
                        return Cities[cityIndex].Citizens[citizenIndex];
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
