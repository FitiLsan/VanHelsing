using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HubMapUICharacter", menuName = "CreateData/HubMapUIData/HubMapUICharacter", order = 0)]
    public class HubMapUICharacterData : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private bool _isFemale;
        [SerializeField] private GameObject _view3DModelPrefab;
        [SerializeField] private RuntimeAnimatorController _view3DModelAnimatorController;
        [SerializeField] private HubMapUIBaseItemData[] _startBackpuckItems;
        [SerializeField] private HubMapUIBaseItemData[] _startEquipmentItems;
        [Tooltip("Sure to use fantasy hero material shader (SyntyStudios/CustomCharacter)")]
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private HubMapUICharacterClothesModuleParts[] _defaultModuleParts;
        [SerializeField] private HubMapUICharacterHeadPart[] _defaultHeadParts;

        #endregion


        #region Properties

        public string Name => _name;
        public Sprite Portrait => _portrait;
        public bool IsFemale => _isFemale;
        public GameObject View3DModelPrefab => _view3DModelPrefab;
        public RuntimeAnimatorController View3DModelAnimatorController => _view3DModelAnimatorController;
        public HubMapUIBaseItemData[] StartBackpuckItems => (HubMapUIBaseItemData[])_startBackpuckItems.Clone();
        public HubMapUIBaseItemData[] StartEquipmentItems => (HubMapUIBaseItemData[])_startEquipmentItems.Clone();
        public Material DefaultMaterial => _defaultMaterial;
        public HubMapUICharacterClothesModuleParts[] DefaultModuleParts => (HubMapUICharacterClothesModuleParts[])_defaultModuleParts.Clone();
        public HubMapUICharacterHeadPart[] DefaultHeadParts => (HubMapUICharacterHeadPart[])_defaultHeadParts.Clone();

        #endregion


        #region Methods

        public void SetDefaultModuleParts(HubMapUICharacterClothesModuleParts[] defaultModuleParts)
        {
            _defaultModuleParts = defaultModuleParts;
            Debug.Log($"Set default module parts for character {Name} completed");
        }

        public void SetDefaultHeadParts(HubMapUICharacterHeadPart[] defaultHeadParts)
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
