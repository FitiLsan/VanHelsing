using Extensions;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class CharacterModel
    {
        #region Fields

        private GameObject _view3DModelPrefab;
        private RuntimeAnimatorController _view3DModelAnimatorController;
        private Material _defaultCharacterMaterial;

        private Dictionary<CharacterHeadPartType, (string name, bool isActiveByDefault)> _defaultHeadParts;
        private Dictionary<ClothesType, List<string>> _defaultModuleParts;

        //todo for optimization?
        //private Dictionary<HubMapUICharacterHeadParts, GameObject> _defaultHeadParts;
        //private Dictionary<HubMapUIClothesType, List<GameObject>> _defaultModuleParts;
        //private Dictionary<HubMapUIClothesType, List<GameObject>> _clothesModuleParts;

        #endregion


        #region Properties

        public string Name { get; private set; }
        public Sprite Portrait { get; private set; }
        public bool IsFemale { get; private set; }
        public GameObject View3DModelObjectOnScene { get; private set; }
        public ItemStorage Backpack { get; private set; }
        public EquippedClothesStorage ClothesEquipment { get; private set; }
        public PocketsStorage Pockets { get; private set; }
        public EquippedWeaponStorage WeaponEquipment { get; private set; }
        public bool IsHaveOrder { get; set; }
        public MapCharacterBehaviour Behaviour { get; set; }

        #endregion


        #region ClassLifeCycle

        public CharacterModel(CharacterData data, CharactersSettingsStruct settings)
        {
            Name = data.Name;
            Portrait = data.Portrait;
            IsFemale = data.IsFemale;
            _view3DModelPrefab = data.View3DModelPrefab;
            _view3DModelAnimatorController = data.View3DModelAnimatorController;
            _defaultCharacterMaterial = data.DefaultMaterial;

            Backpack = new ItemStorage(settings.BackpuckSlotAmount, ItemStorageType.CharacterBackpuck);
            if (data.StartBackpuckItems != null)
            {
                for (int i = 0; i < data.StartBackpuckItems.Length; i++)
                {
                    BaseItemModel itemModel = HubUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(data.StartBackpuckItems[i]);
                    Backpack.PutItem(i, itemModel);
                }
            }

            Pockets = new PocketsStorage();

            ClothesEquipment = new EquippedClothesStorage(settings.ClothesSlots);
            ClothesEquipment.IsEnoughEmptyPocketsFunc = Pockets.IsEnoughFreeSlots;

            ClothesEquipment.OnTakeItemFromSlotHandler += OnTakeClothesEquipmentItem;
            ClothesEquipment.OnPutItemToSlotHandler += OnPutClothesEquipmentItem;

            if (data.StartClothesEquipmentItems != null)
            {
                for (int i = 0; i < data.StartClothesEquipmentItems.Length; i++)
                {
                    BaseItemModel clothes = HubUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(data.StartClothesEquipmentItems[i]);
                    ClothesEquipment.PutItemToFirstEmptySlot(clothes);
                }
            }

            WeaponEquipment = new EquippedWeaponStorage(settings.WeaponSetsAmount);

            if (data.StartWeaponEquipmentItems != null)
            {
                for (int i = 0; i < data.StartWeaponEquipmentItems.Length; i++)
                {
                    BaseItemModel weapon = HubUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(data.StartWeaponEquipmentItems[i]);
                    WeaponEquipment.PutItemToFirstEmptySlot(weapon);
                }
            }

            InitializeDefaultHeadPartsDictionary(data.DefaultHeadParts);
            InitializeDefaultModulePartsDictionary(data.DefaultModuleParts);
        }

        #endregion


        #region Methods

        public bool EquipClothesItem(BaseItemStorage outStorage, int outStorageSlotIndex)
        {
            BaseItemModel item = outStorage.GetItemBySlot(outStorageSlotIndex);
            if (item != null && item.ItemType == ItemType.Clothes)
            {
                if (!ClothesEquipment.PutItemToFirstEmptySlot(item))
                {
                    int? firstSlot = ClothesEquipment.GetFirstSlotIndexForItem(item as ClothesItemModel);
                    ClothesEquipment.SwapItemsWithOtherStorage(firstSlot.Value, outStorage, outStorageSlotIndex);
                }
                else
                {
                    outStorage.RemoveItem(outStorageSlotIndex);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EquipWeaponItem(BaseItemStorage outStorage, int outStorageSlotIndex)
        {
            BaseItemModel item = outStorage.GetItemBySlot(outStorageSlotIndex);
            if (item != null && item.ItemType == ItemType.Weapon)
            {
                if (!WeaponEquipment.PutItemToFirstEmptySlot(item))
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("There is no free slots for this weapon");
                    return false;
                }
                else
                {
                    outStorage.RemoveItem(outStorageSlotIndex);
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
            View3DModelObjectOnScene = GameObject.Instantiate(_view3DModelPrefab, parent);
            View3DModelObjectOnScene.GetComponent<Animator>().runtimeAnimatorController = _view3DModelAnimatorController;

            foreach (KeyValuePair<ClothesType, List<string>> kvp in _defaultModuleParts)
            {
                SetActiveToDefaultClothesByType(true, kvp.Key);
            }

            foreach (KeyValuePair<CharacterHeadPartType, (string name, bool isActiveByDefault)> kvp in _defaultHeadParts)
            {
                SetMaterialToModulePart(FindModulePartByName(kvp.Value.name), _defaultCharacterMaterial);
            }
            SetActiveToHeadPartsByDefault();

            for (int i = 0; i < ClothesEquipment.GetSlotsCount(); i++)
            {
                if (ClothesEquipment.GetItemBySlot(i) != null)
                {
                    PutOnClothes(ClothesEquipment.GetItemBySlot(i) as ClothesItemModel);
                }
            }

            View3DModelObjectOnScene.SetActive(false);
        }

        private void InitializeDefaultHeadPartsDictionary(CharacterHeadPart[] characterHeadParts)
        {
            _defaultHeadParts = new Dictionary<CharacterHeadPartType, (string, bool)>();

            for (int i = 0; i < characterHeadParts.Length; i++)
            {
                CharacterHeadPart headPart = characterHeadParts[i];
                if (!_defaultHeadParts.ContainsKey(headPart.Type))
                {
                    _defaultHeadParts.Add(headPart.Type, (headPart.Name, headPart.IsActivateByDefault));
                }
            }
        }

        private void InitializeDefaultModulePartsDictionary(CharacterClothesModuleParts[] clothesModuleParts)
        {
            _defaultModuleParts = new Dictionary<ClothesType, List<string>>();

            for (int i = 0; i < clothesModuleParts.Length; i++)
            {
                CharacterClothesModuleParts clothesParts = clothesModuleParts[i];

                if (!_defaultModuleParts.ContainsKey(clothesParts.Type))
                {
                    List<string> clothesNames = new List<string>();
                    for (int j = 0; j < clothesParts.Names.Count; j++)
                    {
                        clothesNames.Add(clothesParts.Names[j]);
                    }
                    _defaultModuleParts.Add(clothesParts.Type, clothesNames);
                }
            }
        }

        private void OnTakeClothesEquipmentItem(ItemStorageType storageType, int slotIndex, BaseItemModel item)
        {
            ClothesItemModel clothes = item as ClothesItemModel;

            if (clothes != null)
            {
                Pockets.RemovePockets(clothes.PocketsAmount);

                if (View3DModelObjectOnScene != null)
                {
                    TakeOffClothes(clothes);
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
                    PutOnClothes(clothes);
                }
            }
        }

        private void TakeOffClothes(ClothesItemModel clothes)
        {
            ClothesType clothesType = clothes.ClothesType;

            SetActiveToDefaultClothesByType(true, clothesType);

            if (clothesType == ClothesType.Head)
            {
                SetActiveToHeadPartsByDefault();
            }

            SetActiveClothesModuleParts(false, clothes);
        }

        private void PutOnClothes(ClothesItemModel clothes)
        {
            ClothesType clothesType = clothes.ClothesType;

            SetActiveClothesModuleParts(true, clothes);

            if (clothesType == ClothesType.Head)
            {
                foreach (KeyValuePair<CharacterHeadPartType, (string name, bool)> kvp in _defaultHeadParts)
                {
                    FindModulePartByName(kvp.Value.name).SetActive(true);
                }

                for (int i = 0; i < clothes.DisabledHeadParts.Length; i++)
                {
                    if (_defaultHeadParts.ContainsKey(clothes.DisabledHeadParts[i]))
                    {
                        FindModulePartByName(_defaultHeadParts[clothes.DisabledHeadParts[i]].name).SetActive(false);
                    }
                }
            }

            SetActiveToDefaultClothesByType(false, clothesType);
        }

        private void SetActiveClothesModuleParts(bool flag, ClothesItemModel clothes)
        {
            List<GameObject> moduleParts = new List<GameObject>();

            moduleParts.AddRange(FindModulePartsByNames(clothes.PartsNamesAllGender));

            if (IsFemale)
            {
                moduleParts.AddRange(FindModulePartsByNames(clothes.PartsNamesFemale));
            }
            else
            {
                moduleParts.AddRange(FindModulePartsByNames(clothes.PartsNamesMale));
            }

            if (moduleParts.Count > 0)
            {
                if (flag)
                {
                    ActivatedModuleParts(moduleParts, clothes.FantasyHeroMaterial);
                }
                else
                {
                    DeactivatedModuleParts(moduleParts);
                }
            }
        }

        private void SetActiveToHeadPartsByDefault()
        {
            foreach (KeyValuePair<CharacterHeadPartType, (string name, bool isActiveByDefault)> kvp in _defaultHeadParts)
            {
                FindModulePartByName(kvp.Value.name).SetActive(kvp.Value.isActiveByDefault);
            }
        }

        private void SetActiveToDefaultClothesByType(bool flag, ClothesType clothesType)
        {
            if (_defaultModuleParts.ContainsKey(clothesType))
            {
                if (flag)
                {
                    ActivatedModuleParts(FindModulePartsByNames(_defaultModuleParts[clothesType]), _defaultCharacterMaterial);
                }
                else
                {
                    DeactivatedModuleParts(FindModulePartsByNames(_defaultModuleParts[clothesType]));
                }
            }
        }

        private void ActivatedModuleParts(List<GameObject> moduleParts, Material fantasyHeroMaterial)
        {
            for (int i = 0; i < moduleParts.Count; i++)
            {
                SetMaterialToModulePart(moduleParts[i], fantasyHeroMaterial);
                moduleParts[i].SetActive(true);
            }
        }

        private void DeactivatedModuleParts(List<GameObject> moduleParts)
        {
            for (int i = 0; i < moduleParts.Count; i++)
            {
                moduleParts[i].SetActive(false);
            }
        }

        private List<GameObject> FindModulePartsByNames(IEnumerable<string> modulePartsNames)
        {
            List<GameObject> moduleParts = new List<GameObject>();
            if (modulePartsNames != null)
            {
                foreach (string modulePartName in modulePartsNames)
                {
                    moduleParts.Add(FindModulePartByName(modulePartName));
                }
            }
            return moduleParts;
        }

        private GameObject FindModulePartByName(string modulePartName)
        {
            return View3DModelObjectOnScene.transform.FindDeep(modulePartName).gameObject;
        }

        private void SetMaterialToModulePart(GameObject modulePart, Material fantasyHeroMaterial)
        {
            modulePart.GetComponent<SkinnedMeshRenderer>().material = fantasyHeroMaterial;
        }

        #endregion
    }
}
