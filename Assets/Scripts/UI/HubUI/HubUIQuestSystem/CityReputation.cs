using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class CityReputation
    {
        #region Fields

        [SerializeField] public CitySO _city;
        [SerializeField] public int _reputation;

        #endregion
        

        #region Properties

        public CitySO City => _city;
        public int Reputation => _reputation;

        #endregion
    }
}
