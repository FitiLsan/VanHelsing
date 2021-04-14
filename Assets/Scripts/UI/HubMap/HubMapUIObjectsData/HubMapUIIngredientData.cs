using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HubMapUIIngredient", menuName = "CreateData/HubMapUIData/HubMapUIIngredient", order = 0)]
    public class HubMapUIIngredientData : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;

        #endregion


        #region Properties

        public string Name => _name;

        #endregion
    }
}