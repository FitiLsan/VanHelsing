using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "Fraction", menuName = "CreateData/HubUIData/Fraction", order = 0)]
    public class FractionSO: ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;
        [SerializeField] private Sprite _logo;

        #endregion


        #region Properties

        public string Name => _name;
        public Sprite Logo => _logo;

        #endregion
    }
}
