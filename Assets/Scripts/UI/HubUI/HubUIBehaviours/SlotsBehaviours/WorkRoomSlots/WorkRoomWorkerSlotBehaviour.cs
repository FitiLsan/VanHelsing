namespace BeastHunterHubUI
{
    class WorkRoomWorkerSlotBehaviour : BaseCharacterSlotBehaviour
    {
        #region BaseCharacterSlotBehaviour

        protected override void OnBeginDragLogic()
        {
            base.OnBeginDragLogic();
            base.UpdateSlot(null);
        }

        #endregion
    }
}
