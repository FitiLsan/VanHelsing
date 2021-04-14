using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HubMapUIDweller", menuName = "CreateData/HubMapUIData/HubMapUIDweller", order = 0)]
    public class HubMapUIDwellerData : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;

        #endregion


        #region Properties

        public string Name => _name;

        #endregion
    }
}
