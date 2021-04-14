using UnityEngine;


namespace BeastHunter
{
    [System.Serializable]
    public class HubMapUICityReputation
    {
        #region Fields

        [SerializeField] public HubMapUICityData _city;
        [SerializeField] public int _reputation;

        #endregion


        #region Properties

        public HubMapUICityData City => _city;
        public int Reputation => _reputation;

        #endregion
    }
}
