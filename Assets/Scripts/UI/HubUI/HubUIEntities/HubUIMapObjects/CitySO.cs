using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "City", menuName = "CreateData/HubUIData/City", order = 0)]
    public class CitySO : ScriptableObject
    {
        #region Fields

        [SerializeField] private CityData _cityData;

        #endregion


        #region Properties

        public CityData CityData => _cityData;

        #endregion

        private void OnEnable()
        {
            _cityData.MapObjectData.SetInstanceId(GetInstanceID());
        }
    }
}
