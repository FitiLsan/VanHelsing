using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class LocationData
    {
        #region Fields

        [SerializeField] private MapObjectData _mapObjectData;
        [SerializeField] private int _loadSceneId;
        [SerializeField] private Sprite _screenshot;
        [SerializeField] private int _baseTravelTime;
        [SerializeField] private DwellerSO[] _dwellers;
        [SerializeField] private IngredientItemSO[] _ingredients;

        #endregion


        #region Properties

        public MapObjectData MapObjectData => _mapObjectData;
        public int LoadSceneId => _loadSceneId;
        public Sprite Screenshot => _screenshot;
        public int BaseTravelTime => _baseTravelTime;
        public DwellerSO[] Dwellers => (DwellerSO[])_dwellers?.Clone();
        public IngredientItemSO[] Ingredients => (IngredientItemSO[])_ingredients?.Clone();

        #endregion
    }
}
