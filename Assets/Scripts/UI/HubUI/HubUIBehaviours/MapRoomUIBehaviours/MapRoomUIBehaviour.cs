using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;


namespace BeastHunterHubUI
{
    public class MapRoomUIBehaviour : MonoBehaviour, IStart, IDestroy
    {
        #region PrivateData

        private class SelectedElements
        {
            private int? _generalInventorySlotIndex;
            private int? _shopSlotIndex;
            private int? _buyBackSlotIndex;
            private CharacterModel _character;
            private MapObjectModel _mapObject;

            public Action<int?> OnChanged_GeneralInventorySlotIndex { get; set; }
            public Action<int?> OnChanged_ShopSlotIndex { get; set; }
            public Action<int?> OnChanged_BuyBackSlotIndex { get; set; }
            public Action<CharacterModel> OnChanged_Character { get; set; }
            public Action<MapObjectModel> OnChanged_MapObject { get; set; }

            public int? GeneralInventorySlotIndex
            {
                get
                {
                    return _generalInventorySlotIndex;
                }
                set
                {
                    if (value != _generalInventorySlotIndex)
                    {
                        int? previousValue = _generalInventorySlotIndex;
                        _generalInventorySlotIndex = value;
                        OnChanged_GeneralInventorySlotIndex?.Invoke(previousValue);
                    }
                }
            }

            public int? ShopSlotIndex
            {
                get
                {
                    return _shopSlotIndex;
                }
                set
                {
                    if (value != _shopSlotIndex)
                    {
                        int? previousValue = _shopSlotIndex;
                        _shopSlotIndex = value;
                        OnChanged_ShopSlotIndex?.Invoke(previousValue);
                    }
                }
            }

            public int? BuyBackSlotIndex
            {
                get
                {
                    return _buyBackSlotIndex;
                }
                set
                {
                    if (value != _buyBackSlotIndex)
                    {
                        int? previousValue = _buyBackSlotIndex;
                        _buyBackSlotIndex = value;
                        OnChanged_BuyBackSlotIndex?.Invoke(previousValue);
                    }
                }
            }

            public CharacterModel Character
            {
                get
                {
                    return _character;
                }
                set
                {
                    if (value != _character)
                    {
                        CharacterModel previousValue = _character;
                        _character = value;
                        OnChanged_Character?.Invoke(previousValue);
                    }
                }
            }

            public MapObjectModel MapObject
            {
                get
                {
                    return _mapObject;
                }
                set
                {
                    if (value != _mapObject)
                    {
                        MapObjectModel previousValue = _mapObject;
                        _mapObject = value;
                        OnChanged_MapObject?.Invoke(previousValue);
                    }
                }
            }

            public void RemoveAllListeners()
            {
                OnChanged_GeneralInventorySlotIndex = null;
                OnChanged_ShopSlotIndex = null;
                OnChanged_BuyBackSlotIndex = null;
                OnChanged_Character = null;
                OnChanged_MapObject = null;
            }
        }

        #endregion


        #region Fields

        [Header("Map objects")]
        [SerializeField] private GameObject _mapObjectsPanel;

        [Header("Hub map")]
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private Button _hideInfoPanelButton;
        [SerializeField] private Button _hubButton;
        [SerializeField] private Button _mapButton;
        [SerializeField] private GameObject _inventoryItemsPanel;
        [SerializeField] private GameObject _tooltip;
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private Button _clockButton;

        [Header("City info panel")]
        [SerializeField] private GameObject _cityInfoPanel;
        [SerializeField] private GameObject _citizenPanel;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Image _cityFraction;
        [SerializeField] private Text _cityName;
        [SerializeField] private Text _cityDescription;
        [SerializeField] private Text _cityReputation;

        [Header("Location info panel")]
        [SerializeField] private GameObject _locationInfoPanel;
        [SerializeField] private GameObject _dwellersPanel;
        [SerializeField] private GameObject _ingredientsPanel;
        [SerializeField] private Text _locationName;
        [SerializeField] private Text _locationDescription;
        [SerializeField] private Image _locationScreen;
        [SerializeField] private Button _hikeButton;

        [Header("Dialog panel")]
        [SerializeField] private GameObject _dialogPanel;
        [SerializeField] private Image _citizenPortrait;
        [SerializeField] private Text _citizenName;
        [SerializeField] private Text _dialogText;
        [SerializeField] private GameObject _answerButtonsPanel;

        [Header("Hike panel")]
        [SerializeField] private Button _hikePanelButton;
        [SerializeField] private Button _closeHikePanelButton;
        [SerializeField] private Button _hikeAcceptButton;
        [SerializeField] private GameObject _hikePanel;
        [SerializeField] private GameObject _hikeCharactersPanel;
        [SerializeField] private GameObject _characterInventoryPanel;
        [SerializeField] private GameObject _hikeInventoryScrollView;
        [SerializeField] private Button _perkTreeButton;
        [SerializeField] private Text _travelTimeText;
        [SerializeField] private RawImage _character3DViewModelRawImage;
        [SerializeField] private GameObject _characterClothPanel;
        [SerializeField] private GameObject _pocketSlotsPanel;
        [SerializeField] private GameObject _weaponSetsPanel;

        [Header("Characters panel relocatable")]
        [SerializeField] private GameObject _charactersPanelRelocatable;
        [SerializeField] private GameObject _charactersPanelFillable;
        [SerializeField] private Scrollbar _charactersPanelScrollbar;
        [SerializeField] private Button _charactersPanelNextButton;
        [SerializeField] private Button _charactersPanelPreviousButton;

        [Header("Trade panel")]
        [SerializeField] private GameObject _tradePanel;
        [SerializeField] private GameObject _shopInventoryScrollView;
        [SerializeField] private GameObject _buyingItemsPanel;
        [SerializeField] private GameObject _buyBackItemsPanel;
        [SerializeField] private Button _closeTradePanelButton;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _buyBackButton;
        [SerializeField] private Text _sellingItemPrice;
        [SerializeField] private Text _buyingItemPrice;
        [SerializeField] private Text _buybackItemPrice;
        [SerializeField] private Text _shopCityReputation;
        [SerializeField] private Text _playerGoldAmount;

        [Header("Perks panel")]
        [SerializeField] private GameObject _perksPanel;
        [SerializeField] private Button _closePerksPanelButton;
        [SerializeField] private GameObject _perksCharactersPanel;

        [Header("Time skip panel")]
        [SerializeField] private GameObject _startTimeSkipButton;
        [SerializeField] private GameObject _stopTimeSkipButton;
        [SerializeField] private Text _currentDayText;
        [SerializeField] private Text _currentHourText;

        private SelectedElements _selected;
        private HubUIContext _context;
        private HubUIData _data;

