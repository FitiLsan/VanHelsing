using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "ItemRecipe", menuName = "CreateData/HubUIData/ItemRecipe", order = 0)]
    public class ItemRecipeSO : ScriptableObject
    {
        [SerializeField] private int _baseHoursNumberToComplete;
        [SerializeField] private BaseItemSO _item;
        [SerializeField] private IngredientItemSO[] _requiredIngredients; //todo: ingredients amount


        public int BaseHoursNumberToComplete => _baseHoursNumberToComplete;
        public BaseItemSO Item => _item;
        public IngredientItemSO[] RequiredIngredients => _requiredIngredients;
    }
}
