using Extensions;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class HubMapUICharacterModel
    {
        #region Fields

        private GameObject _view3DModelPrefab;
        private RuntimeAnimatorController _view3DModelAnimatorController;
        private Material _defaultCharacterMaterial;

        private Dictionary<HubMapUICharacterHeadParts, (string name, bool isActiveByDefault)> _defaultHeadParts;
        private Dictionary<HubMapUIClothesType, List<string>> _defaultModuleParts;

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
        public HubMapUIItemStorage Backpack { get; private set; }
        public HubMapUIClothesEquipmentStorage ClothesEquipment { get; private set; }
        public HubMapUIPocketsStorage Pockets { get; private set; }

        //todo: Pockets and Weapon storages
        //public HubMapUIEquipmentModel Pockets { get; private set; }
        //public HubMapUIEquipmentModel Weapon { get; private set; }

        public HubMapUICharacterBehaviour Behaviour { get; set; }

        #endregion


        #region ClassLifeCycle

        public HubMapUICharacterModel(HubMapUICharacterData data, int backpackSize, HubMapUIClothesType[] clothEquipment)
        {
            Name = data.Name;
            Portrait = data.Portrait;
            IsFemale = data.IsFemale;
            _view3DModelPrefab = data.View3DModelPrefab;
            _view3DModelAnimatorController = data.View3DModelAnimatorController;
            _defaultCharacterMaterial = data.DefaultMaterial;

            Backpack = new HubMapUIItemStorage(backpackSize, HubMapUIItemStorageType.CharacterInventory);
            for (int i = 0; i < data.StartBackpuckItems.Length; i++)
            {
                HubMapUIBaseItemModel itemModel = HubMapUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(data.StartBackpuckItems[i]);
                Backpack.PutItem(i, itemModel);
            }

            Pockets = new HubMapUIPocketsStorage();

            ClothesEquipment = new HubMapUIClothesEquipmentStorage(clothEquipment, HubMapUIItemStorageType.CharacterClothEquipment);
            ClothesEquipment.IsEnoughEmptyPocketsFunc = Pockets.IsEnoughFreeSlots;

            ClothesEquipment.OnTakeItemFromSlotHandler += OnTakeClothesEquipmentItem;
            ClothesEquipment.OnPutItemToSlotHandler += OnPutClothesEquipmentItem;

            for (int i = 0; i < data.StartEquipmentItems.Length; i++)
            {
                HubMapUIBaseItemModel clothes = HubMapUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(data.StartEquipmentItems[i]);
                ClothesEquipment.PutItemToFirstEmptySlot(clothes);
            }

            InitializeDefaultHeadPartsDictionary(data.DefaultHeadParts);
            InitializeDefaultModulePartsDictionary(data.DefaultModuleParts);
        }

        #endregion


        #region Methods

        public void InitializeView3DModel(Transform parent)
        {
            View3DModelObjectOnScene = GameObject.Instantiate(_view3DModelPrefab, parent);
            View3DModelObjectOnScene.GetComponent<Animator>().runtimeAnimatorController = _view3DModelAnimatorController;

            foreach (KeyValuePair<HubMapUIClothesType, List<string>> kvp in _defaultModuleParts)
            {
                SetActiveToDefaultClothesByType(true, kvp.Key);
            }

            foreach (KeyValuePair<HubMapUICharacterHeadParts, (string name, bool isActiveByDefault)> kvp in _defaultHeadParts)
            {
                SetMaterialToModulePart(FindModulePartByName(kvp.Value.name), _defaultCharacterMaterial);
            }
            SetActiveToHeadPartsByDefault();

            for (int i = 0; i < ClothesEquipment.GetSlotsCount(); i++)
            {
                if (ClothesEquipment.GetItemBySlot(i) != null)
                {
                    PutOnClothes(ClothesEquipment.GetItemBySlot(i) as HubMapUIClothesItemModel);
                }
            }

            View3DModelObjectOnScene.SetActive(false);
        }

        private void InitializeDefaultHeadPartsDictionary(HubMapUICharacterHeadPart[] characterHeadParts)
        {
            _defaultHeadParts = new Dictionary<HubMapUICharacterHeadParts, (string, bool)>();

            for (int i = 0; i < characterHeadParts.Length; i++)
            {
                HubMapUICharacterHeadPart headPart = characterHeadParts[i];
                if (!_defaultHeadParts.ContainsKey(headPart.Type))
                {
                    _defaultHeadParts.Add(headPart.Type, (headPart.Name, headPart.IsActivateByDefault));
                }
            }
        }

        private void InitializeDefaultModulePartsDictionary(HubMapUICharacterClothesModuleParts[] clothesModuleParts)
        {
            _defaultModuleParts = new Dictionary<HubMapUIClothesType, List<string>>();

            for (int i = 0; i < clothesModuleParts.Length; i++)
            {
                HubMapUICharacterClothesModuleParts clothesParts = clothesModuleParts[i];
                
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

        private void OnTakeClothesEquipmentItem(HubMapUIItemStorageType storageType, int slotIndex, HubMapUIBaseItemModel item)
        {
            HubMapUIClothesItemModel clothes = item as HubMapUIClothesItemModel;

            if (clothes != null)
            {
                Pockets.RemovePockets(clothes.PocketsAmount);

                if (View3DModelObjectOnScene != null)
                {
                    TakeOffClothes(clothes);
                }
            }
        }

        private void OnPutClothesEquipmentItem(HubMapUIItemStorageType storageType, int slotIndex, HubMapUIBaseItemModel item)
        {
            HubMapUIClothesItemModel clothes = item as HubMapUIClothesItemModel;
            
            if (clothes != null)
            {
                Pockets.AddPockets(clothes.PocketsAmount);

                if (View3DModelObjectOnScene != null)
                {
                    PutOnClothes(clothes);
                }
            }
        }

        private void TakeOffClothes(HubMapUIClothesItemModel clothes)
        {
            HubMapUIClothesType clothesType = clothes.ClothesType;

            SetActiveToDefaultClothesByType(true, clothesType);

            if (clothesType == HubMapUIClothesType.Head)
            {
                SetActiveToHeadPartsByDefault();
            }

            SetActiveClothesModuleParts(false, clothes);
        }

        private void PutOnClothes(HubMapUIClothesItemModel clothes)
        {
            HubMapUIClothesType clothesType = clothes.ClothesType;

            SetActiveClothesModuleParts(true, clothes);

            if (clothesType == HubMapUIClothesType.Head)
            {
                foreach (KeyValuePair<HubMapUICharacterHeadParts, (string name, bool)> kvp in _defaultHeadParts)
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

        private void SetActiveClothesModuleParts(bool flag, HubMapUIClothesItemModel clothes)
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
            foreach (KeyValuePair<HubMapUICharacterHeadParts, (string name, bool isActiveByDefault)> kvp in _defaultHeadParts)
            {
                FindModulePartByName(kvp.Value.name).SetActive(kvp.Value.isActiveByDefault);
            }
        }

        private void SetActiveToDefaultClothesByType(bool flag, HubMapUIClothesType clothesType)
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