        private List<GameObject> _rightInfoPanelObjectsForDestroy;
        private List<MapItemSlotBehaviour> _characterBackpackSlotsUIBehaviours;
        private List<MapItemSlotBehaviour> _generalInventorySlotsUIBehaviours;
        private List<MapShopItemSlotBehaviour> _shopSlotsUIBehaviours;
        private List<MapItemSlotBehaviour> _buyBackSlotsUIBehaviours;
        private MapEquipmentSlotBehaviour[] _characterClothesSlotsUIBehaviours;
        private MapEquipmentSlotBehaviour[] _characterWeaponSlotsUIBehaviours;
        private List<MapEquipmentSlotBehaviour> _characterPocketsSlotsBehaviours;
        private Dictionary<CitizenModel, MapCitizenBehaviour> _displayedCurrentCitizensUIBehaviours;
        private List<GameObject> _displayedDialogAnswerButtons;
        private (int? slotIndex, ItemStorageType storageType) _draggedItemInfo;

        private MapCharacterView3DModelBehaviour _character3DViewModelRawImageBehaviour;

        private Coroutine _showTooltipCoroutine;

        private float _charactersPanelSwipeStep;
        private float _tooltipShowingDelay;

        #endregion


        #region Properties

        public Action OnShowMapUIHandler { get; set; }
        public Action OnHideMapUIHandler { get; set; }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _hubButton.onClick.AddListener(OnClick_HubButton);
            _mapButton.onClick.AddListener(OnClick_MapButton);
            _hideInfoPanelButton.onClick.AddListener(OnClick_HideInfoPanelButton);
            _hikeAcceptButton.onClick.AddListener(OnClick_HikeAcceptButton);
            _closeHikePanelButton.onClick.AddListener(OnClick_CloseHikePanelButton);
            _hikePanelButton.onClick.AddListener(OnClick_HikePanelButton);
            _charactersPanelNextButton.onClick.AddListener(()=> OnClick_CharactersPanelNavigationButton(1));
            _charactersPanelPreviousButton.onClick.AddListener(() => OnClick_CharactersPanelNavigationButton(-1));
            _perkTreeButton.onClick.AddListener(OnClick_PerkTreeButton);
            _shopButton.onClick.AddListener(OnClick_OpenTradePanelButton);
            _closeTradePanelButton.onClick.AddListener(OnClick_CloseTradePanelButton);
            _sellButton.onClick.AddListener(OnClick_SellItemButton);
            _buyBackButton.onClick.AddListener(OnClick_BuyBackItemButton);
            _buyButton.onClick.AddListener(OnClick_BuyItemButton);
            _closePerksPanelButton.onClick.AddListener(OnClick_ClosePerksButton);

            _startTimeSkipButton.GetComponentInChildren<Button>().onClick.AddListener(OnClick_StartTimeSkipButton);
            _stopTimeSkipButton.GetComponentInChildren<Button>().onClick.AddListener(OnClick_StopTimeSkipButton);
        }

        private void OnDisable()
        {
            _hubButton.onClick.RemoveAllListeners();
            _mapButton.onClick.RemoveAllListeners();
            _hideInfoPanelButton.onClick.RemoveAllListeners();
            _hikeAcceptButton.onClick.RemoveAllListeners();
            _closeHikePanelButton.onClick.RemoveAllListeners();
            _hikePanelButton.onClick.RemoveAllListeners();
            _charactersPanelNextButton.onClick.RemoveAllListeners();
            _charactersPanelPreviousButton.onClick.RemoveAllListeners();
            _perkTreeButton.onClick.RemoveAllListeners();
            _shopButton.onClick.RemoveAllListeners();
            _closeTradePanelButton.onClick.RemoveAllListeners();
            _sellButton.onClick.RemoveAllListeners();
            _buyBackButton.onClick.RemoveAllListeners();
            _buyButton.onClick.RemoveAllListeners();
            _closePerksPanelButton.onClick.RemoveAllListeners();
            _clockButton.onClick.RemoveAllListeners();

            _startTimeSkipButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            _stopTimeSkipButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        }

        #endregion


        #region IHubMapUIStart

        public void Starting(HubUIContext context)
        {
            _selected = new SelectedElements();
            _data = BeastHunter.Data.HubUIData;
            _context = context;
            _charactersPanelSwipeStep = _data.MapData.CharactersPanelSwipeStep;
            _tooltipShowingDelay = _data.MapData.TooltipShowingDelay;

            _rightInfoPanelObjectsForDestroy = new List<GameObject>();
            _displayedCurrentCitizensUIBehaviours = new Dictionary<CitizenModel, MapCitizenBehaviour>();
            _displayedDialogAnswerButtons = new List<GameObject>();

            _characterBackpackSlotsUIBehaviours = new List<MapItemSlotBehaviour>();
            _generalInventorySlotsUIBehaviours = new List<MapItemSlotBehaviour>();
            _buyBackSlotsUIBehaviours = new List<MapItemSlotBehaviour>();
            _shopSlotsUIBehaviours = new List<MapShopItemSlotBehaviour>();
            _characterClothesSlotsUIBehaviours = _characterClothPanel.GetComponentsInChildren<MapEquipmentSlotBehaviour>();
            _characterPocketsSlotsBehaviours = new List<MapEquipmentSlotBehaviour>();
            _characterWeaponSlotsUIBehaviours = _weaponSetsPanel.GetComponentsInChildren<MapEquipmentSlotBehaviour>();

            _character3DViewModelRawImageBehaviour = _character3DViewModelRawImage.GetComponent<MapCharacterView3DModelBehaviour>();


            if (_characterClothesSlotsUIBehaviours.Length != _data.CharactersGlobalData.ClothesSlots.Length)
            {
                Debug.LogError("The number of cloth UI slots does not match the equipment of the characters!");
            }

            for (int i = 0; i < _characterClothesSlotsUIBehaviours.Length; i++)
            {
                FillCharacterClothesSlot(i);
            }

            if (_characterWeaponSlotsUIBehaviours.Length != _data.CharactersGlobalData.WeaponSetsAmount * 2)
            {
                Debug.LogError("The number of weapon UI slots does not match the equipment of the characters!");
            }

            for (int i = 0; i < _characterWeaponSlotsUIBehaviours.Length; i++)
            {
                FillWeaponSlot(i);
            }

            MapObjectBehaviour[] mapObjectBehaviour = _mapObjectsPanel.GetComponentsInChildren<MapObjectBehaviour>(true);
            List<MapObjectModel> mapObjects = _context.GetAllMapObjects();

            for (int i = 0; i < mapObjects.Count; i++)
            {
                mapObjects[i].Behaviour = mapObjectBehaviour[mapObjects[i].HubMapIndex];
                FillMapObject(mapObjects[i]);
            }

            for (int i = 0; i < _data.CharactersGlobalData.BackpackSlotAmount; i++)
            {
                InitializeCharacterBackpackSlotUI(i);
            }

            for (int i = 0; i < _context.Player.Inventory.GetSlotsCount(); i++)
            {
                InitializeGeneralInventorySlotUI(i);
            }

            for (int i = 0; i < _data.CitiesShopsSlotsAmount; i++)
            {
                InitializeBuyBackSlotUI(i);
            }

            for (int i = 0; i < _data.CitiesShopsSlotsAmount; i++)
            {
                InitializeShopSlotUI(i);
            }

            for (int i = 0; i < context.Player.AvailableCharacters.GetSlotsCount(); i++)
            {
                InitializeCharacterUI(context.Player.AvailableCharacters.GetElementBySlot(i));
            }

            FillItemStorageSlots(ItemStorageType.GeneralInventory);
            _context.Player.Inventory.OnPutItemToSlotHandler += FillSlotUI;
            _context.Player.Inventory.OnTakeItemFromSlotHandler += FillSlotUI;

            _selected.OnChanged_GeneralInventorySlotIndex = OnSelectedInventorySlot;
            _selected.OnChanged_BuyBackSlotIndex = OnSelectedBuyBackSlot;
            _selected.OnChanged_ShopSlotIndex = OnSelectedShopSlot;
            _selected.OnChanged_Character = OnSelectedCharacter;
            _selected.OnChanged_MapObject = OnSelectedMapObject;

            _context.Player.OnChangeGoldAmount += OnChangedPlayerGoldAmount;
            _character3DViewModelRawImageBehaviour.OnDropHandler += OnDropItemOn3DViewModelRawImage;

            _context.GameTime.OnChangeTimeHandler += OnChanged_GameTime;
            context.GameTime.OnSwitchTimeSkipHandler += OnSwitchTimeSkip;
            OnChanged_GameTime(context.GameTime.CurrentTime);
            OnSwitchTimeSkip(context.GameTime.IsTimePassing);

            _mainPanel.SetActive(false);
            _infoPanel.SetActive(false);
            _cityInfoPanel.SetActive(false);
            _dialogPanel.SetActive(false);
            _locationInfoPanel.SetActive(false);
            _hikePanel.SetActive(false);
            _tradePanel.SetActive(false);
            _sellButton.interactable = false;
            _buyBackButton.interactable = false;
            _buyButton.interactable = false;
            _tooltip.SetActive(false);
            _perksPanel.SetActive(false);
            _hikeAcceptButton.interactable = false;
            _loadingPanel.SetActive(false);
            _character3DViewModelRawImage.enabled = false;
        }

