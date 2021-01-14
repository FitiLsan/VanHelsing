using UnityEngine;
using System;


namespace BeastHunter
{
    [Obsolete]
    public sealed class TimeSkipService : IService
    {
        #region Fields

        private GameObject _timeSkipUI;

        #endregion


        #region Properties

        public Action<bool> OnOpenCloseWindow;
        public bool IsOpen { get; private set; }

        #endregion



        #region ClassLifeCycles

        public TimeSkipService()
        {
            _timeSkipUI = GameObject.Instantiate(Data.UIElementsData.TimeSkipPrefab);
            CloseTimeSkipMenu();
        }

        #endregion


        #region Methods

        public void OpenTimeSkipMenu()
        {
            _timeSkipUI.SetActive(true);
            IsOpen = true;
            OnSetOpenCloseAction();
        }

        public void CloseTimeSkipMenu()
        {
            _timeSkipUI.SetActive(false);
            IsOpen = false;
            OnSetOpenCloseAction();
        }

        private void OnSetOpenCloseAction()
        {
            OnOpenCloseWindow?.Invoke(IsOpen);
        }

        #endregion
    }
}

