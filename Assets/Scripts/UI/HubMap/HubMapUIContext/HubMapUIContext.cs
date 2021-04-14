using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class HubMapUIContext
    {
        #region Properties

        public int ShopsSlotsAmount { get; private set; }
        public int CharactersEquipmentSlotAmount { get; private set; }
        public HubMapUIClothesType[] CharactersClothEquipment { get; private set; }
        public HubMapUIPlayerModel Player { get; private set; }
        public List<HubMapUICharacterModel> Characters { get; private set; }
        public List<HubMapUICityModel> Cities { get; private set; }
        public List<HubMapUILocationModel> Locations { get; private set; }
        public HubMapUIQuestData[] QuestsData { get; private set; }

        #endregion


        #region ClassLifeCycle

        public HubMapUIContext(HubMapUIContextData data)
        {
            ShopsSlotsAmount = data.ShopsSlotsAmount;
            CharactersEquipmentSlotAmount = data.CharactersInventorySlotAmount;
            CharactersClothEquipment = data.ClothSlots;

            Player = new HubMapUIPlayerModel(data.Player);

            Characters = new List<HubMapUICharacterModel>();
            for (int i = 0; i < data.Characters.Length; i++)
            {
                Characters.Add(new HubMapUICharacterModel(data.Characters[i], CharactersEquipmentSlotAmount, data.ClothSlots));
            }

            Cities = new List<HubMapUICityModel>();
            for (int i = 0; i< data.Cities.Length; i++)
            {
                Cities.Add(new HubMapUICityModel(data.Cities[i]));
            }

            Locations = new List<HubMapUILocationModel>();
            for (int i = 0; i < data.Locations.Length; i++)
            {
                Locations.Add(new HubMapUILocationModel(data.Locations[i]));
            }

            QuestsData = data.Quests;
        }

        #endregion


        #region Methods

        public HubMapUIMapObjectModel GetMapObjectModel(HubMapUIMapObjectData mapObjectData)
        {
            switch (mapObjectData.GetMapObjectType())
            {
                case HubMapUIMapObjectType.Location:
                    return GetLocation(mapObjectData as HubMapUILocationData);

                case HubMapUIMapObjectType.City:
                    return GetCity(mapObjectData as HubMapUICityData);

                default:
                    return null;
            }
        }

        public HubMapUICityModel GetCity(HubMapUICityData cityData)
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

        public HubMapUILocationModel GetLocation(HubMapUILocationData locationData)
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

        public HubMapUICitizenModel GetCitizen(HubMapUICitizenData citizenData)
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
