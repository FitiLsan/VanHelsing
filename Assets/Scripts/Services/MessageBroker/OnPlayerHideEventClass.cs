namespace BeastHunter
{
    public sealed class OnPlayerHideEventClass
    {
        #region Properties

        public bool IsHiding { get; }

        #endregion


        #region ClassLifeCycle

        public OnPlayerHideEventClass(bool isHiding)
        {
            IsHiding = isHiding;
        }

        #endregion
    }
}