        #endregion


        #region IHubMapUIDestroy

        public void Destroying()
        {
            _context.Player.OnChangeGoldAmount -= OnChangedPlayerGoldAmount;
            _context.Player.Inventory.OnPutItemToSlotHandler -= FillSlotUI;
            _context.Player.Inventory.OnTakeItemFromSlotHandler -= FillSlotUI;
            _character3DViewModelRawImageBehaviour.OnDropHandler -= OnDropItemOn3DViewModelRawImage;
            _context.GameTime.OnChangeTimeHandler -= OnChanged_GameTime;
        }

        #endregion


        #region Methods

        #region OnClick

        private void OnClick_StartTimeSkipButton()
        {
            if (!_context.GameTime.IsTimePassing)
            {
                StartCoroutine(_context.GameTime.StartTimeSkip());
            }
        }

        private void OnClick_StopTimeSkipButton()
        {
            if (_context.GameTime.IsTimePassing)
            {
                _context.GameTime.StopTimeSkip();
            }
        }

        private void OnClick_MapButton()
        {
            OnShowMapUIHandler?.Invoke();
            _mainPanel.SetActive(true);
        }

        private void OnClick_HikePanelButton()
        {
            SetScrollViewParentForPanel(_inventoryItemsPanel, _hikeInventoryScrollView);

            _travelTimeText.text = HubUIServices.SharedInstance.TimeService.
                GetFullPhraseAboutTravelTime(_selected.MapObject as LocationModel);
            _hikePanel.SetActive(true);
        }

        private void OnClick_HubButton()
        {
            OnHideMapUIHandler?.Invoke();
            _mainPanel.SetActive(false);
        }

        private void OnClick_HideInfoPanelButton()
        {
            HideRightInfoPanels();
        }

        private void OnClick_HikeAcceptButton()
        {
            _loadingPanel.SetActive(true);
            _mainPanel.SetActive(false);
            LocationLoad();
        }

        private void OnClick_CloseHikePanelButton()
        {
            _selected.Character = null;
            _selected.GeneralInventorySlotIndex = null;

            _hikePanel.SetActive(false);
        }

        private void OnClick_CityButton(MapObjectModel mapObjectModel)
        {
            _selected.MapObject = mapObjectModel;
            CityModel cityModel = mapObjectModel as CityModel;

            HideRightInfoPanels();
            ClearRightInfoPanel();
            FillCityPanel(cityModel);

            _infoPanel.GetComponent<ScrollRect>().content = _cityInfoPanel.GetComponent<RectTransform>();
            _infoPanel.SetActive(true);

            _cityInfoPanel.SetActive(true);
        }

        private void OnClick_LocationButton(MapObjectModel mapObjectModel)
        {
            _selected.MapObject = mapObjectModel;
            LocationModel locationModel = mapObjectModel as LocationModel;

            HideRightInfoPanels();
            ClearRightInfoPanel();
            FillLocationPanel(locationModel);

            _infoPanel.GetComponent<ScrollRect>().content = _locationInfoPanel.GetComponent<RectTransform>();
            _infoPanel.SetActive(true);

            _locationInfoPanel.SetActive(true);
        }

        private void OnClick_OpenTradePanelButton()
        {
            CityModel city = _selected.MapObject as CityModel;

            SetScrollViewParentForPanel(_inventoryItemsPanel, _shopInventoryScrollView);
            _playerGoldAmount.text = _context.Player.GoldAmount.ToString();
            _shopCityReputation.text = city.PlayerReputation.ToString();

            FillItemStorageSlots(ItemStorageType.ShopStorage);
            city.ShopStorage.OnPutItemToSlotHandler += FillSlotUI;
            city.ShopStorage.OnTakeItemFromSlotHandler += FillSlotUI;

            FillItemStorageSlots(ItemStorageType.ShopBuyBackStorage);
            city.BuyBackStorage.OnPutItemToSlotHandler += FillSlotUI;
            city.BuyBackStorage.OnTakeItemFromSlotHandler += FillSlotUI;

            _tradePanel.SetActive(true);
        }

        private void OnClick_CloseTradePanelButton()
        {
            _selected.GeneralInventorySlotIndex = null;
            _selected.ShopSlotIndex = null;
            _selected.BuyBackSlotIndex = null;

            _tradePanel.SetActive(false);

            CityModel selectedCity = _selected.MapObject as CityModel;
            selectedCity.BuyBackStorage.ClearSlots();
            selectedCity.BuyBackStorage.OnPutItemToSlotHandler -= FillSlotUI;
            selectedCity.BuyBackStorage.OnTakeItemFromSlotHandler -= FillSlotUI;
            selectedCity.ShopStorage.OnPutItemToSlotHandler -= FillSlotUI;
            selectedCity.ShopStorage.OnTakeItemFromSlotHandler -= FillSlotUI;
        }

        private void OnClick_CitizenButton(CitizenModel citizen)
        {
            FillDialogPanel(citizen);
            _dialogPanel.SetActive(true);
        }

        private void OnClick_CharactersPanelNavigationButton(int direction)
        {
            _charactersPanelScrollbar.value += direction * _charactersPanelSwipeStep;
        }

        private void OnClick_PerkTreeButton()
        {
            _hikePanel.SetActive(false);
            SetOtherParentForPanel(_charactersPanelRelocatable, _perksCharactersPanel);
            _perksPanel.SetActive(true);
        }

