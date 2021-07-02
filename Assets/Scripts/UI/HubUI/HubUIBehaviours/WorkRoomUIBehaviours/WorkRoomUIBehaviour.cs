using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    class WorkRoomUIBehaviour : MonoBehaviour, IStart, IDestroy
    {
        #region Fields

        [SerializeField] private GameObject _roomButtonsPanel;
        [SerializeField] private GameObject _roomButtonsFillablePanel;
        [SerializeField] private GameObject _roomPanel;
        [SerializeField] private Text _roomNameText;
        [SerializeField] private Text _roomLevelText;
        [SerializeField] private Button _roomUpgradeButton;
        [SerializeField] private Button _roomCloseButton;
        [SerializeField] private WorkRoomWorkerSlotBehaviour _chiefSlotBehaviour;
        [SerializeField] private Image _chiefSkillLevelImage;
        [SerializeField] private Text _chiefSkillLevelText;
        [SerializeField] private GameObject _assistantsSlotsFillablePanel;
        [SerializeField] private Image _assistantsSkillLevelImage;
        [SerializeField] private Text _assistantsSkillLevelText;
        [SerializeField] private GameObject _ordersSlotsFillablePanel;
        [SerializeField] private GameObject _charactersFillablePanel;
        [SerializeField] private Button _createOrderButton;
        [SerializeField] private Button _takeMakedItemsButton;
        [SerializeField] private WorkRoomCharacterPanelBehaviour _characterPanelBehaviour;

        [Header("Upgrade room panel")]
        [SerializeField] private GameObject _upgradeRoomPanel;
        [SerializeField] private Button _upgradeRoomApplyButton;
        [SerializeField] private Button _upgradeRoomCancelButton;

        [Space]
        //Todo: remove after the implementation of the recipe selection panel
        [SerializeField] private ItemRecipeSO _recipeForDebug; //TEMPORARY


        private HubUIContext _context;
        private HubUIData _data;
        private (int? slotIndex, CharacterStorageType storageType) _draggedCharacterInfo;
        WorkRoomModel _selectedRoom;
        List<WorkRoomWorkerSlotBehaviour> _assistantsSlotsBehaviours;
        List<WorkRoomOrderSlotBehaviour> _orderSlotsBehaviours;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _roomCloseButton.onClick.AddListener(OnClick_CloseRoomButton);
            _roomUpgradeButton.onClick.AddListener(OnClick_UpgradeRoomButton);
            _createOrderButton.onClick.AddListener(OnClick_CreateOrderButton);
            _takeMakedItemsButton.onClick.AddListener(OnClick_TakeMakedItemsButton);
            _upgradeRoomCancelButton.onClick.AddListener(OnClick_CloseUpgradeRoomButton);
        }

        private void OnDisable()
        {
            _roomCloseButton.onClick.RemoveAllListeners();
            _roomUpgradeButton.onClick.RemoveAllListeners();
            _createOrderButton.onClick.RemoveAllListeners();
            _takeMakedItemsButton.onClick.RemoveAllListeners();
            _upgradeRoomCancelButton.onClick.RemoveAllListeners();
        }

        #endregion


        #region IStart

        public void Starting(HubUIContext context)
        {
            _context = context;
            _data = BeastHunter.Data.HubUIData;

            _assistantsSlotsBehaviours = new List<WorkRoomWorkerSlotBehaviour>();
            _orderSlotsBehaviours = new List<WorkRoomOrderSlotBehaviour>();

            for (int i = 0; i < _context.WorkRooms.Count; i++)
            {
                InitializeWorkRoomButton(_context.WorkRooms[i]);
            }

            _chiefSlotBehaviour.Initialize(CharacterStorageType.ChiefWorkplace, 0);
            _chiefSlotBehaviour.OnBeginDragHandler += OnBeginDragCharacterFromSlot;
            _chiefSlotBehaviour.OnEndDragHandler += OnEndDragCharacter;
            _chiefSlotBehaviour.OnDropHandler += OnDropCharacterToSlot;

            for (int i = 0; i < _context.Player.AvailableCharacters.GetSlotsCount(); i++)
            {
                InitializeCharacterListItemUI(i, _context.Player.AvailableCharacters.GetElementBySlot(i));
            }
            _context.Player.AvailableCharacters.OnAddCharacterHandler += InitializeCharacterListItemUI;
            _context.Player.AvailableCharacters.OnRemoveCharacterHandler += RemoveCharacterListItemUI;
            _context.Player.AvailableCharacters.OnReplaceCharacterHandler += UpdateCharacterListItemUI;

            _characterPanelBehaviour.OnDropHandler += OnDropToCharacterPanel;

            gameObject.SetActive(true);
            _roomButtonsFillablePanel.SetActive(true);
            _roomPanel.SetActive(false);
            _upgradeRoomPanel.SetActive(false);
        }

        #endregion


        #region IDestroy

        public void Destroying()
        {
            _context.Player.AvailableCharacters.OnAddCharacterHandler -= InitializeCharacterListItemUI;
            _context.Player.AvailableCharacters.OnRemoveCharacterHandler -= RemoveCharacterListItemUI;
            _context.Player.AvailableCharacters.OnReplaceCharacterHandler -= UpdateCharacterListItemUI;
        }

        #endregion


        #region Methods

        #region PublicMethods

        public void ShowRoomButtonsPanel()
        {
            _roomButtonsPanel.SetActive(true);
        }

        public void HideRoomButtonsPanel()
        {
            _roomButtonsPanel.SetActive(false);
        }

        #endregion

        #region InstantiateAndDestroyMethods

        private void InitializeWorkRoomButton(WorkRoomModel model)
        {
            GameObject buttonUI = InstantiateUIObject(_data.WorkRoomData.WorkRoomButtonPrefab, _roomButtonsFillablePanel);
            WorkRoomButtonBehaviour buttonBehaviour = buttonUI.GetComponentInChildren<WorkRoomButtonBehaviour>();
            buttonBehaviour.Initialize(model);
            buttonBehaviour.OnClickButtonHandler += OnClick_RoomButton;
            model.OnChangeMinOrderCompleteTimeHandler += buttonBehaviour.UpdateOrderTime;
        }

        private void InitializeAssistantSlotUI(int slotIndex)
        {
            GameObject slotUI = InstantiateUIObject(_data.WorkRoomData.WorkerSlotPrefab, _assistantsSlotsFillablePanel);
            WorkRoomWorkerSlotBehaviour slotBehaviour = slotUI.GetComponent<WorkRoomWorkerSlotBehaviour>();
            _assistantsSlotsBehaviours.Add(slotBehaviour);
            slotBehaviour.Initialize(CharacterStorageType.AssistantWorkplaces, slotIndex);
            slotBehaviour.OnBeginDragHandler += OnBeginDragCharacterFromSlot;
            slotBehaviour.OnEndDragHandler += OnEndDragCharacter;
            slotBehaviour.OnDropHandler += OnDropCharacterToSlot;
            _selectedRoom.AssistantWorkplaces.OnPutElementToSlotHandler += FillCharacterSlotUI;
            _selectedRoom.AssistantWorkplaces.OnTakeElementFromSlotHandler += FillCharacterSlotUI;
        }

        private void InitializeOrderSlotUI(int slotIndex)
        {
            GameObject slotUI = InstantiateUIObject(_data.WorkRoomData.OrderSlotPrefab, _ordersSlotsFillablePanel);
            WorkRoomOrderSlotBehaviour slotBehaviour = slotUI.GetComponentInChildren<WorkRoomOrderSlotBehaviour>();
            _orderSlotsBehaviours.Add(slotBehaviour);
            slotBehaviour.Initialize(OrderStorageType.None, slotIndex);
            slotBehaviour.IsDragAndDropOn = false;
            slotBehaviour.OnClickRemoveOrderButtonHandler += OnClick_RemoveOrderFromSlotButton;
            slotBehaviour.OnClickOpenRecipeBookButtonHandler += OnClick_OrderSlot;
        }

        private void InitializeCharacterListItemUI(int slotIndex, CharacterModel character)
        {
            GameObject characterUI = InstantiateUIObject(_data.WorkRoomData.CharacterListItemPrefab, _charactersFillablePanel);
            AvailableCharacterListItemBehaviour uiBehaviour = characterUI.GetComponent<AvailableCharacterListItemBehaviour>();
            uiBehaviour.Initialize(CharacterStorageType.AvailableCharacters, slotIndex);
            uiBehaviour.UpdateSlot(character);
            uiBehaviour.OnBeginDragHandler += OnBeginDragCharacterFromSlot;
            uiBehaviour.OnEndDragHandler += OnEndDragCharacter;
            uiBehaviour.OnDropHandler += OnDropCharacterToSlot;
            uiBehaviour.IsPointerEnterOnFunc += IsPointerEnterCharacterListItemOn;

            if (_selectedRoom != null)
            {
                uiBehaviour.SetDisplayedSkill(_selectedRoom.UsedSkill, character);
            }
        }

        private void RemoveCharacterListItemUI(int slotIndex)
        {
            DestroyImmediate(GetCharacterListItemBehaviour(slotIndex).gameObject);
        }

        private GameObject InstantiateUIObject(GameObject prefab, GameObject parent)
        {
            GameObject objectUI = GameObject.Instantiate(prefab);
            objectUI.transform.SetParent(parent.transform, false);
            objectUI.transform.localScale = new Vector3(1, 1, 1);
            return objectUI;
        }

        #endregion

        #region OnClickMethods

        private void OnClick_RoomButton(WorkRoomModel model)
        {
            _selectedRoom = model;
            FillRoomPanel(model);
            _roomPanel.SetActive(true);
        }

        private void OnClick_CloseRoomButton()
        {
            _roomPanel.SetActive(false);
            ClearRoomPanel();
            _selectedRoom = null;
        }

        private void OnClick_RemoveOrderFromSlotButton(int slotIndex)
        {
            _selectedRoom.OrdersSlots.RemoveElement(slotIndex);
        }

        private void OnClick_CreateOrderButton()
        {
            if (_selectedRoom.OrdersSlots.HasFreeSlots())
            {
                //todo: open craft window
                //temporary for debug:
                ItemOrderModel order = new ItemOrderModel(_recipeForDebug, _selectedRoom.OrderTimeReducePercent);
                _selectedRoom.OrdersSlots.PutElementToFirstEmptySlot(order);
            }
        }

        private void OnClick_OrderSlot(int slotIndex)
        {
            Debug.Log($"OnClick_OrderSlot({slotIndex})");
            //todo: open recipe book etc
        }

        private void OnClick_TakeMakedItemsButton()
        {
            //todo by design doc (open add window and craft fail chance)
            //temporary:
            for (int i = 0; i < _selectedRoom.OrdersSlots.GetSlotsCount(); i++)
            {
                ItemOrderModel order = _selectedRoom.OrdersSlots.GetElementBySlot(i);
                if (order != null && order.IsCompleted)
                {
                    _context.Player.Inventory.PutElementToFirstEmptySlot(order.MakedItem);
                    HubUIServices.SharedInstance.GameMessages.Notice($"{order.MakedItem.Name} moved to player inventory");
                    _selectedRoom.OrdersSlots.RemoveElement(i);
                }
            }

        }

        private void OnClick_UpgradeRoomButton()
        {
            FillUpgradeRoomPanel();
            _upgradeRoomPanel.SetActive(true);
        }

        private void OnClick_ApplyUpgradeRoomButton()
        {
            _selectedRoom.LevelUp();
            OnClick_CloseUpgradeRoomButton();
        }

        private void OnClick_CloseUpgradeRoomButton()
        {
            _upgradeRoomPanel.SetActive(false);
            _upgradeRoomApplyButton.onClick.RemoveAllListeners();
        }

        #endregion

        #region DragAndDropMethods

        private void OnBeginDragCharacterFromSlot(int slotIndex, CharacterStorageType storageType)
        {
            _draggedCharacterInfo.slotIndex = slotIndex;
            _draggedCharacterInfo.storageType = storageType;
        }

        private void OnEndDragCharacter(int slotIndex, CharacterStorageType storageType)
        {
            if (storageType != CharacterStorageType.AvailableCharacters)
            {
                FillCharacterSlotUI(storageType, slotIndex, GetCharacterStorageByType(storageType).GetElementBySlot(slotIndex));
            }
            _draggedCharacterInfo.slotIndex = null;
        }

        private void OnDropCharacterToSlot(int dropSlotIndex, CharacterStorageType dropStorageType)
        {
            if (_draggedCharacterInfo.slotIndex.HasValue)
            {
                if (dropStorageType == CharacterStorageType.AvailableCharacters)
                {
                    BaseCharacterStorage storageOut = GetCharacterStorageByType(_draggedCharacterInfo.storageType);
                    CharacterModel character = storageOut.GetElementBySlot(_draggedCharacterInfo.slotIndex.Value);

                    if (!(_draggedCharacterInfo.storageType == CharacterStorageType.AvailableCharacters && _draggedCharacterInfo.slotIndex < dropSlotIndex))
                    {
                        dropSlotIndex++;
                    }

                    if (storageOut.RemoveElement(_draggedCharacterInfo.slotIndex.Value))
                    {
                        GetCharacterStorageByType(dropStorageType).PutElement(dropSlotIndex, character);
                    }
                    else
                    {
                        Debug.LogError($"The remove element from {_draggedCharacterInfo.storageType} is not succeful");
                    }
                }
                else
                {
                    GetCharacterStorageByType(dropStorageType).SwapElementsWithOtherStorage(dropSlotIndex,
                    GetCharacterStorageByType(_draggedCharacterInfo.storageType), _draggedCharacterInfo.slotIndex.Value);
                }
                _draggedCharacterInfo.slotIndex = null;
            }
        }

        private void OnDropToCharacterPanel()
        {
            if (_draggedCharacterInfo.slotIndex.HasValue)
            {
                GetCharacterStorageByType(CharacterStorageType.AvailableCharacters).
                    PutElementToFirstEmptySlotFromOtherStorage(GetCharacterStorageByType(_draggedCharacterInfo.storageType), _draggedCharacterInfo.slotIndex.Value);
                _draggedCharacterInfo.slotIndex = null;
            }
        }

        private bool IsPointerEnterCharacterListItemOn(int slotIndex)
        {
            if (_draggedCharacterInfo.slotIndex.HasValue)
            {
                if (_draggedCharacterInfo.storageType != CharacterStorageType.AvailableCharacters)
                {
                    return true;
                }
                else
                {
                    return _draggedCharacterInfo.slotIndex.Value != slotIndex;
                }
            }
            return false;
        }

        #endregion

        #region FillAndClearMethods

        private void FillUpgradeRoomPanel()
        {
            //todo: add required resources and checking resources amount etc
            _upgradeRoomApplyButton.onClick.AddListener(OnClick_ApplyUpgradeRoomButton); //todo: disable button if not enough resources
        }

        private void FillRoomPanel(WorkRoomModel room)
        {
            _roomNameText.text = room.Name;
            _roomLevelText.text = $"Ур. {room.Level}";

            FillCharacterSlotUI(CharacterStorageType.ChiefWorkplace, 0, room.ChiefWorkplace.GetElementBySlot(0));
            room.ChiefWorkplace.OnPutElementToSlotHandler += FillCharacterSlotUI;
            room.ChiefWorkplace.OnTakeElementFromSlotHandler += FillCharacterSlotUI;

            for (int i = 0; i < room.AssistantWorkplaces.GetSlotsCount(); i++)
            {
                InitializeAssistantSlotUI(i);
                FillCharacterSlotUI(CharacterStorageType.AssistantWorkplaces, i, room.AssistantWorkplaces.GetElementBySlot(i));
            }

            for (int i = 0; i < room.OrdersSlots.GetSlotsCount(); i++)
            {
                InitializeOrderSlotUI(i);
                ItemOrderModel order = room.OrdersSlots.GetElementBySlot(i);
                if (order != null)
                {
                    OnAddOrderInSlot(OrderStorageType.None, i, order);
                }
            }
            _selectedRoom.OrdersSlots.OnPutElementToSlotHandler += OnAddOrderInSlot;
            _selectedRoom.OrdersSlots.OnTakeElementFromSlotHandler += OnRemoveOrderFromSlot;

            AvailableCharacterListItemBehaviour[] charactersBehaviours = _charactersFillablePanel.GetComponentsInChildren<AvailableCharacterListItemBehaviour>(true);
            for (int i = 0; i < charactersBehaviours.Length; i++)
            {
                charactersBehaviours[i].SetDisplayedSkill(room.UsedSkill, _context.Player.AvailableCharacters.GetElementBySlot(i));
            }

            room.OnLevelUpHandler += UpdateRoomPanel;
        }

        private void ClearRoomPanel()
        {
            _selectedRoom.ChiefWorkplace.OnPutElementToSlotHandler -= FillCharacterSlotUI;
            _selectedRoom.ChiefWorkplace.OnTakeElementFromSlotHandler -= FillCharacterSlotUI;
            _selectedRoom.AssistantWorkplaces.OnPutElementToSlotHandler -= FillCharacterSlotUI;
            _selectedRoom.AssistantWorkplaces.OnTakeElementFromSlotHandler -= FillCharacterSlotUI;
            _selectedRoom.OrdersSlots.OnTakeElementFromSlotHandler -= OnRemoveOrderFromSlot;
            _selectedRoom.OrdersSlots.OnPutElementToSlotHandler -= OnAddOrderInSlot;
            _selectedRoom.OnLevelUpHandler -= UpdateRoomPanel;

            for (int i = 0; i < _selectedRoom.OrdersSlots.GetSlotsCount(); i++)
            {
                ItemOrderModel order = _selectedRoom.OrdersSlots.GetElementBySlot(i);
                if (order != null)
                {
                    order.OnChangeHoursNumberToCompleteHandler -= _orderSlotsBehaviours[i].UpdateCraftTimeText;
                    order.OnCompleteHandler -= _orderSlotsBehaviours[i].UpdateSlot;
                }
            }

            for (int i = _assistantsSlotsBehaviours.Count - 1; i >= 0; i--)
            {
                Destroy(_assistantsSlotsBehaviours[i].gameObject);
            }

            for (int i = _orderSlotsBehaviours.Count - 1; i >= 0; i--)
            {
                Destroy(_orderSlotsBehaviours[i].gameObject);
            }

            _assistantsSlotsBehaviours.Clear();
            _orderSlotsBehaviours.Clear();
        }

        private void UpdateRoomPanel()
        {
            ClearRoomPanel();
            FillRoomPanel(_selectedRoom);
        }

        private void FillCharacterSlotUI(CharacterStorageType storageType, int slotIndex, CharacterModel character)
        {
            CharacterModel characterInSlot = GetCharacterStorageByType(storageType).GetElementBySlot(slotIndex);

            switch (storageType)
            {
                case CharacterStorageType.ChiefWorkplace:

                    _chiefSlotBehaviour.UpdateSlot(characterInSlot);
                    if (character != null)
                    {
                        _chiefSkillLevelImage.fillAmount = (float)character.Skills[_selectedRoom.UsedSkill] / 100;
                        _chiefSkillLevelText.text = character.Skills[_selectedRoom.UsedSkill].ToString() + "%";
                    }
                    else
                    {
                        _chiefSkillLevelImage.fillAmount = 0;
                        _chiefSkillLevelText.text = "0%";
                    }

                    break;

                case CharacterStorageType.AssistantWorkplaces:

                    _assistantsSlotsBehaviours[slotIndex].UpdateSlot(characterInSlot);
                    _assistantsSkillLevelImage.fillAmount = (float)_selectedRoom.AssistansGeneralSkillLevel / 100;
                    _assistantsSkillLevelText.text = _selectedRoom.AssistansGeneralSkillLevel.ToString() + "%";

                    break;

                default:

                    Debug.LogError(this + ": incorrect StorageSlotType");

                    break;
            }
        }

        private void OnAddOrderInSlot(OrderStorageType storageType, int slotIndex, ItemOrderModel order)
        {
            _orderSlotsBehaviours[slotIndex].UpdateSlot(order);
            _orderSlotsBehaviours[slotIndex].UpdateCraftTimeText(order.HoursNumberToComplete);
            order.OnChangeHoursNumberToCompleteHandler += _orderSlotsBehaviours[slotIndex].UpdateCraftTimeText;
            order.OnCompleteHandler += _orderSlotsBehaviours[slotIndex].UpdateSlot;
        }

        private void OnRemoveOrderFromSlot(OrderStorageType storageType, int slotIndex, ItemOrderModel order)
        {
            order.OnChangeHoursNumberToCompleteHandler -= _orderSlotsBehaviours[slotIndex].UpdateCraftTimeText;
            order.OnCompleteHandler -= _orderSlotsBehaviours[slotIndex].UpdateSlot;
            _orderSlotsBehaviours[slotIndex].UpdateSlot(null);
        }

        private void UpdateCharacterListItemUI(int slotIndex, CharacterModel character)
        {
            GetCharacterListItemBehaviour(slotIndex).UpdateSlot(character);
        }

        #endregion

        #region OtherMethods

        private AvailableCharacterListItemBehaviour GetCharacterListItemBehaviour(int index)
        {
            return _charactersFillablePanel.transform.GetChild(index).GetComponent<AvailableCharacterListItemBehaviour>();
        }

        private BaseCharacterStorage GetCharacterStorageByType(CharacterStorageType storageType)
        {
            BaseCharacterStorage storage = null;
            switch (storageType)
            {
                case CharacterStorageType.ChiefWorkplace:
                    storage = _selectedRoom.ChiefWorkplace;
                    break;

                case CharacterStorageType.AssistantWorkplaces:
                    storage = _selectedRoom.AssistantWorkplaces;
                    break;

                case CharacterStorageType.AvailableCharacters:
                    storage = _context.Player.AvailableCharacters;
                    break;

                default:
                    Debug.LogError(this + ": incorrect StorageSlotType");
                    break;
            }
            return storage;
        }

        #endregion

        #endregion
    }
}
