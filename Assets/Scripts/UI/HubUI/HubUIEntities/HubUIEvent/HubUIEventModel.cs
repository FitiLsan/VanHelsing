using System;


namespace BeastHunterHubUI
{
    public class HubUIEventModel
    {
        #region Properties

        public Action OnInvokeHandler { get; set; }

        public HubUIEventType EventType { get; private set; }

        #endregion


        #region ClassLifeCycle

        public HubUIEventModel(HubUIEventType eventType)
        {
            EventType = eventType;
        }

        #endregion


        #region Methods

        public void Invoke()
        {
            OnInvokeHandler?.Invoke();
        }

        #endregion
    }
}
