using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "Character", menuName = "CreateData/HubUIData/Character", order = 0)]
    public class CharacterData : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private bool _isFemale;
        [SerializeField] private GameObject _view3DModelPrefab;
        [SerializeField] private RuntimeAnimatorController _view3DModelAnimatorController;
        [SerializeField] private BaseItemData[] _startBackpuckItems;
        [SerializeField] private ClothesItemData[] _startClothesEquipmentItems;
        [SerializeField] private WeaponItemData[] _startWeaponEquipmentItems;
        [Tooltip("Sure to use fantasy hero material shader (SyntyStudios/CustomCharacter)")]
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private CharacterClothesModuleParts[] _defaultModuleParts;
        [SerializeField] private CharacterHeadPart[] _defaultHeadParts;

        #endregion


        #region Properties

        public string Name => _name;
        public Sprite Portrait => _portrait;
        public bool IsFemale => _isFemale;
        public GameObject View3DModelPrefab => _view3DModelPrefab;
        public RuntimeAnimatorController View3DModelAnimatorController => _view3DModelAnimatorController;
        public BaseItemData[] StartBackpuckItems => (BaseItemData[])_startBackpuckItems?.Clone();
        public ClothesItemData[] StartClothesEquipmentItems => (ClothesItemData[])_startClothesEquipmentItems?.Clone();
        public WeaponItemData[] StartWeaponEquipmentItems => (WeaponItemData[])_startWeaponEquipmentItems?.Clone();
        public Material DefaultMaterial => _defaultMaterial;
        public CharacterClothesModuleParts[] DefaultModuleParts => (CharacterClothesModuleParts[])_defaultModuleParts?.Clone();
        public CharacterHeadPart[] DefaultHeadParts => (CharacterHeadPart[])_defaultHeadParts?.Clone();

        #endregion


        #region Methods

        public void SetDefaultModuleParts(CharacterClothesModuleParts[] defaultModuleParts)
        {
            _defaultModuleParts = defaultModuleParts;
            Debug.Log($"Set default module parts for character {Name} completed");
        }

        public void SetDefaultHeadParts(CharacterHeadPart[] defaultHeadParts)
        {
            _defaultHeadParts = defaultHeadParts;
            Debug.Log($"Set default head parts for character {Name} completed");
        }

        public void SetDefaultMaterial(Material material)
        {
            if(_defaultMaterial != null)
            {
                _defaultMaterial.shader = material.shader;
                _defaultMaterial.CopyPropertiesFromMaterial(material);
                Debug.Log($"Set default material for character {Name} completed");
            }
            else
            {
                Debug.LogError("First need to create new material for the character and drag it to character data!");
            }
        }

        #endregion
    }
}
