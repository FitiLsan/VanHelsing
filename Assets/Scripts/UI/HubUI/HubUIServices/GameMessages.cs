using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class GameMessages
    {
        #region Properties

        public Action<string> OnNoticeMessageHandler { get; set; }
        public Action<string> OnWindowMessageHandler { get; set; }

        #endregion


        #region Methods

        public void Notice(string msg)
        {
            Debug.Log("Notice game message: " + msg);
            OnNoticeMessageHandler?.Invoke(msg);
        }

        public void Window(string msg)
        {
            Debug.Log("Window game message: " + msg);
            OnWindowMessageHandler?.Invoke(msg);
        }

        #endregion
    }
}
