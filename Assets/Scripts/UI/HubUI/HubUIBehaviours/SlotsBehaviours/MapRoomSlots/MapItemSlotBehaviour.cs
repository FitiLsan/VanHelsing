using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace BeastHunterHubUI
{
    public class MapItemSlotBehaviour : BaseItemSlotBehaviour, IPointerDownHandler
    {
        #region Fields

        [SerializeField] private Button _slotButton;
        [SerializeField] private Image _selectSlotFrame;

        #endregion


        #region Properties

        public Action<int, ItemStorageType> OnPointerDownHandler { get; set; }

        #endregion


        #region Methods

        public override void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            OnPointerDownHandler = null;
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
                UpdateSlot(null);
            }
        }

        #endregion


        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsInteractable)
            {
                OnPointerDownHandler?.Invoke(_storageSlotIndex, _storageType);
            }
        }

        #endregion
    }
}
