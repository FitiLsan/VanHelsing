using UnityEngine;


namespace BeastHunter
{
    public class HubMapUILocationModel : HubMapUIMapObjectModel
    {
        #region Properties

        public int LoadSceneId { get; private set; }
        public Sprite Screenshot { get; private set;}
        public int TravelTime { get; private set; }
        public HubMapUIDwellerData[] Dwellers { get; private set; }
        public HubMapUIIngredientData[] Ingredients { get; private set; }

        #endregion


        public HubMapUILocationModel(HubMapUIMapObjectData mapObjectData) : base(mapObjectData)
        {
            HubMapUILocationData locationData = mapObjectData as HubMapUILocationData;

            LoadSceneId = locationData.LoadSceneId;
            Screenshot = locationData.Screenshot;
            TravelTime = locationData.TravelTime;
            Dwellers = locationData.Dwellers;
            Ingredients = locationData.Ingredients;
        }
    }
}
