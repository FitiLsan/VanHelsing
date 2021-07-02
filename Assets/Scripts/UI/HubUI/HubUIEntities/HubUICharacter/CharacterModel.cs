using Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class CharacterModel
    {
        #region Fields

        private CharactersGlobalData _globalData;
        private Material _defaultCharacterMaterial;

        private Dictionary<CharacterHeadPartType, (string name, bool isActiveByDefault)> _defaultHeadPartsNames;
        private Dictionary<ClothesType, List<string>> _defaultModulePartsNames;
        private Dictionary<CharacterHeadPartType, (GameObject headPart, bool isActiveByDefault)> _defaultHeadParts;
        private Dictionary<ClothesType, List<GameObject>> _defaultModuleParts;
        private Dictionary<ClothesType, List<GameObject>> _clothesModuleParts;

        private Transform _characterModulesTransform;

        #endregion


        #region Properties

        public string Name { get; private set; }
        public Sprite Portrait { get; private set; }
        public bool IsFemale { get; private set; }
        public GameObject View3DModelObjectOnScene { get; private set; }
        public ItemSlotStorage Backpack { get; private set; }
        public ClothesSlotStorage ClothesEquipment { get; private set; }
        public PocketsSlotStorage Pockets { get; private set; }
        public WeaponSlotStorage WeaponEquipment { get; private set; }
        public bool IsAssignedToWork { get; set; }
        public MapCharacterBehaviour Behaviour { get; set; }
        public Dictionary<SkillType, int> Skills { get; private set; }
        public int Rank { get; private set; }

        #endregion


        #region ClassLifeCycle

        public CharacterModel(CharacterData characterStruct)
        {
            Rank = characterStruct.Rank;
            _globalData = BeastHunter.Data.HubUIData.CharactersGlobalData;
            Name = characterStruct.Name;
            Portrait = characterStruct.Portrait;
            IsFemale = characterStruct.IsFemale;
            _defaultCharacterMaterial = characterStruct.DefaultMaterial;

            Backpack = new ItemSlotStorage(_globalData.BackpackSlotAmount, ItemStorageType.CharacterBackpack);
            if (characterStruct.BackpackItems != null)
            {
                for (int i = 0; i < characterStruct.BackpackItems.Length; i++)
                {
                    BaseItemModel itemModel = HubUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(characterStruct.BackpackItems[i]);
                    Backpack.PutElement(i, itemModel);
                }
            }

            Pockets = new PocketsSlotStorage();

            ClothesEquipment = new ClothesSlotStorage(_globalData.ClothesSlots);
            ClothesEquipment.IsEnoughEmptyPocketsFunc = Pockets.IsEnoughFreeSlots;

            ClothesEquipment.OnTakeItemFromSlotHandler += OnTakeClothesEquipmentItem;
            ClothesEquipment.OnPutItemToSlotHandler += OnPutClothesEquipmentItem;

            if (characterStruct.ClothesEquipmentItems != null)
            {
                for (int i = 0; i < characterStruct.ClothesEquipmentItems.Length; i++)
                {
                    BaseItemModel clothes = HubUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(characterStruct.ClothesEquipmentItems[i]);
                    ClothesEquipment.PutElementToFirstEmptySlot(clothes);
                }
            }

            if (characterStruct.PocketItems != null)
            {
                for (int i = 0; i < characterStruct.PocketItems.Length; i++)
                {
                    BaseItemModel pocketItem = HubUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(characterStruct.PocketItems[i]);
                    if (!Pockets.PutElementToFirstEmptySlot(pocketItem))
                    {
                        Debug.LogError("Impossible to put item in pocket: there are more items than pockets!");
                    }
                }
            }

            WeaponEquipment = new WeaponSlotStorage(_globalData.WeaponSetsAmount);
            if (characterStruct.WeaponEquipmentItems != null)
            {
                for (int i = 0; i < characterStruct.WeaponEquipmentItems.Length; i++)
                {
                    BaseItemModel weapon = HubUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(characterStruct.WeaponEquipmentItems[i]);
                    WeaponEquipment.PutElementToFirstEmptySlot(weapon);
                }
            }

            InitializeDefaultHeadPartsDictionary(characterStruct.DefaultHeadParts);
            InitializeDefaultModulePartsDictionary(characterStruct.DefaultModuleParts);
            InitializeView3DModel(_globalData.Character3DViewModelRendering.transform);

            SkillsInitialize(characterStruct.Skills);
        }

        #endregion


        #region Methods

        public bool EquipClothesItem(BaseItemSlotStorage outStorage, int outStorageSlotIndex)
        {
            BaseItemModel item = outStorage.GetElementBySlot(outStorageSlotIndex);
            if (item != null && item.ItemType == ItemType.Clothes)
            {
                if (!ClothesEquipment.PutElementToFirstEmptySlot(item))
                {
                    int? firstSlot = ClothesEquipment.GetFirstSlotIndexForItem(item as ClothesItemModel);
                    ClothesEquipment.SwapElementsWithOtherStorage(firstSlot.Value, outStorage, outStorageSlotIndex);
                }
                else
                {
                    outStorage.RemoveElement(outStorageSlotIndex);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EquipWeaponItem(BaseItemSlotStorage outStorage, int outStorageSlotIndex)
        {
            BaseItemModel item = outStorage.GetElementBySlot(outStorageSlotIndex);
            if (item != null && item.ItemType == ItemType.Weapon)
            {
                if (!WeaponEquipment.PutElementToFirstEmptySlot(item))
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("There is no free slots for this weapon");
                    return false;
                }
                else
                {
                    outStorage.RemoveElement(outStorageSlotIndex);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

         public void InitializeView3DModel(Transform parent)
        {
            View3DModelObjectOnScene = GameObject.Instantiate(_globalData.View3DModelPrefab, parent);
            View3DModelObjectOnScene.GetComponent<Animator>().runtimeAnimatorController = _globalData.View3DModelAnimatorController;
            _characterModulesTransform = View3DModelObjectOnScene.transform.FindDeep(_globalData.ModularCharactersChildGOForModulesName);

            InitializeDefaultModules();
            Portrait = _globalData.GetCharacterPortrait();

            InitializeClothesModulePartsDic();
            for (int i = 0; i < ClothesEquipment.GetSlotsCount(); i++)
            {
                if (ClothesEquipment.GetElementBySlot(i) != null)
                {
                    PutOnClothesOnModel(ClothesEquipment.GetElementBySlot(i) as ClothesItemModel);
                }
            }

            View3DModelObjectOnScene.SetActive(false);
        }

        private void InitializeClothesModulePartsDic()
        {
            _clothesModuleParts = new Dictionary<ClothesType, List<GameObject>>();

            foreach (ClothesType clothesType in Enum.GetValues(typeof(ClothesType)))
            {
                _clothesModuleParts.Add(clothesType, new List<GameObject>());
            }

            _clothesModuleParts.Remove(ClothesType.Amulet);
            _clothesModuleParts.Remove(ClothesType.Ring);
        }

        private void OnTakeClothesEquipmentItem(ItemStorageType storageType, int slotIndex, BaseItemModel item)
        {
            ClothesItemModel clothes = item as ClothesItemModel;

            if (clothes != null)
            {
                Pockets.RemovePockets(clothes.PocketsAmount);

                if (View3DModelObjectOnScene != null)
                {
                    TakeOffClothesFromModel(clothes);
                }
            }
        }

        private void OnPutClothesEquipmentItem(ItemStorageType storageType, int slotIndex, BaseItemModel item)
        {
            ClothesItemModel clothes = item as ClothesItemModel;

            if (clothes != null)
            {
                Pockets.AddPockets(clothes.PocketsAmount);

                if (View3DModelObjectOnScene != null)
                {
                    PutOnClothesOnModel(clothes);
                }
            }
        }

        private void InitializeDefaultModules()
        {
            _defaultModuleParts = new Dictionary<ClothesType, List<GameObject>>();
            foreach (KeyValuePair<ClothesType, List<string>> kvp in _defaultModulePartsNames)
            {
                List<GameObject> defaultModules = AddModulePartsTo3DModel(_globalData.GetModulePartsByNames(kvp.Value), _defaultCharacterMaterial);
                for (int i = 0; i < defaultModules.Count; i++)
                {
                    defaultModules[i].name += "(default)";
                }
                _defaultModuleParts.Add(kvp.Key, defaultModules);
            }

            _defaultHeadParts = new Dictionary<CharacterHeadPartType, (GameObject module, bool isActiveByDefault)>();
            foreach (KeyValuePair<CharacterHeadPartType, (string name, bool isActiveByDefault)> kvp in _defaultHeadPartsNames)
            {
                GameObject defaultHeadPart = AddModulePartTo3DModel(_globalData.GetModulePartByName(kvp.Value.name), _defaultCharacterMaterial);
                defaultHeadPart.name += "(default)";
                _defaultHeadParts.Add(kvp.Key, (defaultHeadPart, kvp.Value.isActiveByDefault));
            }
            SetDefaultActiveStatusToHeadParts();
        }

        private void TakeOffClothesFromModel(ClothesItemModel clothes)
        {
            ClothesType clothesType = clothes.ClothesType;

            SetActiveStatusToDefaultClothesByType(true, clothesType);

            if (clothesType == ClothesType.Head)
            {
                SetDefaultActiveStatusToHeadParts();
            }

            RemoveClothesModulePartsFrom3DModel(clothes.ClothesType);
        }

        private void PutOnClothesOnModel(ClothesItemModel clothes)
        {
            ClothesType clothesType = clothes.ClothesType;

            AddClothesModulePartsTo3DModel(clothes);

            if (clothesType == ClothesType.Head)
            {
                foreach (KeyValuePair<CharacterHeadPartType, (GameObject headPart, bool)> kvp in _defaultHeadParts)
                {
                    kvp.Value.headPart.SetActive(true);
                }

                for (int i = 0; i < clothes.DisabledHeadParts.Length; i++)
                {
                    if (_defaultHeadParts.ContainsKey(clothes.DisabledHeadParts[i]))
                    {
                        _defaultHeadParts[clothes.DisabledHeadParts[i]].headPart.SetActive(false);
                    }
                }
            }

            SetActiveStatusToDefaultClothesByType(false, clothesType);
        }

        private void RemoveClothesModulePartsFrom3DModel(ClothesType clothesType)
        {
            if (_clothesModuleParts.ContainsKey(clothesType))
            {
                for (int i = 0; i < _clothesModuleParts[clothesType].Count; i++)
                {
                    _clothesModuleParts[clothesType][i].SetActive(false);
                    GameObject.Destroy(_clothesModuleParts[clothesType][i]);
                }
                _clothesModuleParts[clothesType].Clear();
            }
        }

        private void AddClothesModulePartsTo3DModel(ClothesItemModel clothes)
        {
            List<GameObject> moduleParts = new List<GameObject>();

            moduleParts.AddRange(_globalData.GetModulePartsByNames(clothes.PartsNamesAllGender));

            if (IsFemale)
            {
                moduleParts.AddRange(_globalData.GetModulePartsByNames(clothes.PartsNamesFemale));
            }
            else
            {
                moduleParts.AddRange(_globalData.GetModulePartsByNames(clothes.PartsNamesMale));
            }

            if (moduleParts.Count > 0)
            {
                AddClothesModulesToDictionary(clothes.ClothesType, AddModulePartsTo3DModel(moduleParts, clothes.FantasyHeroMaterial));
            }
        }

        private void AddClothesModulesToDictionary(ClothesType clothesType, List<GameObject> clothesModules)
        {
            if (_clothesModuleParts.ContainsKey(clothesType))
            {
                _clothesModuleParts[clothesType].AddRange(clothesModules);
            }
        }

        private List<GameObject> AddModulePartsTo3DModel(List<GameObject> moduleParts, Material fantasyHeroMaterial)
        {
            List<GameObject> addedModules = new List<GameObject>();
            for (int i = 0; i < moduleParts.Count; i++)
            {
                addedModules.Add(AddModulePartTo3DModel(moduleParts[i], fantasyHeroMaterial));
            }
            return addedModules;
        }

        private GameObject AddModulePartTo3DModel(GameObject modulePart, Material fantasyHeroMaterial)
        {
            GameObject addedModule = GameObject.Instantiate(modulePart, _characterModulesTransform);
            _globalData.BindModuleToCharacter(addedModule, View3DModelObjectOnScene);
            addedModule.GetComponent<SkinnedMeshRenderer>().material = fantasyHeroMaterial;
            addedModule.SetActive(true);
            return addedModule;
        }

        private void SetDefaultActiveStatusToHeadParts()
        {
            foreach (KeyValuePair<CharacterHeadPartType, (GameObject headPart, bool isActiveByDefault)> kvp in _defaultHeadParts)
            {
                kvp.Value.headPart.SetActive(kvp.Value.isActiveByDefault);
            }
        }

        private void SetActiveStatusToDefaultClothesByType(bool flag, ClothesType clothesType)
        {
            if (_defaultModuleParts.ContainsKey(clothesType))
            {
                for (int i = 0; i < _defaultModuleParts[clothesType].Count; i++)
                {
                    _defaultModuleParts[clothesType][i].SetActive(flag);
                }
            }
        }

        private void InitializeDefaultHeadPartsDictionary(IEnumerable<CharacterHeadPart> characterHeadParts)
        {
            _defaultHeadPartsNames = new Dictionary<CharacterHeadPartType, (string, bool)>();

            foreach (CharacterHeadPart headPart in characterHeadParts)
            {
                if (!_defaultHeadPartsNames.ContainsKey(headPart.Type))
                {
                    _defaultHeadPartsNames.Add(headPart.Type, (headPart.Name, headPart.IsActivateByDefault));
                }
            }
        }

        private void InitializeDefaultModulePartsDictionary(IEnumerable<CharacterClothesModuleParts> clothesModuleParts)
        {
            _defaultModulePartsNames = new Dictionary<ClothesType, List<string>>();

            foreach (CharacterClothesModuleParts clothesParts in clothesModuleParts)
            {
                if (!_defaultModulePartsNames.ContainsKey(clothesParts.Type))
                {
                    List<string> clothesNames = new List<string>();
                    for (int j = 0; j < clothesParts.Names.Count; j++)
                    {
                        clothesNames.Add(clothesParts.Names[j]);
                    }
                    _defaultModulePartsNames.Add(clothesParts.Type, clothesNames);
                }
            }
        }

        private void SkillsInitialize(IEnumerable<CharacterSkillLevel> skills)
        {
            Skills = new Dictionary<SkillType, int>();
            foreach (SkillType skillType in Enum.GetValues(typeof(SkillType)))
            {
                Skills.Add(skillType, 0);
            }

            foreach (CharacterSkillLevel skill in skills)
            {
                Skills[skill.SkillType] = skill.SkillLevel;
            }

            Skills.Remove(SkillType.None);
        }

        #endregion
    }
}
