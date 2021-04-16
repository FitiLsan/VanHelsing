using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace BeastHunter
{
    public class HubMapUIMainBehaviour : MonoBehaviour
    {
        #region PrivateData

        private class SelectedElements
        {
            private int? _generalInventorySlotIndex;
            private int? _shopSlotIndex;
            private int? _buyBackSlotIndex;
            private HubMapUICharacterModel _character;
            private HubMapUIMapObjectModel _mapObject;

            public Action<int?> OnChanged_GeneralInventorySlotIndex { get; set; }
            public Action<int?> OnChanged_ShopSlotIndex { get; set; }
            public Action<int?> OnChanged_BuyBackSlotIndex { get; set; }
            public Action<HubMapUICharacterModel> OnChanged_Character { get; set; }
            public Action<HubMapUIMapObjectModel> OnChanged_MapObject { get; set; }

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

            public HubMapUICharacterModel Character
            {
                get
                {
                    return _character;
                }
                set
                {
                    if (value != _character)
                    {
                        HubMapUICharacterModel previousValue = _character;
                        _character = value;
                        OnChanged_Character?.Invoke(previousValue);
                    }
                }
            }

            public HubMapUIMapObjectModel MapObject
            {
                get
                {
                    return _mapObject;
                }
                set
                {
                    if (value != _mapObject)
                    {
                        HubMapUIMapObjectModel previousValue = _mapObject;
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


        #region Constants

        private const float CHARACTERS_PANEL_SCROLLBAR_STEP = 1.0f;

        #endregion


        #region Fields

        [Header("Map objects")]
        [SerializeField] private GameObject[] _mapObjects;

        [Header("Hub map")]
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private Button _hideInfoPanelButton;
        [SerializeField] private Button _hubButton;
        [SerializeField] private Button _mapButton;
        [SerializeField] private GameObject _inventoryItemsPanel;
        [SerializeField] private GameObject _tooltip;
        [SerializeField] private GameObject _loadingPanel;

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


        private HubMapUIContext _context;
        private HubMapUIQuestController _questController;

        private HubMapUIData _data;
        private HubMapUIPlayerModel _player;
        private HubMapUIItemStorage _generalInventory;

        private List<GameObject> _rightInfoPanelObjectsForDestroy;
        private List<HubMapUIStorageSlotBehaviour> _characterBackpuckSlotsUIBehaviours;
        private List<HubMapUIStorageSlotBehaviour> _generalInventorySlotsUIBehaviours;
        private List<HubMapUIShopSlotBehaviour> _shopSlotsUIBehaviours;
        private List<HubMapUIStorageSlotBehaviour> _buyBackSlotsUIBehaviours;
        private HubMapUIEquipmentSlotBehaviour[] _characterClothSlotsUIBehaviours;
        private List<HubMapUIEquipmentSlotBehaviour> _characterPocketsSlotsBehaviours;
        private Dictionary<HubMapUICitizenModel, HubMapUICitizenBehaviour> _displayedCurrentCitizensUIBehaviours;
        private List<GameObject> _displayedDialogAnswerButtons;
        private (int? slotIndex, HubMapUIItemStorageType storageType) _draggedItemInfo;

        private HubMapUIItemStorage _currentBuyBackStorage;
        private HubMapUIItemStorage _currentShopStorage;
        private SelectedElements _selected;

        private GameObject _character3DViewModelRendering;
        private HubMapUIView3DModelBehaviour _character3DViewModelRawImageBehaviour;

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
            _charactersPanelNextButton.onClick.AddListener(()=> OnClick_CharactersPanelNavigationButton(CHARACTERS_PANEL_SCROLLBAR_STEP));
            _charactersPanelPreviousButton.onClick.AddListener(() => OnClick_CharactersPanelNavigationButton(-CHARACTERS_PANEL_SCROLLBAR_STEP));
            _perkTreeButton.onClick.AddListener(OnClick_PerkTreeButton);
            _shopButton.onClick.AddListener(OnClick_OpenTradePanelButton);
            _closeTradePanelButton.onClick.AddListener(OnClick_CloseTradePanelButton);
            _sellButton.onClick.AddListener(OnClick_SellItemButton);
            _buyBackButton.onClick.AddListener(OnClick_BuyBackItemButton);
            _buyButton.onClick.AddListener(OnClick_BuyItemButton);
            _closePerksPanelButton.onClick.AddListener(OnClick_ClosePerksButton);
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
        }

        private void Awake()
        {
            _data = Data.HubMapData;

            HubMapUIServices.SharedInstance.InitializeServices();
            _context = new HubMapUIContext(_data.ContextData);
            _questController = new HubMapUIQuestController(_context);

            _player = _context.Player;
            _generalInventory = _player.Inventory;

            _rightInfoPanelObjectsForDestroy = new List<GameObject>();
            _displayedCurrentCitizensUIBehaviours = new Dictionary<HubMapUICitizenModel, HubMapUICitizenBehaviour>();
            _displayedDialogAnswerButtons = new List<GameObject>();

            _characterBackpuckSlotsUIBehaviours = new List<HubMapUIStorageSlotBehaviour>();
            _generalInventorySlotsUIBehaviours = new List<HubMapUIStorageSlotBehaviour>();
            _buyBackSlotsUIBehaviours = new List<HubMapUIStorageSlotBehaviour>();
            _shopSlotsUIBehaviours = new List<HubMapUIShopSlotBehaviour>();
            _characterClothSlotsUIBehaviours = _characterClothPanel.GetComponentsInChildren<HubMapUIEquipmentSlotBehaviour>();
            _characterPocketsSlotsBehaviours = new List<HubMapUIEquipmentSlotBehaviour>();

            _character3DViewModelRawImageBehaviour = _character3DViewModelRawImage.GetComponent<HubMapUIView3DModelBehaviour>();

            _selected = new SelectedElements();
        }

        private void Start()
        {
            if (_characterClothSlotsUIBehaviours.Length != _context.CharactersClothEquipment.Length)
            {
                Debug.LogError("The number of cloth UI slots does not match the equipment of the characters!");
            }

            for (int i = 0; i < _characterClothSlotsUIBehaviours.Length; i++)
            {
                FillCharacterClothesSlot(i);
            }

            for (int i = 0; i < _mapObjects.Length; i++)
            {
                FillMapObject(_mapObjects[i], _data.MapObjects[i]);
            }

            for (int i = 0; i < _context.CharactersEquipmentSlotAmount; i++)
            {
                InitializeCharacterBackpuckSlotUI(i);
            }

            for (int i = 0; i < _generalInventory.GetSlotsCount(); i++)
            {
                InitializeGeneralInventorySlotUI(i);
            }

            for (int i = 0; i < _context.ShopsSlotsAmount; i++)
            {
                InitializeBuyBackSlotUI(i);
            }

            for (int i = 0; i < _context.ShopsSlotsAmount; i++)
            {
                InitializeShopSlotUI(i);
            }

            _character3DViewModelRendering =
                Instantiate(_data.Characters3DViewRenderingPrefab, _data.Characters3DViewRenderingObjectPosition, Quaternion.identity);
            for (int i = 0; i < _context.Characters.Count; i++)
            {
                InitializeCharacterUI(_context.Characters[i]);
            }

            FillItemStorageSlots(HubMapUIItemStorageType.GeneralInventory);
            _generalInventory.OnPutItemToSlotHandler += FillSlotUI;
            _generalInventory.OnTakeItemFromSlotHandler += FillSlotUI;

            _selected.OnChanged_GeneralInventorySlotIndex = OnChangedSelectedInventorySlot;
            _selected.OnChanged_BuyBackSlotIndex = OnChangedSelectedBuyBackSlot;
            _selected.OnChanged_ShopSlotIndex = OnChangedSelectedShopSlot;
            _selected.OnChanged_Character = OnChangedSelectedCharacter;
            _selected.OnChanged_MapObject = OnChangedSelectedMapObject;

            _character3DViewModelRawImageBehaviour.OnDropHandler += OnDropItemOn3DViewModelRawImage;

            _mainPanel.SetActive(_data.MapOnStartEnabled);
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


        #region Methods


        #region EventMethods(OnClick, OnDrag etc.)

        private void OnClick_MapButton()
        {
            _mainPanel.SetActive(true);
        }

        private void OnClick_HikePanelButton()
        {
            SetScrollViewParentForPanel(_inventoryItemsPanel, _hikeInventoryScrollView);

            _travelTimeText.text = HubMapUIServices.SharedInstance.TravelTimeService.
                GetFullPhraseAboutTravelTime(_selected.MapObject as HubMapUILocationModel);
            _hikePanel.SetActive(true);
        }

        private void OnClick_HubButton()
        {
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

        private void OnClick_CityButton(HubMapUIMapObjectModel mapObjectModel)
        {
            _selected.MapObject = mapObjectModel;
            HubMapUICityModel cityModel = mapObjectModel as HubMapUICityModel;

            HideRightInfoPanels();
            ClearRightInfoPanel();
            FillCityPanel(cityModel);

            _infoPanel.GetComponent<ScrollRect>().content = _cityInfoPanel.GetComponent<RectTransform>();
            _infoPanel.SetActive(true);

            _cityInfoPanel.SetActive(true);
        }

        private void OnClick_LocationButton(HubMapUIMapObjectModel mapObjectModel)
        {
            _selected.MapObject = mapObjectModel;
            HubMapUILocationModel locationModel = mapObjectModel as HubMapUILocationModel;

            HideRightInfoPanels();
            ClearRightInfoPanel();
            FillLocationPanel(locationModel);

            _infoPanel.GetComponent<ScrollRect>().content = _locationInfoPanel.GetComponent<RectTransform>();
            _infoPanel.SetActive(true);

            _locationInfoPanel.SetActive(true);
        }

        private void OnClick_OpenTradePanelButton()
        {
            HubMapUICityModel city = _selected.MapObject as HubMapUICityModel;

            SetScrollViewParentForPanel(_inventoryItemsPanel, _shopInventoryScrollView);
            _playerGoldAmount.text = _player.GoldAmount.ToString();
            _shopCityReputation.text = city.PlayerReputation.ToString();

            _currentShopStorage = city.ShopStorage;
            FillItemStorageSlots(HubMapUIItemStorageType.ShopStorage);
            _currentShopStorage.OnPutItemToSlotHandler += FillSlotUI;
            _currentShopStorage.OnTakeItemFromSlotHandler += FillSlotUI;

            _currentBuyBackStorage = city.BuyBackStorage;
            FillItemStorageSlots(HubMapUIItemStorageType.BuyBackStorage);
            _currentBuyBackStorage.OnPutItemToSlotHandler += FillSlotUI;
            _currentBuyBackStorage.OnTakeItemFromSlotHandler += FillSlotUI;

            _tradePanel.SetActive(true);
        }

        private void OnClick_CloseTradePanelButton()
        {
            _selected.GeneralInventorySlotIndex = null;
            _selected.ShopSlotIndex = null;
            _selected.BuyBackSlotIndex = null;

            _tradePanel.SetActive(false);

            _currentBuyBackStorage.Clear();
            _currentBuyBackStorage.OnPutItemToSlotHandler -= FillSlotUI;
            _currentBuyBackStorage.OnTakeItemFromSlotHandler -= FillSlotUI;
            _currentBuyBackStorage = null;

            _currentShopStorage.OnPutItemToSlotHandler -= FillSlotUI;
            _currentShopStorage.OnTakeItemFromSlotHandler -= FillSlotUI;
            _currentShopStorage = null;
        }

        private void OnClick_CitizenButton(HubMapUICitizenModel citizen)
        {
            FillDialogPanel(citizen);
            _dialogPanel.SetActive(true);
        }

        private void OnClick_CharactersPanelNavigationButton(float step)
        {
            _charactersPanelScrollbar.value += step;
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

        private void OnClick_CharacterButton(HubMapUICharacterModel character)
        {
            _selected.Character = character;
        }

        private void OnPointerDown_InventorySlot(int slotIndex)
        {
            _selected.GeneralInventorySlotIndex = slotIndex;
        }

        private void OnPointerDown_BuyBackSlot(int slotIndex)
        {
            _selected.BuyBackSlotIndex = slotIndex;
        }

        private void OnPointerDown_ShopSlot(int slotIndex)
        {
            _selected.ShopSlotIndex = slotIndex;
        }

        private void OnDoubleClick_InventorySlot(int slotIndex)
        {
            if (_hikePanel.activeSelf && _selected.Character != null)
            {
                MoveItemToClothesEquipment(_generalInventory, slotIndex);
            }
        }

        private void OnDoubleClick_CharacterBackpuckSlot(int slotIndex)
        {
            if (_selected.Character != null)
            {
                MoveItemToClothesEquipment(_selected.Character.Backpack, slotIndex);
            }
        }

        private void OnClick_AnswerButton(HubMapUICitizenModel citizen, HubMapUIDialogAnswer dialogAnswer)
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
                HubMapUIBaseItemModel sellingItem = _generalInventory.GetItemBySlot(_selected.GeneralInventorySlotIndex.Value);

                if (sellingItem != null)
                {
                    if (_currentBuyBackStorage.PutItemToFirstEmptySlot(sellingItem))
                    {
                        _generalInventory.RemoveItem(_selected.GeneralInventorySlotIndex.Value);
                        ChangePlayerGoldAmount(HubMapUIServices.SharedInstance.ShopService.CountSellPrice(sellingItem));
                    }
                    else
                    {
                        Debug.Log("BuyBack storage is full");
                    }
                }

                _selected.GeneralInventorySlotIndex = null;
            }
        }

        private void OnClick_BuyBackItemButton()
        {
            if (_selected.BuyBackSlotIndex.HasValue)
            {
                HubMapUIBaseItemModel buyingItem = _currentBuyBackStorage.GetItemBySlot(_selected.BuyBackSlotIndex.Value);
                if (buyingItem != null)
                {
                    if (_player.GoldAmount >= HubMapUIServices.SharedInstance.ShopService.CountSellPrice(buyingItem)) 
                    { 
                        if (_generalInventory.PutItemToFirstEmptySlot(buyingItem))
                        {
                            _currentBuyBackStorage.RemoveItem(_selected.BuyBackSlotIndex.Value);
                            ChangePlayerGoldAmount(-HubMapUIServices.SharedInstance.ShopService.CountSellPrice(buyingItem));
                        }
                        else
                        {
                            Debug.Log("Inventory storage is full");
                        }
                    }
                    else
                    {
                        Debug.Log("The player does not have enough gold ");
                    }
                }

                _selected.BuyBackSlotIndex = null;
            }
        }

        private void OnClick_BuyItemButton()
        {
            if (_selected.ShopSlotIndex.HasValue)
            {
                HubMapUIBaseItemModel buyingItem = _currentShopStorage.GetItemBySlot(_selected.ShopSlotIndex.Value);

                if (buyingItem != null)
                {
                    if (IsPossibleToBuyShopItem(buyingItem))
                    {
                        if (_generalInventory.PutItemToFirstEmptySlot(buyingItem))
                        {
                            _currentShopStorage.RemoveItem(_selected.ShopSlotIndex.Value);
                            ChangePlayerGoldAmount(-HubMapUIServices.SharedInstance.ShopService.GetItemPrice(buyingItem));
                        }
                        else
                        {
                            Debug.Log("Inventory storage is full");
                        }
                    }
                    else
                    {
                        Debug.Log("The player does not have enough gold ");
                    }
                }

                _selected.ShopSlotIndex = null;
            }
        }

        private void OnPointerEnter_Slot(int slotIndex, HubMapUIItemStorageType storageType)
        {
            //todo: tooltip not must activate when dragged item
            FillTooltipByItemInfo(slotIndex, storageType);

            RectTransform tooltipRectTransform = _tooltip.GetComponent<RectTransform>();

            if (storageType == HubMapUIItemStorageType.GeneralInventory && _hikePanel.activeSelf)
            {
                tooltipRectTransform.pivot = new Vector2(0, 1);
            }
            else if (storageType == HubMapUIItemStorageType.ShopStorage || storageType == HubMapUIItemStorageType.BuyBackStorage)
            {
                tooltipRectTransform.pivot = new Vector2(1, 0);
            }
            else
            {
                tooltipRectTransform.pivot = new Vector2(0, 0);
            }

            _tooltip.transform.position = Mouse.current.position.ReadValue();
            _tooltip.SetActive(true);
        }


        private void OnPointerExit_Slot(int slotIndex)
        {
            _tooltip.SetActive(false);
        }

         private void OnBeginDragItemFromSlot(int slotIndex, HubMapUIItemStorageType storageType)
        {
            _tooltip.SetActive(false);
            _draggedItemInfo.slotIndex = slotIndex;
            _draggedItemInfo.storageType = storageType;
        }

        private void OnEndDragItem(int slotIndex, HubMapUIItemStorageType storageType)
        {
            FillSlotUI(storageType, slotIndex, GetStorageByType(storageType).GetItemBySlot(slotIndex));
            _draggedItemInfo.slotIndex = null;
        }

        private void OnDropItemToSlot(int dropSlotIndex, HubMapUIItemStorageType dropStorageType)
        {
            if (_draggedItemInfo.slotIndex.HasValue)
            {
                SwapItems
                    (GetStorageByType(dropStorageType), dropSlotIndex,
                    GetStorageByType(_draggedItemInfo.storageType), _draggedItemInfo.slotIndex.Value);

                switch (dropStorageType)
                {
                    case HubMapUIItemStorageType.GeneralInventory:
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
                MoveItemToClothesEquipment(GetStorageByType(_draggedItemInfo.storageType), _draggedItemInfo.slotIndex.Value);
            }
        }

        private void OnChangedSelectedInventorySlot(int? previousSlotIndex)
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
                    if (_generalInventory.GetItemBySlot(_selected.GeneralInventorySlotIndex.Value) != null)
                    {
                        _sellingItemPrice.text = HubMapUIServices.SharedInstance.ShopService.
                            CountSellPrice(_generalInventory.GetItemBySlot(_selected.GeneralInventorySlotIndex.Value)).ToString();
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

        private void OnChangedSelectedBuyBackSlot(int? previousSlotIndex)
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
                    if (_currentBuyBackStorage.GetItemBySlot(_selected.BuyBackSlotIndex.Value) != null)
                    {
                        _buybackItemPrice.text = HubMapUIServices.SharedInstance.ShopService.
                            CountSellPrice(_currentBuyBackStorage.GetItemBySlot(_selected.BuyBackSlotIndex.Value)).ToString();

                        _buyBackButton.interactable = _player.GoldAmount >= HubMapUIServices.SharedInstance.ShopService.
                            CountSellPrice(_currentBuyBackStorage.GetItemBySlot(_selected.BuyBackSlotIndex.Value));
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

        private void OnChangedSelectedShopSlot(int? previousSlotIndex)
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
                    HubMapUIBaseItemModel item = _currentShopStorage.GetItemBySlot(_selected.ShopSlotIndex.Value);
                    if (item != null)
                    {
                        _buyingItemPrice.text = HubMapUIServices.SharedInstance.ShopService.GetItemPrice(item).ToString();
                        _buyButton.interactable = IsPossibleToBuyShopItem(item);
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

        private void OnChangedSelectedCharacter(HubMapUICharacterModel previousCharacter)
        {
            if (previousCharacter != null)
            {
                previousCharacter.Behaviour.SelectFrameSwitch(false);

                previousCharacter.Backpack.OnPutItemToSlotHandler -= FillSlotUI;
                previousCharacter.Backpack.OnTakeItemFromSlotHandler -= FillSlotUI;
                previousCharacter.ClothesEquipment.OnPutItemToSlotHandler -= FillSlotUI;
                previousCharacter.ClothesEquipment.OnTakeItemFromSlotHandler -= FillSlotUI;

                previousCharacter.Pockets.OnPutItemToSlotHandler -= FillSlotUI;
                previousCharacter.Pockets.OnTakeItemFromSlotHandler -= FillSlotUI;
                previousCharacter.Pockets.OnChangeSlotsAmountHandler -= OnChangePocketsSlotsAmount;
                
                DestroyPocketsSlotsUI();

                previousCharacter.View3DModelObjectOnScene.SetActive(false);
                previousCharacter.View3DModelObjectOnScene.transform.rotation = Quaternion.identity;
            }

            if (_selected.Character != null)
            {
                _selected.Character.Behaviour.SelectFrameSwitch(true);
                _hikeAcceptButton.interactable = true;

                FillItemStorageSlots(HubMapUIItemStorageType.CharacterBackpuck);
                SetStorageSlotsInteractable(true, _characterBackpuckSlotsUIBehaviours);
                _selected.Character.Backpack.OnPutItemToSlotHandler += FillSlotUI;
                _selected.Character.Backpack.OnTakeItemFromSlotHandler += FillSlotUI;

                FillItemStorageSlots(HubMapUIItemStorageType.CharacterClothEquipment);
                SetStorageSlotsInteractable(true, _characterClothSlotsUIBehaviours);
                _selected.Character.ClothesEquipment.OnPutItemToSlotHandler += FillSlotUI;
                _selected.Character.ClothesEquipment.OnTakeItemFromSlotHandler += FillSlotUI;

                InitializePocketsSlotsUI(_selected.Character.Pockets.GetSlotsCount());
                _selected.Character.Pockets.OnPutItemToSlotHandler += FillSlotUI;
                _selected.Character.Pockets.OnTakeItemFromSlotHandler += FillSlotUI;
                _selected.Character.Pockets.OnChangeSlotsAmountHandler += OnChangePocketsSlotsAmount;

                _selected.Character.View3DModelObjectOnScene.SetActive(true);
                _character3DViewModelRawImage.enabled = true;
                _character3DViewModelRawImageBehaviour.RotateObject = _selected.Character.View3DModelObjectOnScene;
            }
            else
            {
                _character3DViewModelRawImage.enabled = false;
                _hikeAcceptButton.interactable = false;
                SetStorageSlotsInteractable(false, _characterBackpuckSlotsUIBehaviours);
                SetStorageSlotsInteractable(false, _characterClothSlotsUIBehaviours);
            }
        }

        private void OnChangePocketsSlotsAmount()
        {
            DestroyPocketsSlotsUI();
            InitializePocketsSlotsUI(_selected.Character.Pockets.GetSlotsCount());
        }

        private void OnChangedSelectedMapObject(HubMapUIMapObjectModel previousMapObject)
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


        #region InitializeUIElements

        private void InitializePocketsSlotsUI(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject slotUI = InstantiateUIObject(_data.CharacterBackpuckSlotUIPrefab, _pocketSlotsPanel);
                HubMapUIEquipmentSlotBehaviour slotBehaviour = slotUI.GetComponent<HubMapUIEquipmentSlotBehaviour>();
                slotBehaviour.FillSlotInfo(i, true, _data.PocketItemSlotIcon);
                slotBehaviour.FillSlot(_selected.Character.Pockets.GetItemIconBySlot(i));
                slotBehaviour.OnBeginDragItemHandler = (slotIndex) => OnBeginDragItemFromSlot(slotIndex, HubMapUIItemStorageType.PocketsStorage);
                slotBehaviour.OnDroppedItemHandler = (slotIndex) => OnDropItemToSlot(slotIndex, HubMapUIItemStorageType.PocketsStorage);
                slotBehaviour.OnEndDragItemHandler = (slotIndex) => OnEndDragItem(slotIndex, HubMapUIItemStorageType.PocketsStorage);
                slotBehaviour.OnPointerEnterHandler = (slotIndex) => OnPointerEnter_Slot(slotIndex, HubMapUIItemStorageType.PocketsStorage);
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

        private void InitializeCharacterUI(HubMapUICharacterModel character)
        {
            GameObject characterUI = InstantiateUIObject(_data.CharacterUIPrefab, _charactersPanelFillable);
            HubMapUICharacterBehaviour behaviourUI = characterUI.GetComponentInChildren<HubMapUICharacterBehaviour>();
            behaviourUI.Initialize(character);
            behaviourUI.OnClick_ButtonHandler += OnClick_CharacterButton;

            character.InitializeView3DModel(_character3DViewModelRendering.transform);
        }

        private void InitializeCharacterBackpuckSlotUI(int slotIndex)
        {
            GameObject equipSlotUI = InstantiateUIObject(_data.EquipmentSlotUIPrefab, _characterInventoryPanel);

            HubMapUIStorageSlotBehaviour slotBehaviour = equipSlotUI.GetComponent<HubMapUIStorageSlotBehaviour>();
            slotBehaviour.FillSlotInfo(slotIndex, true);
            slotBehaviour.SetInteractable(false);
            slotBehaviour.OnDoubleClickButtonHandler = OnDoubleClick_CharacterBackpuckSlot;
            slotBehaviour.OnBeginDragItemHandler = (slotIndex) => OnBeginDragItemFromSlot(slotIndex, HubMapUIItemStorageType.CharacterBackpuck);
            slotBehaviour.OnDroppedItemHandler = (slotIndex) => OnDropItemToSlot(slotIndex, HubMapUIItemStorageType.CharacterBackpuck);
            slotBehaviour.OnEndDragItemHandler = (slotIndex) => OnEndDragItem(slotIndex, HubMapUIItemStorageType.CharacterBackpuck);
            slotBehaviour.OnPointerEnterHandler = (slotIndex) => OnPointerEnter_Slot(slotIndex, HubMapUIItemStorageType.CharacterBackpuck);
            slotBehaviour.OnPointerExitHandler = OnPointerExit_Slot;

            _characterBackpuckSlotsUIBehaviours.Add(slotBehaviour);
        }

        private void InitializeGeneralInventorySlotUI(int slotIndex)
        {
            GameObject inventorySlotUI = InstantiateUIObject(_data.InventorySlotUIPrefab, _inventoryItemsPanel);

            HubMapUIStorageSlotBehaviour slotBehaviour = inventorySlotUI.GetComponent<HubMapUIStorageSlotBehaviour>();
            slotBehaviour.FillSlotInfo(slotIndex, true);
            slotBehaviour.OnPointerDownHandler = OnPointerDown_InventorySlot;
            slotBehaviour.OnDoubleClickButtonHandler = OnDoubleClick_InventorySlot;
            slotBehaviour.OnBeginDragItemHandler = (slotIndex) => OnBeginDragItemFromSlot(slotIndex, HubMapUIItemStorageType.GeneralInventory);
            slotBehaviour.OnDroppedItemHandler = (slotIndex) => OnDropItemToSlot(slotIndex, HubMapUIItemStorageType.GeneralInventory);
            slotBehaviour.OnEndDragItemHandler = (slotIndex) => OnEndDragItem(slotIndex, HubMapUIItemStorageType.GeneralInventory);
            slotBehaviour.OnPointerEnterHandler = (slotIndex) => OnPointerEnter_Slot(slotIndex, HubMapUIItemStorageType.GeneralInventory);
            slotBehaviour.OnPointerExitHandler = OnPointerExit_Slot;

            _generalInventorySlotsUIBehaviours.Add(slotBehaviour);
        }

        private void InitializeBuyBackSlotUI(int slotIndex)
        {
            GameObject buyBackSlotUI = InstantiateUIObject(_data.ShopSlotUIPrefab, _buyBackItemsPanel);

            HubMapUIStorageSlotBehaviour slotBehaviour = buyBackSlotUI.GetComponent<HubMapUIStorageSlotBehaviour>();
            slotBehaviour.FillSlotInfo(slotIndex, false);
            slotBehaviour.SetInteractable(false);
            slotBehaviour.OnPointerDownHandler = OnPointerDown_BuyBackSlot;
            slotBehaviour.OnPointerEnterHandler = (slotIndex) => OnPointerEnter_Slot(slotIndex, HubMapUIItemStorageType.BuyBackStorage);
            slotBehaviour.OnPointerExitHandler = OnPointerExit_Slot;

            _buyBackSlotsUIBehaviours.Add(slotBehaviour);
        }

        private void InitializeShopSlotUI(int slotIndex)
        {
            GameObject shopSlotUI = InstantiateUIObject(_data.ShopSlotUIPrefab, _buyingItemsPanel);

            HubMapUIShopSlotBehaviour slotBehaviour = shopSlotUI.GetComponent<HubMapUIShopSlotBehaviour>();
            slotBehaviour.FillSlotInfo(slotIndex, false);
            slotBehaviour.SetInteractable(false);
            slotBehaviour.OnPointerDownHandler = OnPointerDown_ShopSlot;
            slotBehaviour.OnPointerEnterHandler = (slotIndex) => OnPointerEnter_Slot(slotIndex, HubMapUIItemStorageType.ShopStorage);
            slotBehaviour.OnPointerExitHandler = OnPointerExit_Slot;

            _shopSlotsUIBehaviours.Add(slotBehaviour);
        }

        private void InitializeCitizenUI(HubMapUICitizenModel citizen)
        {
            GameObject citizenUI = InstantiateUIObject(_data.CitizenUIPrefab, _citizenPanel);

            HubMapUICitizenBehaviour citizenUIBehaviour = citizenUI.GetComponentInChildren<HubMapUICitizenBehaviour>();
            citizenUIBehaviour.FillCitizenInfo(citizen);
            citizenUIBehaviour.OnClick_CitizenButtonHandler = OnClick_CitizenButton;

            citizen.OnChangeQuestMarkerTypeHandler += citizenUIBehaviour.SetQuestMarker;

            _rightInfoPanelObjectsForDestroy.Add(citizenUI);
            _displayedCurrentCitizensUIBehaviours.Add(citizen, citizenUIBehaviour);
        }

        private void InitializeDialogAnswerButton(HubMapUICitizenModel citizen, HubMapUIDialogAnswer answer)
        {
            GameObject answerButton = InstantiateUIObject(_data.AnswerButtonUIPrefab, _answerButtonsPanel);
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
            Sprite slotSprite = Data.HubMapData.GetClothSlotSpriteByType(_context.CharactersClothEquipment[slotIndex]);
            _characterClothSlotsUIBehaviours[slotIndex].FillSlotInfo(slotIndex, true, slotSprite);
            _characterClothSlotsUIBehaviours[slotIndex].SetInteractable(false);
            _characterClothSlotsUIBehaviours[slotIndex].OnBeginDragItemHandler += (slotIndex) => OnBeginDragItemFromSlot(slotIndex, HubMapUIItemStorageType.CharacterClothEquipment);
            _characterClothSlotsUIBehaviours[slotIndex].OnEndDragItemHandler += (slotIndex) => OnEndDragItem(slotIndex, HubMapUIItemStorageType.CharacterClothEquipment);
            _characterClothSlotsUIBehaviours[slotIndex].OnDroppedItemHandler += (slotIndex) => OnDropItemToSlot(slotIndex, HubMapUIItemStorageType.CharacterClothEquipment);
            _characterClothSlotsUIBehaviours[slotIndex].OnPointerEnterHandler += (slotIndex) => OnPointerEnter_Slot(slotIndex, HubMapUIItemStorageType.CharacterClothEquipment);
            _characterClothSlotsUIBehaviours[slotIndex].OnPointerExitHandler += OnPointerExit_Slot;
        }

        private void FillMapObject(GameObject mapObject, HubMapUIMapObjectData mapObjectdata)
        {
            HubMapUIMapObjectModel mapObjectModel = _context.GetMapObjectModel(mapObjectdata);

            if (mapObject != null)
            {
                HubMapUIMapObjectBehaviour mapObjectBehaviour = mapObject.GetComponent<HubMapUIMapObjectBehaviour>();
                mapObjectModel.Behaviour = mapObjectBehaviour;

                switch (mapObjectdata.GetMapObjectType())
                {
                    case HubMapUIMapObjectType.Location:

                        mapObjectBehaviour.FillInfo(mapObjectModel as HubMapUILocationModel);
                        mapObjectBehaviour.OnClick_ButtonHandler += OnClick_LocationButton;

                        break;
                    case HubMapUIMapObjectType.City:

                        mapObjectBehaviour.FillInfo(mapObjectModel as HubMapUICityModel);
                        mapObjectBehaviour.OnClick_ButtonHandler += OnClick_CityButton;

                        break;
                    default:

                        Debug.LogError(this + " incorrect HubMapUIMapObjectType value");

                        break;
                }
            }
            else
            {
                Debug.LogError(this + " HubMapUIContext not contain requested HubMapUIMapObjectModel: " + mapObjectModel.Name);
            }
        }

        private void FillTooltipByItemInfo(int slotIndex, HubMapUIItemStorageType storageType)
        {
            HubMapUIBaseItemModel item = GetStorageByType(storageType).GetItemBySlot(slotIndex);

            if (item != null)
            {
                Text[] tooltipTexts = _tooltip.GetComponentsInChildren<Text>(true);

                tooltipTexts[0].gameObject.SetActive(false);
                tooltipTexts[1].text = item.Name;
                tooltipTexts[2].text = item.Description;
                tooltipTexts[3].text = "Цена: " + HubMapUIServices.SharedInstance.ShopService.GetItemPrice(item).ToString();

                if (storageType == HubMapUIItemStorageType.ShopStorage)
                {
                    if (!IsPossibleToBuyShopItem(item, out string message))
                    {
                        tooltipTexts[0].gameObject.SetActive(true);
                        tooltipTexts[0].text = message;
                    }
                }
            }
        }

        private void FillSlotUI(HubMapUIItemStorageType storageType, int slotIndex, HubMapUIBaseItemModel item)
        {
            HubMapUIBaseItemStorage storage = GetStorageByType(storageType);
            Sprite sprite = storage.GetItemIconBySlot(slotIndex);

            switch (storageType)
            {
                case HubMapUIItemStorageType.CharacterBackpuck:

                    _characterBackpuckSlotsUIBehaviours[slotIndex].FillSlot(sprite);

                    break;
                case HubMapUIItemStorageType.GeneralInventory:

                    _generalInventorySlotsUIBehaviours[slotIndex].FillSlot(sprite);

                    break;
                case HubMapUIItemStorageType.BuyBackStorage:

                    _buyBackSlotsUIBehaviours[slotIndex].FillSlot(sprite);
                    _buyBackSlotsUIBehaviours[slotIndex].SetInteractable(sprite != null);

                    break;
                case HubMapUIItemStorageType.ShopStorage:

                    _shopSlotsUIBehaviours[slotIndex].FillSlot(sprite);
                    _shopSlotsUIBehaviours[slotIndex].SetInteractable(sprite != null);
                    _shopSlotsUIBehaviours[slotIndex].SetAvailability(IsPossibleToBuyShopItem(storage.GetItemBySlot(slotIndex)));

                    break;
                case HubMapUIItemStorageType.CharacterClothEquipment:

                    _characterClothSlotsUIBehaviours[slotIndex].FillSlot(sprite);

                    break;
                case HubMapUIItemStorageType.PocketsStorage:

                    _characterPocketsSlotsBehaviours[slotIndex].FillSlot(sprite);

                    break;
                default:

                    Debug.LogError(this + ": incorrect StorageSlotType");

                    break;
            }
        }

        private void FillItemStorageSlots(HubMapUIItemStorageType storageType)
        {
            HubMapUIBaseItemStorage storage = GetStorageByType(storageType);

            for (int i = 0; i < storage.GetSlotsCount(); i++)
            {
                FillSlotUI(storageType, i, storage.GetItemBySlot(i));
            }
        }

        private void FillCityPanel(HubMapUICityModel city)
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

        private void FillLocationPanel(HubMapUILocationModel location)
        {
            _locationScreen.sprite = location.Screenshot;
            _locationName.text = location.Name;
            _locationDescription.text = location.Description;

            if (location.Dwellers.Length > 0)
            {
                _dwellersPanel.transform.GetChild(0).gameObject.SetActive(false);

                for (int i = 0; i < location.Dwellers.Length; i++)
                {
                    GameObject dwellerUI = InstantiateUIObject(_data.LocationTextUIPrefab, _dwellersPanel);
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
                    GameObject ingredientUI = InstantiateUIObject(_data.LocationTextUIPrefab, _ingredientsPanel);
                    _rightInfoPanelObjectsForDestroy.Add(ingredientUI);
                    ingredientUI.GetComponentInChildren<Text>().text = location.Ingredients[i].Name;
                }
            }
            else
            {
                _ingredientsPanel.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        private void FillDialogPanel(HubMapUICitizenModel citizen)
        {
            _citizenName.text = citizen.Name;
            _citizenPortrait.sprite = citizen.Portrait;
            _dialogText.text = citizen.CurrentDialog.Text;

            List<HubMapUIDialogAnswer> answers = citizen.GetAllCurrentAnswers();
            for (int i = 0; i < answers.Count; i++)
            {
                InitializeDialogAnswerButton(citizen, answers[i]);
            }
        }

        #endregion


        private void HideRightInfoPanels()
        {
            _cityInfoPanel.SetActive(false);
            _locationInfoPanel.SetActive(false);
            _infoPanel.SetActive(false);
        }

        private void ClearRightInfoPanel()
        {
            foreach (KeyValuePair<HubMapUICitizenModel, HubMapUICitizenBehaviour> kvp in _displayedCurrentCitizensUIBehaviours)
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

        private void MoveItemToClothesEquipment(HubMapUIBaseItemStorage outStorage, int outStorageSlotIndex)
        {
            HubMapUIBaseItemModel itemInClickedSlot = outStorage.GetItemBySlot(outStorageSlotIndex);
            if (itemInClickedSlot != null &&
                itemInClickedSlot.ItemType == HubMapUIItemType.Cloth &&
                _selected.Character != null)
            {
                HubMapUIClothesEquipmentStorage clothesEquipmentStorage = _selected.Character.ClothesEquipment;
                if (!clothesEquipmentStorage.PutItemToFirstEmptySlot(itemInClickedSlot))
                {
                    int? firstSlot = clothesEquipmentStorage.GetFirstSlotIndexForItem(itemInClickedSlot as HubMapUIClothesItemModel);
                    SwapItems(clothesEquipmentStorage, firstSlot.Value, outStorage, outStorageSlotIndex);
                }
                else
                {
                    outStorage.RemoveItem(outStorageSlotIndex);
                }

            }
        }

        private void SetStorageSlotsInteractable<T>(bool flag, IEnumerable<T> slotBehaviours) where T : HubMapUIBaseSlotBehaviour
        {
            foreach (HubMapUIBaseSlotBehaviour slot in slotBehaviours)
            {
                slot.SetInteractable(flag);
            }
        }

        private HubMapUIBaseItemStorage GetStorageByType(HubMapUIItemStorageType storageType)
        {
            HubMapUIBaseItemStorage storage = null;
            switch (storageType)
            {
                case HubMapUIItemStorageType.CharacterBackpuck:
                    storage = _selected.Character.Backpack;
                    break;

                case HubMapUIItemStorageType.GeneralInventory:
                    storage = _generalInventory;
                    break;

                case HubMapUIItemStorageType.BuyBackStorage:
                    storage = _currentBuyBackStorage;
                    break;

                case HubMapUIItemStorageType.ShopStorage:
                    storage = _currentShopStorage;
                    break;

                case HubMapUIItemStorageType.CharacterClothEquipment:
                    storage = _selected.Character.ClothesEquipment;
                    break;

                case HubMapUIItemStorageType.PocketsStorage:
                    storage = _selected.Character.Pockets;
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

        private void SwapItems(HubMapUIBaseItemStorage inStorage, int inStorageSlotIndex, HubMapUIBaseItemStorage outStorage, int outStorageSlotIndex)
        {
            HubMapUIBaseItemModel outStorageItem = outStorage.GetItemBySlot(outStorageSlotIndex);
            HubMapUIBaseItemModel inStorageItem = inStorage.GetItemBySlot(inStorageSlotIndex);
            bool isSuccefulTakeItems = false;
            bool isSuccefullPutItems = false;


            if (inStorage.RemoveItem(inStorageSlotIndex))
            {
                if (outStorage.RemoveItem(outStorageSlotIndex))
                {
                    isSuccefulTakeItems = true;
                }
                else
                {
                    inStorage.PutItem(inStorageSlotIndex, inStorageItem);
                }
            }

            if (isSuccefulTakeItems)
            {
                isSuccefullPutItems =
                    inStorage.PutItem(inStorageSlotIndex, outStorageItem) &&
                    outStorage.PutItem(outStorageSlotIndex, inStorageItem);

                if (!isSuccefullPutItems)
                {
                    inStorage.RemoveItem(inStorageSlotIndex);
                    inStorage.PutItem(inStorageSlotIndex, inStorageItem);

                    outStorage.RemoveItem(outStorageSlotIndex);
                    outStorage.PutItem(outStorageSlotIndex, outStorageItem);
                }
            }
           
            if (!(isSuccefulTakeItems && isSuccefullPutItems))
            {
                Debug.LogWarning("Drag and drop swap items operation was not successful");
            }
        }

        private void ChangePlayerGoldAmount(int goldAmount)
        {
            _player.GoldAmount += goldAmount;
            _playerGoldAmount.text = _player.GoldAmount.ToString();

            for (int i = 0; i < _shopSlotsUIBehaviours.Count; i++)
            {
                _shopSlotsUIBehaviours[i].SetAvailability(IsPossibleToBuyShopItem(_currentShopStorage.GetItemBySlot(i)));
            }
        }

        private bool IsPossibleToBuyShopItem(HubMapUIBaseItemModel item)
        {
            if (item != null)
            {
                return
                    item.RequiredReputationForSaleInShop <= (_selected.MapObject as HubMapUICityModel).PlayerReputation &&
                    _player.GoldAmount >= HubMapUIServices.SharedInstance.ShopService.GetItemPrice(item);
            }
            else
            {
                return true;
            }
        }

        private bool IsPossibleToBuyShopItem(HubMapUIBaseItemModel item, out string message)
        {
            bool flag = true;
            StringBuilder sb = new StringBuilder();

            if (item != null)
            {
                if ((_selected.MapObject as HubMapUICityModel).PlayerReputation < item.RequiredReputationForSaleInShop)
                {
                    flag = false;

                    sb.AppendFormat("Недостаточно репутации для покупки");
                    sb.AppendLine();
                    sb.AppendFormat($"Необходимая репутация: {item.RequiredReputationForSaleInShop}");
                }
                if (_player.GoldAmount < HubMapUIServices.SharedInstance.ShopService.GetItemPrice(item))
                {
                    flag = false;

                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                    }
                    sb.AppendFormat("Недостаточно денег");
                }
            }

            message = sb.ToString();
            return flag;
        }

         private void LocationLoad()
        {
            Debug.Log("Load location. ID: " + (_selected.MapObject as HubMapUILocationModel).LoadSceneId);
            SceneManager.LoadScene((_selected.MapObject as HubMapUILocationModel).LoadSceneId);
        }

        #endregion

    }
}