        private void OnClick_ClosePerksButton()
        {
            _perksPanel.SetActive(false);
            SetOtherParentForPanel(_charactersPanelRelocatable, _hikeCharactersPanel);
            _hikePanel.SetActive(true);
        }

        private void OnClick_CharacterButton(CharacterModel character)
        {
            _selected.Character = character;
        }

        private void OnClick_AnswerButton(CitizenModel citizen, DialogAnswer dialogAnswer)
        {
            dialogAnswer.SelectedByPlayer();

            for (int i = 0; i < _displayedDialogAnswerButtons.Count; i++)
            {
                Destroy(_displayedDialogAnswerButtons[i]);
            }
            _displayedDialogAnswerButtons.Clear();

            if (dialogAnswer.IsDialogEnd)
            {
                _dialogPanel.SetActive(false);
            }
            else
            {
                FillDialogPanel(citizen);
            }
        }

        private void OnClick_SellItemButton()
        {
            if (_selected.GeneralInventorySlotIndex.HasValue)
            {
                HubUIServices.SharedInstance.ShopService.SellItem(_selected.MapObject as CityModel, _selected.GeneralInventorySlotIndex.Value);
                _selected.GeneralInventorySlotIndex = null;
            }
        }

        private void OnClick_BuyBackItemButton()
        {
            if (_selected.BuyBackSlotIndex.HasValue)
            {
                HubUIServices.SharedInstance.ShopService.BuyBackItem(_selected.MapObject as CityModel, _selected.BuyBackSlotIndex.Value);
                _selected.BuyBackSlotIndex = null;
            }
        }

        private void OnClick_BuyItemButton()
        {
            if (_selected.ShopSlotIndex.HasValue)
            {
                HubUIServices.SharedInstance.ShopService.BuyItem(_selected.MapObject as CityModel, _selected.ShopSlotIndex.Value);
                _selected.ShopSlotIndex = null;
            }
        }

        #endregion

        #region OnPointer

        private void OnPointerDown_Slot(int slotIndex, ItemStorageType storageType)
        {
            switch (storageType)
            {
                case ItemStorageType.GeneralInventory:
                    _selected.GeneralInventorySlotIndex = slotIndex;
                    break;

                case ItemStorageType.ShopBuyBackStorage:
                    _selected.BuyBackSlotIndex = slotIndex;
                    break;

                case ItemStorageType.ShopStorage:
                    _selected.ShopSlotIndex = slotIndex;
                    break;

                default:
                    Debug.LogError("Incorrect ItemStorageType");
                    break;
            }
        }

        private void OnPointerEnter_Slot(int slotIndex, ItemStorageType storageType)
        {
            StopShowTooltipCoroutine();
            _showTooltipCoroutine = StartCoroutine(ShowTooltip(slotIndex, storageType));
        }

        private void OnPointerExit_Slot(int slotIndex, ItemStorageType storageType)
        {
            StopShowTooltipCoroutine();
            _tooltip.SetActive(false);
        }

        #endregion

        #region OnDoubleClick

