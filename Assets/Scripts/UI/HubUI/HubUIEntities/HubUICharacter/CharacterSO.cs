using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "Character", menuName = "CreateData/HubUIData/Character", order = 0)]
    public class CharacterSO : ScriptableObject
    {
        #region Fields

        [SerializeField] private CharacterData _characterStruct;

        #endregion


        #region Properties

        public CharacterData CharacterStruct => _characterStruct;

        #endregion


        #region ClassLifeCycle

        public CharacterSO(CharacterData characterStruct)
        {
            _characterStruct = characterStruct;
        }

        #endregion


        #region Methods

        public void SetDefaultModuleParts(CharacterClothesModuleParts[] defaultModuleParts)
        {
            _characterStruct.DefaultModuleParts = defaultModuleParts;
            Debug.Log($"Set default module parts for character {CharacterStruct.Name} completed");
        }

        public void SetDefaultHeadParts(CharacterHeadPart[] defaultHeadParts)
        {
            _characterStruct.DefaultHeadParts = defaultHeadParts;
            Debug.Log($"Set default head parts for character {CharacterStruct.Name} completed");
        }

        public void SetDefaultMaterial(Material material)
        {
            if(_characterStruct.DefaultMaterial != null)
            {
                _characterStruct.DefaultMaterial.shader = material.shader;
                _characterStruct.DefaultMaterial.CopyPropertiesFromMaterial(material);
                Debug.Log($"Set default material for character {CharacterStruct.Name} completed");
            }
            else
            {
                Debug.LogError("First need to create new material for the character and drag it to character data!");
            }
        }

        #endregion
    }
}
