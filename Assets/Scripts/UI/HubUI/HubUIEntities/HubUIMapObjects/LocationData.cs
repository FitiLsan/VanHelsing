using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "Location", menuName = "CreateData/HubUIData/Location", order = 0)]
    public class LocationData : MapObjectData
    {
        #region Fields

        [Header("Location data")]
        [SerializeField] private int _loadSceneId;
        [SerializeField] private Sprite _screenshot;
        [SerializeField] private int _travelTime;
        [SerializeField] private DwellerData[] _dwellers;
        [SerializeField] private IngredientData[] _ingredients;

        #endregion


        #region Properties

        public int LoadSceneId => _loadSceneId;
        public Sprite Screenshot => _screenshot;
        public int TravelTime => _travelTime;
        public DwellerData[] Dwellers => _dwellers;
        public IngredientData[] Ingredients => _ingredients;

        #endregion


        #region HubMapUIMapObjectData

        public override MapObjectType GetMapObjectType()
        {
            return MapObjectType.Location;
        }

        #endregion
    }
}
