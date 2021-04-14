using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public class HubMapUIEquipmentSlotBehaviour : HubMapUIBaseSlotBehaviour
    {
        [SerializeField] Image _slotImage;

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
    }
}
