namespace BeastHunterHubUI
{
    public abstract class BaseCharacterSlotBehaviour : BaseSlotBehaviour<CharacterModel, CharacterStorageType>
    {
        #region BaseCharacterSlotBehaviour

        protected override void ClearSlot()
        {
            _changeableImage.sprite = null;
        }

        protected override void FillSlot(CharacterModel entityModel)
        {
            _changeableImage.sprite = entityModel.Portrait;
        }

        #endregion
    }
}
