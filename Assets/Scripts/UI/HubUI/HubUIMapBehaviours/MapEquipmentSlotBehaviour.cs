using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    public class MapEquipmentSlotBehaviour : MapBaseSlotBehaviour
    {
        #region Fields

        [SerializeField] Image _slotImage;

        #endregion


        #region Methods

        public void FillSlotInfo(int slotIndex, bool isDragAndDropOn, Sprite slotSprite)
        {
            base.FillSlotInfo(slotIndex, isDragAndDropOn);
            _slotImage.sprite = slotSprite;
            _slotImage.enabled = true;
        }

        public override void FillSlot(Sprite sprite)
        {
            base.FillSlot(sprite);

            if (sprite != null)
            {
                _slotImage.enabled = false;
            }
            else
            {
                _slotImage.enabled = true;
            }
        }

        public override void SetInteractable(bool flag)
        {
            base.SetInteractable(flag);

            if (!flag)
            {
                base.FillSlot(null);
            }

            _slotImage.enabled = flag;
        }

        public void FillSlotAsSecondary(Sprite sprite)
        {
            FillSlot(sprite);

            if (sprite != null)
            {
                _itemImage.color = GetTranslucentColor(true, _itemImage.color);
                base.SetInteractable(false);
            }
            else
            {
                _itemImage.color = GetTranslucentColor(false, _itemImage.color);
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
    }
}
