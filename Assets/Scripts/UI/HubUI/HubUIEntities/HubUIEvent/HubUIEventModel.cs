using System;


namespace BeastHunterHubUI
{
    public class HubUIEventModel
    {
        #region Properties

        public Action OnInvokeHandler { get; set; }
        public Action OnTickTimeHandler { get; set; }
        public bool IsEachTimeTickInvokeOn { get; private set; }

        public GameTimeStruct InvokeTime { get; private set; }

        #endregion


        #region ClassLifeCycle

        public HubUIEventModel(int invokeHoursAmount, bool isEachTimeTickInvokeOn)
        {
            IsEachTimeTickInvokeOn = isEachTimeTickInvokeOn;
            InvokeTime = HubUIServices.SharedInstance.TimeService.CalculateTimeOnAddHours(invokeHoursAmount);
        }

        #endregion


        #region Methods

        public void Invoke()
        {
            OnInvokeHandler?.Invoke();
        }

        public void TimeTick()
        {
            OnTickTimeHandler?.Invoke();
        }

        #endregion
    }
}
