namespace BeastHunter
{
    public sealed class OnPlayerReachHidePlaceEventClass
    {
        #region Properties

        public bool CanHide { get; }

        #endregion


        #region ClassLifeCycle

        public OnPlayerReachHidePlaceEventClass(bool canHide)
        {
            CanHide = canHide;
        }

        #endregion
    }
}

