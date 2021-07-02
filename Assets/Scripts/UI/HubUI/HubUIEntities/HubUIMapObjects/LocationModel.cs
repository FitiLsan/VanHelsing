using UnityEngine;


namespace BeastHunterHubUI
{
    public class LocationModel : MapObjectModel
    {
        #region Properties

        public int LoadSceneId { get; private set; }
        public Sprite Screenshot { get; private set;}
        public int BaseTravelTime { get; private set; }
        public DwellerSO[] Dwellers { get; private set; }
        public IngredientItemSO[] Ingredients { get; private set; }

        #endregion


        #region ClassLifeCycle

        public LocationModel(LocationData locationData) : base(locationData.MapObjectData)
        {
            LoadSceneId = locationData.LoadSceneId;
            Screenshot = locationData.Screenshot;
            BaseTravelTime = locationData.BaseTravelTime;
            Dwellers = locationData.Dwellers;
            Ingredients = locationData.Ingredients;
        }

        #endregion
    }
}
