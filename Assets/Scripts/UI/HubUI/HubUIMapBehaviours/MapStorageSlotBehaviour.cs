using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace BeastHunterHubUI
{
    public class MapStorageSlotBehaviour : MapBaseSlotBehaviour, IPointerDownHandler
    {
        #region Fields

        [SerializeField] private Button _slotButton;
        [SerializeField] private Image _selectSlotFrame;

        #endregion


        #region Properties

        public Action<int> OnPointerDownHandler { get; set; }

        #endregion


        #region Methods

        public override void FillSlotInfo(int slotIndex, bool isDragAndDropOn)
        {
            base.FillSlotInfo(slotIndex, isDragAndDropOn);
        }

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
                FillSlot(null);
            }
        }

        #endregion


        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isInteractable)
            {
                OnPointerDownHandler?.Invoke(_slotIndex);
            }
        }

        #endregion
    }
}
