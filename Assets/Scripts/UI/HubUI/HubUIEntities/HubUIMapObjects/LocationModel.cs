using UnityEngine;


namespace BeastHunterHubUI
{
    public class LocationModel : MapObjectModel
    {
        #region Properties

        public int LoadSceneId { get; private set; }
        public Sprite Screenshot { get; private set;}
        public int TravelTime { get; private set; }
        public DwellerData[] Dwellers { get; private set; }
        public IngredientData[] Ingredients { get; private set; }

        #endregion


        public LocationModel(MapObjectData mapObjectData) : base(mapObjectData)
        {
            LocationData locationData = mapObjectData as LocationData;

            LoadSceneId = locationData.LoadSceneId;
            Screenshot = locationData.Screenshot;
            TravelTime = locationData.TravelTime;
            Dwellers = locationData.Dwellers;
            Ingredients = locationData.Ingredients;
        }
    }
}
