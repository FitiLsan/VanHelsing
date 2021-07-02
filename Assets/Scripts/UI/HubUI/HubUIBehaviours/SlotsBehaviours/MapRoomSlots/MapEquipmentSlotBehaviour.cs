using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    public class MapEquipmentSlotBehaviour : BaseItemSlotBehaviour
    {
        #region Fields

        [SerializeField] Image _slotImage;

        #endregion


        #region Methods

        public void Initialize(ItemStorageType storageType, int slotIndex, Sprite slotSprite)
        {
            base.Initialize(storageType, slotIndex);
            _slotImage.sprite = slotSprite;
            _slotImage.enabled = true;
        }

        public void FillSlotAsSecondary(BaseItemModel item)
        {
            UpdateSlot(item);

            if (item != null)
            {
                _changeableImage.color = GetTranslucentColor(true, _changeableImage.color);
                base.SetInteractable(false);
            }
            else
            {
                _changeableImage.color = GetTranslucentColor(false, _changeableImage.color);
                base.SetInteractable(true);
            }
        }

        private Color GetTranslucentColor(bool isTranslucent, Color color)
        {
            Color newColor;
            if (isTranslucent)
            {
                newColor = new Color(color.r, color.g, color.b, 0.5f);
            }
            else
            {
                newColor = new Color(color.r, color.g, color.b, 1.0f);
            }
            return newColor;
        }

        #endregion


        #region BaseItemSlotBehaviour

        public override void Initialize(ItemStorageType storageType, int storageSlotIndex)
        {
            base.Initialize(storageType, storageSlotIndex);
            _slotImage.enabled = false;
            Debug.LogWarning("If you want to use icon for slot, use a second Initialize method (with sprite parameter)");
        }

        protected override void FillSlot(BaseItemModel item)
        {
            base.FillSlot(item);
            _slotImage.enabled = false;
        }

        protected override void ClearSlot()
        {
            base.ClearSlot();
            _slotImage.enabled = true;
        }

        public override void SetInteractable(bool flag)
        {
            base.SetInteractable(flag);

            if (!flag)
            {
                base.UpdateSlot(null);
            }

            _slotImage.enabled = flag;
        }

        #endregion
    }
}
