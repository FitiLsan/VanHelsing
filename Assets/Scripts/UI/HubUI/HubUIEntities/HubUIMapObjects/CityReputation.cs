using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class CityReputation
    {
        #region Fields

        [SerializeField] public CityData _city;
        [SerializeField] public int _reputation;

        #endregion
        

        #region Properties

        public CityData City => _city;
        public int Reputation => _reputation;

        #endregion
    }
}
