using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public class HubMapUIStorageSlotBehaviour : HubMapUIBaseSlotBehaviour, IPointerClickHandler,  IPointerDownHandler
    {
        #region Constants

        private const float DOUBLECLICK_TIME = 0.75f;

        #endregion


        #region Fields

        [SerializeField] private Button _slotButton;
        [SerializeField] private Image _selectSlotFrame;

        private float _lastClickTime;

        #endregion


        #region Properties

        public Action<int> OnClick_SlotButtonHandler { get; set; }
        public Action<int> OnPointerDownHandler { get; set; }
        public Action<int> OnDoubleClickButtonHandler { get; set; }

        #endregion


        #region Methods

        public override void FillSlotInfo(int slotIndex, bool isDragAndDropOn)
        {
            base.FillSlotInfo(slotIndex, isDragAndDropOn);
            _slotButton.onClick.AddListener(() => OnClick_SlotButton());
        }

        public override void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            OnClick_SlotButtonHandler = null;
            OnPointerDownHandler = null;
            OnDoubleClickButtonHandler = null;
        }

        public void SelectFrameSwitcher(bool flag)
        {
            _selectSlotFrame.enabled = flag;
        }

        public override void SetInteractable(bool flag)
        {
            base.SetInteractable(flag);
            _slotButton.interactable = flag;

            if (!flag)
            {
                FillSlot(null);
            }
        }

        private void OnClick_SlotButton()
        {
            if (_slotButton.interactable)
            {
                OnClick_SlotButtonHandler?.Invoke(_slotIndex);
            }
        }

        #endregion


        #region IPointerClickHandler

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_slotButton.IsInteractable())
            {
                if (Time.time < _lastClickTime + DOUBLECLICK_TIME)
                {
                    OnDoubleClickButtonHandler?.Invoke(_slotIndex);
                }
                _lastClickTime = Time.time;
            }
        }

        #endregion


        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_slotButton.interactable)
            {
                OnPointerDownHandler?.Invoke(_slotIndex);
            }
        }

        #endregion
    }
}
