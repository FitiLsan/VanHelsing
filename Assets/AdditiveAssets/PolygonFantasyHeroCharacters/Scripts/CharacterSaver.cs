using Extensions;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    //todo?:
    //the ContextMenu method to quickly select a male or female character
    //the ContextMenu method for randomization of one head only
    class CharacterSaver : MonoBehaviour
    {
        #region Fields

        [Tooltip("Put the character SO, in which you want to save the form of the character on the scene")]
        [SerializeField] CharacterData _characterData;
        [SerializeField] private Material _fantasyHeroMaterial;

        private GameObject _modularCharacters;
        private List<CharacterHeadPart> _savedHeadParts;
        private List<CharacterClothesModuleParts> _savedModuleParts;
        private Dictionary<ClothesType, List<string>> _modulePartsGroups;
        private Dictionary<CharacterHeadPartType, List<string>> _headPartsGroups;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _modularCharacters = GameObject.Find("ModularCharacters");
            _savedHeadParts = new List<CharacterHeadPart>();
            _savedModuleParts = new List<CharacterClothesModuleParts>();
            InitializeHeadPartsTypesDictionary();
            InitializeModulePartsTypesDictionary();
        }

        #endregion


        #region Methods

        [ContextMenu("SaveCharacterMaterial")]
        public void SaveCharacterMaterial()
        {
            _characterData.SetDefaultMaterial(_fantasyHeroMaterial);
        }

        [ContextMenu("SaveCharacterModulesInCharacterData")]
        public void SaveCharacterModulesInCharacterData()
        {
            SaveCharacterActiveModulePartsInList();

            CharacterClothesModuleParts[] defaultModulesParts = new CharacterClothesModuleParts[_savedModuleParts.Count];
            for (int i = 0; i < _savedModuleParts.Count; i++)
            {
                defaultModulesParts[i] = _savedModuleParts[i];
            }

            _characterData.SetDefaultModuleParts(defaultModulesParts);
            _savedModuleParts.Clear();
        }

        [ContextMenu("SaveCharacterHeadPartsInCharacterData")]
        public void SaveCharacterHeadPartsInCharacterData()
        {
            SaveCharacterActiveHeadPartsInList();

            CharacterHeadPart[] defaultHeadParts = new CharacterHeadPart[_savedHeadParts.Count];
            for (int i = 0; i < _savedHeadParts.Count; i++)
            {
                defaultHeadParts[i] = _savedHeadParts[i];
            }

            _characterData.SetDefaultHeadParts(defaultHeadParts);
            _savedHeadParts.Clear();
        }

        private void SaveCharacterActiveModulePartsInList()
        {
            foreach(KeyValuePair<ClothesType, List<string>> kvp in _modulePartsGroups)
            {
                CharacterClothesModuleParts activeModuleParts = GetActiveModulePartsByClothesType(kvp.Key, kvp.Value);
                if(activeModuleParts != null)
                {
                    _savedModuleParts.Add(activeModuleParts);
                }
            }
        }

        private void SaveCharacterActiveHeadPartsInList()
        {
            foreach (KeyValuePair<CharacterHeadPartType, List<string>> kvp in _headPartsGroups)
            {
                CharacterHeadPart activeHeadPart = GetActiveHeadPartsByHeadType(kvp.Key, kvp.Value);
                if (activeHeadPart != null)
                {
                    _savedHeadParts.Add(activeHeadPart);
                }
            }
        }

        private CharacterClothesModuleParts GetActiveModulePartsByClothesType(ClothesType clothesType, List<string> moduleGroups)
        {
            List<string> modulePartsNames = new List<string>();

            for (int i = 0; i < moduleGroups.Count; i++)
            {
                GameObject moduleGroup = _modularCharacters.transform.FindDeep(moduleGroups[i]).gameObject;
                if (moduleGroup.activeSelf)
                {
                    SkinnedMeshRenderer activeModulePart = moduleGroup.GetComponentInChildren<SkinnedMeshRenderer>(false);
                    if (activeModulePart != null)
                    {
                        modulePartsNames.Add(activeModulePart.gameObject.name);
                    }
                }
            }

            if (modulePartsNames.Count > 0)
            {
                return new CharacterClothesModuleParts(clothesType, modulePartsNames);
            }
            else
            {
                return null;
            }
        }

        private CharacterHeadPart GetActiveHeadPartsByHeadType(CharacterHeadPartType headPartType, List<string> headPartGroups)
        {
            string headPartsName = null;

            for (int i = 0; i < headPartGroups.Count; i++)
            {
                GameObject headGroup = _modularCharacters.transform.FindDeep(headPartGroups[i]).gameObject;
                if (headGroup.activeSelf)
                {
                    SkinnedMeshRenderer activeHeadPart = headGroup.GetComponentInChildren<SkinnedMeshRenderer>(false);

                    if (activeHeadPart != null)
                    {
                        headPartsName = activeHeadPart.gameObject.name;
                    }
                }
            }

            if (headPartsName != null)
            {
                return new CharacterHeadPart(headPartType, headPartsName);
            }
            else
            {
                return null;
            }
        }

        private void InitializeHeadPartsTypesDictionary()
        {
            _headPartsGroups = new Dictionary<CharacterHeadPartType, List<string>>();

            List<string> eyebrowsList = new List<string>()
            {
                "Male_01_Eyebrows",
                "Female_01_Eyebrows",
            };
            _headPartsGroups.Add(CharacterHeadPartType.Eyebrows, eyebrowsList);

            List<string> facialHairList = new List<string>()
            {
                "Male_02_FacialHair",
            };
            _headPartsGroups.Add(CharacterHeadPartType.FacialHair, facialHairList);

            List<string> hairList = new List<string>()
            {
                "All_01_Hair",
            };
            _headPartsGroups.Add(CharacterHeadPartType.Hair, hairList);

            List<string> headList = new List<string>()
            {
                "Male_Head_All_Elements",
                "Female_Head_All_Elements",
            };
            _headPartsGroups.Add(CharacterHeadPartType.Head, headList);

            List<string> earsList = new List<string>()
            {
                "Elf_Ear",
            };
            _headPartsGroups.Add(CharacterHeadPartType.Ears, earsList);
        }

        private void InitializeModulePartsTypesDictionary()
        {
            _modulePartsGroups = new Dictionary<ClothesType, List<string>>();

            List<string> backList = new List<string>()
            {
                "All_04_Back_Attachment",

            };
            _modulePartsGroups.Add(ClothesType.Back, backList);

            List<string> headList = new List<string>()
            {
                "Helmet",
                "Female_Head_No_Elements",
                "Male_Head_No_Elements",
                "HeadCoverings_No_Hair",
                "HeadCoverings_No_FacialHair",
                "HeadCoverings_Base_Hair",
            };
            _modulePartsGroups.Add(ClothesType.Head, headList);

            List<string> torsoList = new List<string>()
            {
                "Male_03_Torso",
                "Female_03_Torso",
            };
            _modulePartsGroups.Add(ClothesType.Torso, torsoList);

            List<string> hipsList = new List<string>()
            {
                "Male_10_Hips",
                "Female_10_Hips",
                "All_10_Knee_Attachement_Right",
                "All_11_Knee_Attachement_Left",
            };
            _modulePartsGroups.Add(ClothesType.Hips, hipsList);

            List<string> legsList = new List<string>()
            {
                "Male_11_Leg_Right",
                "Female_11_Leg_Right",
                "Male_12_Leg_Left",
                "Female_12_Leg_Left",
            };
            _modulePartsGroups.Add(ClothesType.Legs, legsList);

            List<string> shouldersList = new List<string>()
            {
                "All_05_Shoulder_Attachment_Right",
                "All_06_Shoulder_Attachment_Left",
                "Male_04_Arm_Upper_Right",
                "Female_04_Arm_Upper_Right",
                "Male_05_Arm_Upper_Left",
                "Female_05_Arm_Upper_Left",
            };
            _modulePartsGroups.Add(ClothesType.Shoulders, shouldersList);

            List<string> armsList = new List<string>()
            {
                "Male_06_Arm_Lower_Right",
                "Female_06_Arm_Lower_Right",
                "Male_07_Arm_Lower_Left",
                "Female_07_Arm_Lower_Left",
            };
            _modulePartsGroups.Add(ClothesType.Arms, armsList);

            List<string> handsList = new List<string>()
            {
                "Male_08_Hand_Right",
                "Female_08_Hand_Right",
                "Male_09_Hand_Left",
                "Female_09_Hand_Left",
            };
            _modulePartsGroups.Add(ClothesType.Hands, handsList);

            List<string> beltList = new List<string>()
            {
                "All_09_Hips_Attachment",
            };
            _modulePartsGroups.Add(ClothesType.Belt, beltList);
        }

        #endregion


        ////WIP
        //Dictionary<string, Transform> _bonesMap;
        //GameObject newCharacter;
        //[ContextMenu("Create character copy on scene (WIP)")]
        //public void CreateCharacterCopyOnScene()
        //{
        //    newCharacter = GameObject.Instantiate(_modularCharacters, null, false);
        //    //Destroy(newCharacter.GetComponent<CharacterRandomizer>());

        //    SkinnedMeshRenderer[] moduleParts = newCharacter.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        //    _bonesMap = new Dictionary<string, Transform>();
        //    for (int i = 0; i < moduleParts.Length; i++)
        //    {
        //        for (int j = 0; j < moduleParts[i].bones.Length; j++)
        //        {
        //            if (!_bonesMap.ContainsKey(moduleParts[i].bones[j].name))
        //            {
        //                _bonesMap.Add(moduleParts[i].bones[j].name, moduleParts[i].bones[j]);
        //            }
        //        }
        //    }

        //    for (int i = 0; i < moduleParts.Length; i++)
        //    {
        //        if (!moduleParts[i].gameObject.activeSelf)
        //        {
        //            Destroy(moduleParts[i].gameObject);
        //        }
        //    }
        //}
    }
}
