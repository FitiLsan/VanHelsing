using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HubMapUILocation", menuName = "CreateData/HubMapUIData/HubMapUILocation", order = 0)]
    public class HubMapUILocationData : HubMapUIMapObjectData
    {
        #region Fields

        [Header("Location data")]
        [SerializeField] private int _loadSceneId;
        [SerializeField] private Sprite _screenshot;
        [SerializeField] private int _travelTime;
        [SerializeField] private HubMapUIDwellerData[] _dwellers;
        [SerializeField] private HubMapUIIngredientData[] _ingredients;

        #endregion


        #region Properties

        public int LoadSceneId => _loadSceneId;
        public Sprite Screenshot => _screenshot;
        public int TravelTime => _travelTime;
        public HubMapUIDwellerData[] Dwellers => _dwellers;
        public HubMapUIIngredientData[] Ingredients => _ingredients;

        #endregion


        #region HubMapUIMapObjectData

        public override HubMapUIMapObjectType GetMapObjectType()
        {
            return HubMapUIMapObjectType.Location;
        }

        #endregion
    }
}
