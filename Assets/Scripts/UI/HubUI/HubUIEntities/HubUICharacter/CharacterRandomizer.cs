using Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BeastHunterHubUI
{
    //TODO: rework character randomizer by design doc
    /// <summary>
    /// The randomizer is temporary, it needs to be reworked according to the game design document! (when will it come..)
    /// </summary>
    public class CharacterRandomizer
    {
        #region Constants

        private const int CHECK_NAME_ON_FREE_ATTEMPTS_NUMBER = 500;

        #endregion


        #region Fields

        private GameObject _modularCharactersPrefab;
        private CharactersGlobalData _charactersData;

        private List<string> _allHairModulesNames;
        private List<string> _femaleHeadModulesNames;
        private List<string> _femaleEyebrowModulesNames;
        private List<string> _maleHeadModulesNames;
        private List<string> _maleEyebrowModulesNames;
        private List<string> _maleFacialHairModulesNames;

        private List<CharacterClothesModuleParts> _femaleDefaultBodyParts;
        private List<CharacterClothesModuleParts> _maleDefaultBodyParts;

        #endregion


        #region ClassLifeCycle

        public CharacterRandomizer()
        {
            _charactersData = BeastHunter.Data.HubUIData.CharactersGlobalData;
            _modularCharactersPrefab = _charactersData.ModularCharactersPrefab;

            if (_modularCharactersPrefab != null)
            {
                _allHairModulesNames = GetModulesNamesByGroupName("All_01_Hair");
                _femaleHeadModulesNames = GetModulesNamesByGroupName("Female_Head_All_Elements");
                _femaleEyebrowModulesNames = GetModulesNamesByGroupName("Female_01_Eyebrows");
                _maleHeadModulesNames = GetModulesNamesByGroupName("Male_Head_All_Elements");
                _maleEyebrowModulesNames = GetModulesNamesByGroupName("Male_01_Eyebrows");
                _maleFacialHairModulesNames = GetModulesNamesByGroupName("Male_02_FacialHair");
            }
            else
            {
                Debug.LogError("ModularCharacters prefab is not assigned");
            }

            _femaleDefaultBodyParts = new List<CharacterClothesModuleParts>()
            {
                new CharacterClothesModuleParts(ClothesType.Torso, new List<string>(){ "Chr_Torso_Female_00" }),
                new CharacterClothesModuleParts(ClothesType.Hips, new List<string>(){ "Chr_Hips_Female_00" }),

                new CharacterClothesModuleParts(ClothesType.Shoulders, new List<string>()
                {
                    "Chr_ArmUpperRight_Female_00",
                    "Chr_ArmUpperLeft_Female_00",
                }),
                new CharacterClothesModuleParts(ClothesType.Arms, new List<string>()
                {
                    "Chr_ArmLowerRight_Female_00",
                    "Chr_ArmLowerLeft_Female_00",
                }),
                new CharacterClothesModuleParts(ClothesType.Hands, new List<string>()
                {
                    "Chr_HandRight_Female_00",
                    "Chr_HandLeft_Female_00",
                }),
                new CharacterClothesModuleParts(ClothesType.Legs, new List<string>()
                {
                    "Chr_LegRight_Female_00",
                    "Chr_LegLeft_Female_00",
                }),
            };

             _maleDefaultBodyParts = new List<CharacterClothesModuleParts>()
            {
                new CharacterClothesModuleParts(ClothesType.Torso, new List<string>(){ "Chr_Torso_Male_00" }),
                new CharacterClothesModuleParts(ClothesType.Hips, new List<string>(){ "Chr_Hips_Male_00" }),

                new CharacterClothesModuleParts(ClothesType.Shoulders, new List<string>()
                {
                    "Chr_ArmUpperRight_Male_00",
                    "Chr_ArmUpperLeft_Male_00",
                }),
                new CharacterClothesModuleParts(ClothesType.Arms, new List<string>()
                {
                    "Chr_ArmLowerRight_Male_00",
                    "Chr_ArmLowerLeft_Male_00",
                }),
                new CharacterClothesModuleParts(ClothesType.Hands, new List<string>()
                {
                    "Chr_HandRight_Male_00",
                    "Chr_HandLeft_Male_00",
                }),
                new CharacterClothesModuleParts(ClothesType.Legs, new List<string>()
                {
                    "Chr_LegRight_Male_00",
                    "Chr_LegLeft_Male_00",
                }),
            };
        }

        #endregion


        #region Methods

        public int GetRank()
        {
            //logic
            //temporary for debug
            return 0;
        }

        public List<BaseItemSO> GetRandomBackpackItems()
        {
            List<BaseItemSO> list = new List<BaseItemSO>();

            //logic

            return list;
        }

        public List<CharacterSkillLevel> GetSkills()
        {

            List<CharacterSkillLevel> list = new List<CharacterSkillLevel>();

            //temporary for debug:
            foreach (SkillType skillType in Enum.GetValues(typeof(SkillType)))
            {
                if(skillType != SkillType.None)
                {
                    list.Add(new CharacterSkillLevel(skillType, Random.Range(10, 100))); //temporary for debug!
                }
            }

            return list;
        }

        public bool IsFemale()
        {
            return Random.Range(0, 101) <= _charactersData.FemaleGenderChance * 100;
        }

        public string GetRandomNameFromPool(bool isFemale)
        {
            string name = null;

            for (int i = 0; i < CHECK_NAME_ON_FREE_ATTEMPTS_NUMBER; i++)
            {
                name = isFemale ?
                _charactersData.FemaleNamesPool[Random.Range(0, _charactersData.FemaleNamesPool.Length)] :
                _charactersData.MaleNamesPool[Random.Range(0, _charactersData.MaleNamesPool.Length)];

                if (HubUIServices.SharedInstance.CharacterCheckNameService.IsNameFree(name))
                {
                    break;
                }

                if (i == CHECK_NAME_ON_FREE_ATTEMPTS_NUMBER - 1)
                {
                    Debug.LogWarning("Need to increase the characters names pool");
                }
            }

            return name;
        }

        public Material GetRandomMaterialFromPool()
        {
            return _charactersData.FantasyHeroMaterialsPool[Random.Range(0, _charactersData.FantasyHeroMaterialsPool.Length)];
        }

        public List<ClothesItemSO> GetRandomClothes(int rank)
        {
            List<ClothesItemSO> clothes = new List<ClothesItemSO>();

            int clothesItemsAmount = Random.Range(_charactersData.MinClothesEquipItemsAmount, _charactersData.ClothesSlots.Length);

            List<ClothesType> clothesTypePool = new List<ClothesType>();
            for (int i = 0; i < _charactersData.ClothesSlots.Length; i++)
            {
                clothesTypePool.Add(_charactersData.ClothesSlots[i]);
            }

            for (int i = 0; i < clothesItemsAmount; i++)
            {
                int randomIndex = Random.Range(0, clothesTypePool.Count);
                ClothesType randomClothesType = clothesTypePool[randomIndex];
                clothesTypePool.RemoveAt(randomIndex);

                ClothesItemSO randomClothesData = BeastHunter.Data.HubUIData.ItemDataPools.GetRandomClothesSOByRankAndType(rank, randomClothesType);
                clothes.Add(randomClothesData);
            }

            return clothes;
        }

        public List<WeaponItemSO> GetRandomWeapon(int rank)
        {
            List<WeaponItemSO> weapons = new List<WeaponItemSO>();

            WeaponItemSO randomWeaponData = BeastHunter.Data.HubUIData.ItemDataPools.GetRandomWeaponSOByRank(rank);
            weapons.Add(randomWeaponData);

            return weapons;
        }

        public List<PocketItemSO> GetRandomPocketItems(int maxPockets)
        {
            List<PocketItemSO> list = new List<PocketItemSO>();

            //temporary for debug:
            if (maxPockets > 0)
            {
                int itemsAmount = Random.Range(1, Mathf.CeilToInt(maxPockets/3));
                for (int i = 0; i < itemsAmount; i++)
                {
                    PocketItemSO randomPocketData = BeastHunter.Data.HubUIData.ItemDataPools.GetRandomPocketItem();
                    list.Add(randomPocketData);
                }
            }

            return list;
        }

        public List<CharacterClothesModuleParts> GetDefaultBodyModules(bool isFemale)
        {
            if (isFemale)
            {
                return _femaleDefaultBodyParts;
            }
            else
            {
                return _maleDefaultBodyParts;
            }
        }

        public List<CharacterHeadPart> GetDefaultHeadModuleParts(bool isFemale)
        {
            List<CharacterHeadPart> headModuleParts = new List<CharacterHeadPart>();

            bool isFacialHair = !isFemale && Random.Range(0, 101) <= _charactersData.MaleFacialHairChance * 100;
            bool isHairless = Random.Range(0, 101) <= _charactersData.HairlessChance * 100;

            string randomHeadModule;
            string randomEyebrowsModule;

            if (isFemale)
            {
                randomHeadModule = _femaleHeadModulesNames[Random.Range(0, _femaleHeadModulesNames.Count)];
                randomEyebrowsModule = _femaleEyebrowModulesNames[Random.Range(0, _femaleEyebrowModulesNames.Count)];
            }
            else
            {
                randomHeadModule = _maleHeadModulesNames[Random.Range(0, _maleHeadModulesNames.Count)];
                randomEyebrowsModule = _maleEyebrowModulesNames[Random.Range(0, _maleEyebrowModulesNames.Count)];
            }

            headModuleParts.Add(new CharacterHeadPart(CharacterHeadPartType.Head, randomHeadModule));
            headModuleParts.Add(new CharacterHeadPart(CharacterHeadPartType.Eyebrows, randomEyebrowsModule));

            if (!isHairless)
            {
                string randomHairModule = _allHairModulesNames[Random.Range(0, _allHairModulesNames.Count)];
                headModuleParts.Add(new CharacterHeadPart(CharacterHeadPartType.Hair, randomHairModule));
            }

            if (isFacialHair)
            {
                string randomFacialHairModule = _maleFacialHairModulesNames[Random.Range(0, _maleFacialHairModulesNames.Count)];
                headModuleParts.Add(new CharacterHeadPart(CharacterHeadPartType.FacialHair, randomFacialHairModule));
            }

            return headModuleParts;
        }

        private List<string> GetModulesNamesByGroupName(string groupName)
        {
            List<string> modulesNames = new List<string>();

            SkinnedMeshRenderer[] skinnedMeshRenderers =
                _modularCharactersPrefab.transform.FindDeep(groupName).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                modulesNames.Add(skinnedMeshRenderers[i].gameObject.name);
            }

            return modulesNames;
        }

        #endregion
    }
}
