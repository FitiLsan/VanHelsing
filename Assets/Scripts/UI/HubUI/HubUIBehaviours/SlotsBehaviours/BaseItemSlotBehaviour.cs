namespace BeastHunterHubUI
{
    public abstract class BaseItemSlotBehaviour : BaseSlotBehaviour<BaseItemModel, ItemStorageType>
    {
        #region BaseSlotBehaviour

        protected override void FillSlot(BaseItemModel entityModel)
        {
            _changeableImage.sprite = entityModel.Icon;
        }

        protected override void ClearSlot()
        {
            _changeableImage.sprite = null;
        }

        protected override void OnBeginDragLogic()
        {
            base.OnBeginDragLogic();
            UpdateSlot(null);
        }

        #endregion
    }
}
