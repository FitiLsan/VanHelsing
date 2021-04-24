using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "CreateData/HubUIData/Ingredient", order = 0)]
    public class IngredientData : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;

        #endregion


        #region Properties

        public string Name => _name;

        #endregion
    }
}