        private void OnDoubleClick_CharacterEquipmentSlot(int slotIndex, ItemStorageType storageType)
        {
            if (_selected.Character != null)
            {
                if(_context.Player.Inventory.PutElementToFirstEmptySlotFromOtherStorage(GetStorageByType(storageType), slotIndex))
                {
                    StopShowTooltipCoroutine();
                    _tooltip.SetActive(false);
                }
                else
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("Inventory is full");
                }
            }
        }

        private void OnDoubleClick_StorageSlot(int slotIndex, ItemStorageType storageType)
        {
            BaseItemSlotStorage storage = GetStorageByType(storageType);
            BaseItemModel itemInClickedSlot = storage.GetElementBySlot(slotIndex);

            if (_hikePanel.activeSelf && itemInClickedSlot != null && _selected.Character != null)
            {
                if(itemInClickedSlot.ItemType == ItemType.Clothes)
                {
                    if(_selected.Character.EquipClothesItem(storage, slotIndex))
                    {
                        StopShowTooltipCoroutine();
                        _tooltip.SetActive(false);
                    }
                }
                else if (itemInClickedSlot.ItemType == ItemType.Weapon)
                {
                    if(_selected.Character.EquipWeaponItem(storage, slotIndex))
                    {
                        StopShowTooltipCoroutine();
                        _tooltip.SetActive(false);
                    }
                }
            }
        }

        #endregion

        #region OnDragAndDrop

        private void OnBeginDragItemFromSlot(int slotIndex, ItemStorageType storageType)
        {
            _tooltip.SetActive(false);
            StopShowTooltipCoroutine();
            _draggedItemInfo.slotIndex = slotIndex;
            _draggedItemInfo.storageType = storageType;
        }

        private void OnEndDragItem(int slotIndex, ItemStorageType storageType)
        {
            FillSlotUI(storageType, slotIndex, GetStorageByType(storageType).GetElementBySlot(slotIndex));
            _draggedItemInfo.slotIndex = null;
        }

        private void OnDropItemToSlot(int dropSlotIndex, ItemStorageType dropStorageType)
        {
            if (_draggedItemInfo.slotIndex.HasValue)
            {
                GetStorageByType(dropStorageType).SwapElementsWithOtherStorage(dropSlotIndex,
                    GetStorageByType(_draggedItemInfo.storageType), _draggedItemInfo.slotIndex.Value);

                switch (dropStorageType)
                {
                    case ItemStorageType.GeneralInventory:
                        _selected.GeneralInventorySlotIndex = dropSlotIndex;
                        break;

                    default:
                        break;
                }
            }
        }

         private void OnDropItemOn3DViewModelRawImage()
        {
            if (_draggedItemInfo.slotIndex.HasValue)
            {
                _selected.Character.EquipClothesItem(GetStorageByType(_draggedItemInfo.storageType), _draggedItemInfo.slotIndex.Value);
            }
        }

        #endregion

        #region OnSelected

        private void OnSelectedInventorySlot(int? previousSlotIndex)
        {
            if (previousSlotIndex.HasValue)
            {
                _generalInventorySlotsUIBehaviours[previousSlotIndex.Value].SelectFrameSwitcher(false);
            }

            if (_selected.GeneralInventorySlotIndex.HasValue)
            {
                _generalInventorySlotsUIBehaviours[_selected.GeneralInventorySlotIndex.Value].SelectFrameSwitcher(true);

                if (_tradePanel.activeSelf)
                {
                    if (_context.Player.Inventory.GetElementBySlot(_selected.GeneralInventorySlotIndex.Value) != null)
                    {
                        _sellingItemPrice.text = HubUIServices.SharedInstance.ShopService.
                            CountSellPrice(_context.Player.Inventory.GetElementBySlot(_selected.GeneralInventorySlotIndex.Value)).ToString();
                        _sellButton.interactable = true;
                    }
                    else
                    {
                        _sellButton.interactable = false;
                        _sellingItemPrice.text = "0";
                    }
                }
            }
            else
            {
                if (_tradePanel.activeSelf)
                {
                    _sellButton.interactable = false;
                    _sellingItemPrice.text = "";
                }
            }
        }

        private void OnSelectedBuyBackSlot(int? previousSlotIndex)
        {
            if (previousSlotIndex.HasValue)
            {
                _buyBackSlotsUIBehaviours[previousSlotIndex.Value].SelectFrameSwitcher(false);
            }

            if (_selected.BuyBackSlotIndex.HasValue)
            {
                _buyBackSlotsUIBehaviours[_selected.BuyBackSlotIndex.Value].SelectFrameSwitcher(true);

                if (_tradePanel.activeSelf)
                {
                    BaseItemModel item = (_selected.MapObject as CityModel).BuyBackStorage.
                        GetElementBySlot(_selected.BuyBackSlotIndex.Value);

                    if (item != null)
                    {
                        _buybackItemPrice.text = HubUIServices.SharedInstance.ShopService.CountSellPrice(item).ToString();
                        _buyBackButton.interactable = _context.Player.GoldAmount >= HubUIServices.SharedInstance.ShopService.CountSellPrice(item);
                    }
                    else
                    {
                        _buyBackButton.interactable = false;
                        _buybackItemPrice.text = "0";
                    }
                }
            }
            else
            {
                if (_tradePanel.activeSelf)
                {
                    _buyBackButton.interactable = false;
                    _buybackItemPrice.text = "";
                }
            }
        }

        private void OnSelectedShopSlot(int? previousSlotIndex)
        {
            if (previousSlotIndex.HasValue)
            {
                _shopSlotsUIBehaviours[previousSlotIndex.Value].SelectFrameSwitcher(false);
            }

            if (_selected.ShopSlotIndex.HasValue)
            {
                _shopSlotsUIBehaviours[_selected.ShopSlotIndex.Value].SelectFrameSwitcher(true);

                if (_tradePanel.activeSelf)
                {
                    BaseItemModel item = (_selected.MapObject as CityModel).ShopStorage.
                        GetElementBySlot(_selected.ShopSlotIndex.Value);

                    if (item != null)
                    {
                        _buyingItemPrice.text = HubUIServices.SharedInstance.ShopService.GetItemPrice(item).ToString();
                        _buyButton.interactable = HubUIServices.SharedInstance.ShopService.IsPossibleToBuyShopItem(item, _selected.MapObject as CityModel);
                    }
                    else
                    {
                        _buyButton.interactable = false;
                        _buyingItemPrice.text = "0";
                    }
                }
            }
            else
            {
                if (_tradePanel.activeSelf)
                {
                    _buyButton.interactable = false;
                    _buyingItemPrice.text = "";
                }
            }
        }

        private void OnSelectedCharacter(CharacterModel previousCharacter)
        {
            if (previousCharacter != null)
            {
                previousCharacter.Behaviour.SelectFrameSwitch(false);

                previousCharacter.Backpack.OnPutItemToSlotHandler -= FillSlotUI;
                previousCharacter.Backpack.OnTakeItemFromSlotHandler -= FillSlotUI;

                previousCharacter.ClothesEquipment.OnPutItemToSlotHandler -= FillSlotUI;
                previousCharacter.ClothesEquipment.OnTakeItemFromSlotHandler -= FillSlotUI;

                previousCharacter.WeaponEquipment.OnPutItemToSlotHandler -= FillSlotUI;
                previousCharacter.WeaponEquipment.OnTakeItemFromSlotHandler -= FillSlotUI;
                previousCharacter.WeaponEquipment.OnTwoHandedWeaponHandler -= FillWeaponSlotUIAsSecondary;

                previousCharacter.Pockets.OnPutItemToSlotHandler -= FillSlotUI;
                previousCharacter.Pockets.OnTakeItemFromSlotHandler -= FillSlotUI;
                previousCharacter.Pockets.OnChangeSlotsAmountHandler -= OnChangedPocketsSlotsAmount;

                DestroyPocketsSlotsUI();

                previousCharacter.View3DModelObjectOnScene.SetActive(false);
                previousCharacter.View3DModelObjectOnScene.transform.rotation = Quaternion.identity;
            }
            else
            {
                if (_selected.Character != null)
                {
                    SetStorageSlotsInteractable(true, _characterBackpackSlotsUIBehaviours);
                    SetStorageSlotsInteractable(true, _characterClothesSlotsUIBehaviours);
                    SetStorageSlotsInteractable(true, _characterWeaponSlotsUIBehaviours);
                }
            }

            if (_selected.Character != null)
            {
                _selected.Character.Behaviour.SelectFrameSwitch(true);
                _hikeAcceptButton.interactable = true;

                FillItemStorageSlots(ItemStorageType.CharacterBackpack);
                _selected.Character.Backpack.OnPutItemToSlotHandler += FillSlotUI;
                _selected.Character.Backpack.OnTakeItemFromSlotHandler += FillSlotUI;

                FillItemStorageSlots(ItemStorageType.ClothesEquipment);
                _selected.Character.ClothesEquipment.OnPutItemToSlotHandler += FillSlotUI;
                _selected.Character.ClothesEquipment.OnTakeItemFromSlotHandler += FillSlotUI;

                FillItemStorageSlots(ItemStorageType.WeaponEquipment);
                _selected.Character.WeaponEquipment.OnPutItemToSlotHandler += FillSlotUI;
                _selected.Character.WeaponEquipment.OnTakeItemFromSlotHandler += FillSlotUI;
                _selected.Character.WeaponEquipment.OnTwoHandedWeaponHandler += FillWeaponSlotUIAsSecondary;

                InitializePocketsSlotsUI(_selected.Character.Pockets.GetSlotsCount());
                _selected.Character.Pockets.OnPutItemToSlotHandler += FillSlotUI;
                _selected.Character.Pockets.OnTakeItemFromSlotHandler += FillSlotUI;
                _selected.Character.Pockets.OnChangeSlotsAmountHandler += OnChangedPocketsSlotsAmount;

                _selected.Character.View3DModelObjectOnScene.SetActive(true);
                _character3DViewModelRawImage.enabled = true;
                _character3DViewModelRawImageBehaviour.RotateObject = _selected.Character.View3DModelObjectOnScene;
            }
            else
            {
                _character3DViewModelRawImage.enabled = false;
                _hikeAcceptButton.interactable = false;
                SetStorageSlotsInteractable(false, _characterBackpackSlotsUIBehaviours);
                SetStorageSlotsInteractable(false, _characterClothesSlotsUIBehaviours);
                SetStorageSlotsInteractable(false, _characterWeaponSlotsUIBehaviours);
            }
        }

        private void OnSelectedMapObject(MapObjectModel previousMapObject)
        {
            if (previousMapObject != null)
            {
                previousMapObject.Behaviour.SelectFrameSwitch(false);
            }

            if (_selected.MapObject != null)
            {
                _selected.MapObject.Behaviour.SelectFrameSwitch(true);
            }
        }

        #endregion

        #region OnChanged

        private void OnSwitchTimeSkip(bool isTimePassing)
        {
            _startTimeSkipButton.SetActive(!isTimePassing);
            _stopTimeSkipButton.SetActive(isTimePassing);
        }

        private void OnChanged_GameTime(GameTimeStruct currentTime)
        {
            _currentDayText.text = "Day: " + currentTime.Day.ToString();
            _currentHourText.text = "Hour: " + currentTime.Hour.ToString();
        }

        private void OnChangedPocketsSlotsAmount()
        {
            DestroyPocketsSlotsUI();
            InitializePocketsSlotsUI(_selected.Character.Pockets.GetSlotsCount());
        }

        private void OnChangedPlayerGoldAmount(int goldAmount)
        {
            _playerGoldAmount.text = goldAmount.ToString();

            for (int i = 0; i < _shopSlotsUIBehaviours.Count; i++)
            {
                CityModel selectedCity = _selected.MapObject as CityModel;
                _shopSlotsUIBehaviours[i].SetAvailability
                    (HubUIServices.SharedInstance.ShopService.IsPossibleToBuyShopItem(selectedCity.ShopStorage.GetElementBySlot(i), selectedCity));
            }
        }

        #endregion

        #region InitializeUIElements

        private void InitializePocketsSlotsUI(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject slotUI = InstantiateUIObject(_data.MapData.EquipmentSlotUIPrefab, _pocketSlotsPanel);
                MapEquipmentSlotBehaviour slotBehaviour = slotUI.GetComponent<MapEquipmentSlotBehaviour>();
                slotBehaviour.Initialize(ItemStorageType.PocketsStorage, i, _data.MapData.PocketItemSlotIcon);
                slotBehaviour.UpdateSlot(_selected.Character.Pockets.GetElementBySlot(i));
                slotBehaviour.OnBeginDragHandler = OnBeginDragItemFromSlot;
                slotBehaviour.OnDropHandler = OnDropItemToSlot;
                slotBehaviour.OnEndDragHandler = OnEndDragItem;
                slotBehaviour.OnPointerEnterHandler = OnPointerEnter_Slot;
                slotBehaviour.OnPointerExitHandler = OnPointerExit_Slot;
                _characterPocketsSlotsBehaviours.Add(slotBehaviour);
            }
        }

        private void DestroyPocketsSlotsUI()
        {
            for (int i = 0; i < _characterPocketsSlotsBehaviours.Count; i++)
            {
                Destroy(_characterPocketsSlotsBehaviours[i].gameObject);
            }
            _characterPocketsSlotsBehaviours.Clear();
        }

        private void InitializeCharacterUI(CharacterModel character)
        {
            GameObject characterUI = InstantiateUIObject(_data.MapData.CharacterUIPrefab, _charactersPanelFillable);
            MapCharacterBehaviour behaviourUI = characterUI.GetComponentInChildren<MapCharacterBehaviour>();
            behaviourUI.Initialize(character);
            behaviourUI.OnClick_ButtonHandler += OnClick_CharacterButton;
        }

        private void InitializeCharacterBackpackSlotUI(int slotIndex)
        {
            GameObject backpackSlotUI = InstantiateUIObject(_data.MapData.CharacterBackpackSlotUIPrefab, _characterInventoryPanel);

            MapItemSlotBehaviour slotBehaviour = backpackSlotUI.GetComponent<MapItemSlotBehaviour>();
            slotBehaviour.Initialize(ItemStorageType.CharacterBackpack, slotIndex);
            slotBehaviour.SetInteractable(false);
            slotBehaviour.OnDoubleClickButtonHandler = OnDoubleClick_StorageSlot;
            slotBehaviour.OnBeginDragHandler = OnBeginDragItemFromSlot;
            slotBehaviour.OnDropHandler = OnDropItemToSlot;
            slotBehaviour.OnEndDragHandler = OnEndDragItem;
            slotBehaviour.OnPointerEnterHandler = OnPointerEnter_Slot;
            slotBehaviour.OnPointerExitHandler = OnPointerExit_Slot;

            _characterBackpackSlotsUIBehaviours.Add(slotBehaviour);
        }

        private void InitializeGeneralInventorySlotUI(int slotIndex)
        {
            GameObject inventorySlotUI = InstantiateUIObject(_data.MapData.InventorySlotUIPrefab, _inventoryItemsPanel);

            MapItemSlotBehaviour slotBehaviour = inventorySlotUI.GetComponent<MapItemSlotBehaviour>();
            slotBehaviour.Initialize(ItemStorageType.GeneralInventory, slotIndex);
            slotBehaviour.OnPointerDownHandler = OnPointerDown_Slot;
            slotBehaviour.OnDoubleClickButtonHandler = OnDoubleClick_StorageSlot;
            slotBehaviour.OnBeginDragHandler = OnBeginDragItemFromSlot;
            slotBehaviour.OnDropHandler = OnDropItemToSlot;
            slotBehaviour.OnEndDragHandler = OnEndDragItem;
            slotBehaviour.OnPointerEnterHandler = OnPointerEnter_Slot;
            slotBehaviour.OnPointerExitHandler = OnPointerExit_Slot;

            _generalInventorySlotsUIBehaviours.Add(slotBehaviour);
        }

        private void InitializeBuyBackSlotUI(int slotIndex)
        {
            GameObject buyBackSlotUI = InstantiateUIObject(_data.MapData.ShopSlotUIPrefab, _buyBackItemsPanel);

            MapItemSlotBehaviour slotBehaviour = buyBackSlotUI.GetComponent<MapItemSlotBehaviour>();
            slotBehaviour.Initialize(ItemStorageType.ShopBuyBackStorage, slotIndex);
            slotBehaviour.IsDragAndDropOn = false;
            slotBehaviour.SetInteractable(false);
            slotBehaviour.OnPointerDownHandler = OnPointerDown_Slot;
            slotBehaviour.OnPointerEnterHandler = OnPointerEnter_Slot;
            slotBehaviour.OnPointerExitHandler = OnPointerExit_Slot;

            _buyBackSlotsUIBehaviours.Add(slotBehaviour);
        }

        private void InitializeShopSlotUI(int slotIndex)
        {
            GameObject shopSlotUI = InstantiateUIObject(_data.MapData.ShopSlotUIPrefab, _buyingItemsPanel);

            MapShopItemSlotBehaviour slotBehaviour = shopSlotUI.GetComponent<MapShopItemSlotBehaviour>();
            slotBehaviour.Initialize(ItemStorageType.ShopStorage, slotIndex);
            slotBehaviour.IsDragAndDropOn = false;
            slotBehaviour.SetInteractable(false);
            slotBehaviour.OnPointerDownHandler = OnPointerDown_Slot;
            slotBehaviour.OnPointerEnterHandler = OnPointerEnter_Slot;
            slotBehaviour.OnPointerExitHandler = OnPointerExit_Slot;

            _shopSlotsUIBehaviours.Add(slotBehaviour);
        }

        private void InitializeCitizenUI(CitizenModel citizen)
        {
            GameObject citizenUI = InstantiateUIObject(_data.MapData.CitizenUIPrefab, _citizenPanel);

            MapCitizenBehaviour citizenUIBehaviour = citizenUI.GetComponentInChildren<MapCitizenBehaviour>();
            citizenUIBehaviour.FillCitizenInfo(citizen);
            citizenUIBehaviour.OnClick_CitizenButtonHandler = OnClick_CitizenButton;

            citizen.OnChangeQuestMarkerTypeHandler += citizenUIBehaviour.SetQuestMarker;

            _rightInfoPanelObjectsForDestroy.Add(citizenUI);
            _displayedCurrentCitizensUIBehaviours.Add(citizen, citizenUIBehaviour);
        }

        private void InitializeDialogAnswerButton(CitizenModel citizen, DialogAnswer answer)
        {
            GameObject answerButton = InstantiateUIObject(_data.MapData.AnswerButtonUIPrefab, _answerButtonsPanel);
            answerButton.GetComponentInChildren<Text>().text = answer.Text;

            if (answer.IsDialogEnd)
            {
                answerButton.GetComponentInChildren<Text>().text += " (уйти)";
            }

            answerButton.GetComponentInChildren<Button>().interactable = answer.IsInteractable;
            answerButton.GetComponentInChildren<Button>().onClick.AddListener(() => OnClick_AnswerButton(citizen, answer));
            _displayedDialogAnswerButtons.Add(answerButton);
        }

        #endregion

        #region FillUIElementsByInfo

        private void FillCharacterClothesSlot(int slotIndex)
        {
            Sprite slotSprite = _data.MapData.GetClothSlotSpriteByType(_data.CharactersGlobalData.ClothesSlots[slotIndex]);
            _characterClothesSlotsUIBehaviours[slotIndex].Initialize(ItemStorageType.ClothesEquipment, slotIndex, slotSprite);
            _characterClothesSlotsUIBehaviours[slotIndex].SetInteractable(false);
            _characterClothesSlotsUIBehaviours[slotIndex].OnDoubleClickButtonHandler = OnDoubleClick_CharacterEquipmentSlot;
            _characterClothesSlotsUIBehaviours[slotIndex].OnBeginDragHandler += OnBeginDragItemFromSlot;
            _characterClothesSlotsUIBehaviours[slotIndex].OnEndDragHandler += OnEndDragItem;
            _characterClothesSlotsUIBehaviours[slotIndex].OnDropHandler += OnDropItemToSlot;
            _characterClothesSlotsUIBehaviours[slotIndex].OnPointerEnterHandler += OnPointerEnter_Slot;
            _characterClothesSlotsUIBehaviours[slotIndex].OnPointerExitHandler += OnPointerExit_Slot;
        }

        private void FillWeaponSlot(int slotIndex)
        {
            Sprite slotSprite = _data.MapData.WeaponSlotIcon;
            _characterWeaponSlotsUIBehaviours[slotIndex].Initialize(ItemStorageType.WeaponEquipment, slotIndex, slotSprite);
            _characterWeaponSlotsUIBehaviours[slotIndex].SetInteractable(false);
            _characterWeaponSlotsUIBehaviours[slotIndex].OnDoubleClickButtonHandler = OnDoubleClick_CharacterEquipmentSlot;
            _characterWeaponSlotsUIBehaviours[slotIndex].OnBeginDragHandler += OnBeginDragItemFromSlot;
            _characterWeaponSlotsUIBehaviours[slotIndex].OnEndDragHandler += OnEndDragItem;
            _characterWeaponSlotsUIBehaviours[slotIndex].OnDropHandler += OnDropItemToSlot;
            _characterWeaponSlotsUIBehaviours[slotIndex].OnPointerEnterHandler += OnPointerEnter_Slot;
            _characterWeaponSlotsUIBehaviours[slotIndex].OnPointerExitHandler += OnPointerExit_Slot;
        }

        private void FillMapObject(MapObjectModel mapObjectModel)
        {
            MapObjectBehaviour behaviour = mapObjectModel.Behaviour;

            if (mapObjectModel is CityModel)
            {
                behaviour.FillInfo(mapObjectModel as CityModel);
                behaviour.OnClick_ButtonHandler += OnClick_CityButton;
            }
            else if (mapObjectModel is LocationModel)
            {
                behaviour.FillInfo(mapObjectModel as LocationModel);
                behaviour.OnClick_ButtonHandler += OnClick_LocationButton;
            }
            else
            {
                Debug.LogError(this + " unknown map object model value");
            }
        }

        private void FillTooltipByItemInfo(int slotIndex, ItemStorageType storageType)
        {
            BaseItemModel item = GetStorageByType(storageType).GetElementBySlot(slotIndex);

            if (item != null)
            {
                Text[] tooltipTexts = _tooltip.GetComponentsInChildren<Text>(true);

                tooltipTexts[0].gameObject.SetActive(false);
                tooltipTexts[1].text = item.Name;
                tooltipTexts[2].text = item.Description;
                tooltipTexts[3].text = "Цена: " + HubUIServices.SharedInstance.ShopService.GetItemPrice(item).ToString();

                if (storageType == ItemStorageType.ShopStorage)
                {
                    if(!HubUIServices.SharedInstance.ShopService.IsPossibleToBuyShopItem(item, _selected.MapObject as CityModel, out string message))
                    {
                        tooltipTexts[0].gameObject.SetActive(true);
                        tooltipTexts[0].text = message;
                    }
                }
            }
        }

        private void FillSlotUI(ItemStorageType storageType, int slotIndex, BaseItemModel item)
        {
            BaseItemModel itemInSlot = GetStorageByType(storageType).GetElementBySlot(slotIndex);

            switch (storageType)
            {
                case ItemStorageType.CharacterBackpack:

                    _characterBackpackSlotsUIBehaviours[slotIndex].UpdateSlot(itemInSlot);

                    break;
                case ItemStorageType.GeneralInventory:

                    _generalInventorySlotsUIBehaviours[slotIndex].UpdateSlot(itemInSlot);

                    break;
                case ItemStorageType.ShopBuyBackStorage:

                    _buyBackSlotsUIBehaviours[slotIndex].UpdateSlot(itemInSlot);
                    _buyBackSlotsUIBehaviours[slotIndex].SetInteractable(itemInSlot != null);

                    break;
                case ItemStorageType.ShopStorage:

                    _shopSlotsUIBehaviours[slotIndex].UpdateSlot(itemInSlot);
                    _shopSlotsUIBehaviours[slotIndex].SetInteractable(itemInSlot != null);
                    _shopSlotsUIBehaviours[slotIndex].SetAvailability(HubUIServices.SharedInstance.ShopService.IsPossibleToBuyShopItem
                        (itemInSlot, _selected.MapObject as CityModel));

                    break;
                case ItemStorageType.ClothesEquipment:

                    _characterClothesSlotsUIBehaviours[slotIndex].UpdateSlot(itemInSlot);

                    break;
                case ItemStorageType.PocketsStorage:

                    _characterPocketsSlotsBehaviours[slotIndex].UpdateSlot(itemInSlot);

                    break;
                case ItemStorageType.WeaponEquipment:

                    _characterWeaponSlotsUIBehaviours[slotIndex].UpdateSlot(itemInSlot);

                    break;
                default:

                    Debug.LogError(this + ": incorrect StorageSlotType");

                    break;
            }
        }

        private void FillWeaponSlotUIAsSecondary(int slotIndex, WeaponItemModel weapon)
        {
            _characterWeaponSlotsUIBehaviours[slotIndex].FillSlotAsSecondary(weapon);
        }

        private void FillItemStorageSlots(ItemStorageType storageType)
        {
            BaseItemSlotStorage storage = GetStorageByType(storageType);

            for (int i = 0; i < storage.GetSlotsCount(); i++)
            {
                FillSlotUI(storageType, i, storage.GetElementBySlot(i));
            }

            if (storageType == ItemStorageType.WeaponEquipment)
            {
                for (int i = 1; i < storage.GetSlotsCount(); i += 2)
                {
                    WeaponItemModel adjacentWeapon = (storage as WeaponSlotStorage).AdjacentWeapon(i);
                    if (adjacentWeapon != null && adjacentWeapon.IsTwoHanded)
                    {
                        FillWeaponSlotUIAsSecondary(i, adjacentWeapon);
                    }
                }
            }
        }

        private void FillCityPanel(CityModel city)
        {
            _cityFraction.sprite = city.Fraction.Logo;
            _cityName.text = city.Name;
            _cityDescription.text = city.Description;
            _cityReputation.text = city.PlayerReputation.ToString();

            for (int i = 0; i < city.Citizens.Count; i++)
            {
                InitializeCitizenUI(city.Citizens[i]);
            }
        }

        private void FillLocationPanel(LocationModel location)
        {
            _locationScreen.sprite = location.Screenshot;
            _locationName.text = location.Name;
            _locationDescription.text = location.Description;

            if (location.Dwellers.Length > 0)
            {
                _dwellersPanel.transform.GetChild(0).gameObject.SetActive(false);

                for (int i = 0; i < location.Dwellers.Length; i++)
                {
                    GameObject dwellerUI = InstantiateUIObject(_data.MapData.LocationTextUIPrefab, _dwellersPanel);
                    _rightInfoPanelObjectsForDestroy.Add(dwellerUI);
                    dwellerUI.GetComponentInChildren<Text>().text = location.Dwellers[i].Name;
                }
            }
            else
            {
                _dwellersPanel.transform.GetChild(0).gameObject.SetActive(true);
            }


            if (location.Ingredients.Length > 0)
            {
                _ingredientsPanel.transform.GetChild(0).gameObject.SetActive(false);

                for (int i = 0; i < location.Ingredients.Length; i++)
                {
                    GameObject ingredientUI = InstantiateUIObject(_data.MapData.LocationTextUIPrefab, _ingredientsPanel);
                    _rightInfoPanelObjectsForDestroy.Add(ingredientUI);
                    ingredientUI.GetComponentInChildren<Text>().text = location.Ingredients[i].Name;
                }
            }
            else
            {
                _ingredientsPanel.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        private void FillDialogPanel(CitizenModel citizen)
        {
            _citizenName.text = citizen.Name;
            _citizenPortrait.sprite = citizen.Portrait;
            _dialogText.text = citizen.CurrentDialog.Text;

            List<DialogAnswer> answers = citizen.GetAllCurrentAnswers();
            for (int i = 0; i < answers.Count; i++)
            {
                InitializeDialogAnswerButton(citizen, answers[i]);
            }
        }

        #endregion

        #region Other Methods

        private void HideRightInfoPanels()
        {
            _cityInfoPanel.SetActive(false);
            _locationInfoPanel.SetActive(false);
            _infoPanel.SetActive(false);
        }

        private void ClearRightInfoPanel()
        {
            foreach (KeyValuePair<CitizenModel, MapCitizenBehaviour> kvp in _displayedCurrentCitizensUIBehaviours)
            {
                kvp.Key.OnChangeQuestMarkerTypeHandler -= kvp.Value.SetQuestMarker;
            }

            for (int i=0; i< _rightInfoPanelObjectsForDestroy.Count; i++)
            {
                Destroy(_rightInfoPanelObjectsForDestroy[i]);
            }

            _rightInfoPanelObjectsForDestroy.Clear();
            _displayedCurrentCitizensUIBehaviours.Clear();
        }

         private void SetStorageSlotsInteractable(bool flag, IEnumerable<BaseItemSlotBehaviour> slotBehaviours)
        {
            foreach (BaseItemSlotBehaviour slot in slotBehaviours)
            {
                slot.SetInteractable(flag);
            }
        }

        private BaseItemSlotStorage GetStorageByType(Enum storageType)
        {
            BaseItemSlotStorage storage = null;
            switch (storageType)
            {
                case ItemStorageType.CharacterBackpack:
                    storage = _selected.Character.Backpack;
                    break;

                case ItemStorageType.GeneralInventory:
                    storage = _context.Player.Inventory;
                    break;

                case ItemStorageType.ShopBuyBackStorage:
                    storage = (_selected.MapObject as CityModel).BuyBackStorage;
                    break;

                case ItemStorageType.ShopStorage:
                    storage = (_selected.MapObject as CityModel).ShopStorage;
                    break;

                case ItemStorageType.ClothesEquipment:
                    storage = _selected.Character.ClothesEquipment;
                    break;

                case ItemStorageType.PocketsStorage:
                    storage = _selected.Character.Pockets;
                    break;

                case ItemStorageType.WeaponEquipment:
                    storage = _selected.Character.WeaponEquipment;
                    break;

                default:
                    Debug.LogError(this + ": incorrect StorageSlotType");
                    break;
            }
            return storage;
        }

        private void SetScrollViewParentForPanel(GameObject panel, GameObject parentPanel)
        {
            panel.transform.SetParent(parentPanel.transform.Find("Viewport"), false);
            parentPanel.GetComponent<ScrollRect>().content = panel.GetComponent<RectTransform>();
        }

        private void SetOtherParentForPanel(GameObject panel, GameObject parentPanel)
        {
            panel.transform.SetParent(parentPanel.transform, false);
        }

        private GameObject InstantiateUIObject(GameObject prefab, GameObject parent)
        {
            GameObject objectUI = GameObject.Instantiate(prefab);
            objectUI.transform.SetParent(parent.transform, false);
            objectUI.transform.localScale = new Vector3(1, 1, 1);
            return objectUI;
        }

        private void StopShowTooltipCoroutine()
        {
            if (_showTooltipCoroutine != null)
            {
                StopCoroutine(_showTooltipCoroutine);
            }
        }

        private IEnumerator ShowTooltip(int slotIndex, ItemStorageType storageType)
        {
            yield return new WaitForSeconds(_tooltipShowingDelay);

            FillTooltipByItemInfo(slotIndex, storageType);

            RectTransform tooltipRectTransform = _tooltip.GetComponent<RectTransform>();

            if (storageType == ItemStorageType.GeneralInventory && _hikePanel.activeSelf)
            {
                tooltipRectTransform.pivot = new Vector2(0, 1);
            }
            else if (storageType == ItemStorageType.ShopStorage || storageType == ItemStorageType.ShopBuyBackStorage)
            {
                tooltipRectTransform.pivot = new Vector2(1, 0);
            }
            else
            {
                tooltipRectTransform.pivot = new Vector2(0, 0);
            }

            _tooltip.transform.position = Mouse.current.position.ReadValue();
            _tooltip.SetActive(true);

            yield return null;
        }

         private void LocationLoad()
         {
            Debug.Log("Load location. ID: " + (_selected.MapObject as LocationModel).LoadSceneId);
            SceneManager.LoadScene((_selected.MapObject as LocationModel).LoadSceneId);
         }

        #endregion

        #endregion
    }
}
