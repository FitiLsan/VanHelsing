using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "ClothesItem", menuName = "CreateData/HubUIData/Items/Clothes", order = 0)]
    public class ClothesItemData : BaseItemData
    {
        #region Fields

        [SerializeField] private ClothesType _clothType;
        [SerializeField] private int _pocketsAmount;
        [Tooltip("Sure to use fantasy hero material shader (SyntyStudios/CustomCharacter)")]
        [SerializeField] private Material _fantasyHeroMaterial;
        [SerializeField] private string[] _clothesPartsNamesAllGender;  //todo: quick clothes saver for game designers ?
        [SerializeField] private string[] _clothesPartsNamesMale;
        [SerializeField] private string[] _clothesPartsNamesFemale;
        [SerializeField] private CharacterHeadPartType[] _disabledHeadParts;

        #endregion


        #region Properties

        public ClothesType ClothesType => _clothType;
        public int PocketsAmount => _pocketsAmount;
        public Material FantasyHeroMaterial => _fantasyHeroMaterial;
        public string[] ClothesPartsNamesAllGender => (string[])_clothesPartsNamesAllGender?.Clone();
        public string[] ClothesPartsNamesMale => (string[])_clothesPartsNamesMale?.Clone();
        public string[] ClothesPartsNamesFemale => (string[])_clothesPartsNamesFemale?.Clone();
        public CharacterHeadPartType[] DisabledHeadParts => (CharacterHeadPartType[])_disabledHeadParts?.Clone();
        public override ItemType ItemType { get; protected set; }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            ItemType = ItemType.Clothes;
        }

        #endregion
    }
}
