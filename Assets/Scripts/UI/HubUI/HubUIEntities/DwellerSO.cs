using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "Dweller", menuName = "CreateData/HubUIData/Dweller", order = 0)]
    public class DwellerSO : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;

        #endregion


        #region Properties

        public string Name => _name;

        #endregion
    }
}
