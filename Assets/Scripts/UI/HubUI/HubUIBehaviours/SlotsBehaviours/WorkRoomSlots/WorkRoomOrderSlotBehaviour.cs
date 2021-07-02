using System;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    class WorkRoomOrderSlotBehaviour : BaseSlotBehaviour<ItemOrderModel, OrderStorageType>
    {
        #region Fields

        [SerializeField] private Button _openRecipeBookButton;
        [SerializeField] private Button _removeOrderButton;
        [SerializeField] private GameObject _processImage;
        [SerializeField] private Text _timeText;

        #endregion


        #region Properties

        public Action<int> OnClickRemoveOrderButtonHandler { get; set; }
        public Action<int> OnClickOpenRecipeBookButtonHandler { get; set; }

        #endregion


        #region BaseSlotBehaviour

        protected override void FillSlot(ItemOrderModel entityModel)
        {
            _changeableImage.sprite = entityModel.Recipe.Item.Icon;
            _openRecipeBookButton.interactable = false;
            _removeOrderButton.gameObject.SetActive(!entityModel.IsCompleted);
            _processImage.SetActive(!entityModel.IsCompleted);
        }

        protected override void ClearSlot()
        {
            _openRecipeBookButton.interactable = true;
            _removeOrderButton.gameObject.SetActive(false);
            _processImage.SetActive(false);
        }

        public override void Initialize(OrderStorageType storageType, int storageSlotIndex)
        {
            base.Initialize(storageType, storageSlotIndex);
            _removeOrderButton.onClick.AddListener(OnClickRemoveOrderButton);
            _openRecipeBookButton.onClick.AddListener(OnClickOpenRecipeBookButton);
            _removeOrderButton.gameObject.SetActive(false);
            _processImage.SetActive(false);
        }

        #endregion


        #region Methods

        public void UpdateCraftTimeText(int hours)
        {
            _timeText.text = $"{hours} {HubUIServices.SharedInstance.TimeService.GetHoursWord(hours)}";
        }

        private void OnClickRemoveOrderButton()
        {
            OnClickRemoveOrderButtonHandler?.Invoke(_storageSlotIndex);
        }

        private void OnClickOpenRecipeBookButton()
        {
            OnClickOpenRecipeBookButtonHandler?.Invoke(_storageSlotIndex);
        }

        #endregion
    }
}
