using UnityEngine;
using System;


namespace BeastHunter
{
    public class TimeSkipService : Service
    {
        #region Fields

        private GameObject _timeSkipUI;
        private TimeSkipData _data;

        #endregion


        #region Properties

        public Action<bool> OnOpenCloseWindow;
        public bool IsOpen { get; private set; }

        #endregion



        #region ClassLifeCycles

        public TimeSkipService(Contexts contexts) : base(contexts)
        {
            _data = Data.TimeSkipData;
            _timeSkipUI = GameObject.Instantiate(_data.UIPrefab);
            CloseTimeSkipMenu();
        }

        #endregion


        #region Methods

        public void OpenTimeSkipMenu()
        {
            _timeSkipUI.SetActive(true);
            IsOpen = true;
            OnSetOpenAction();
        }

        public void CloseTimeSkipMenu()
        {
            _timeSkipUI.SetActive(false);
            IsOpen = false;
            OnSetOpenAction();
        }

        private void OnSetOpenAction()
        {
            OnOpenCloseWindow?.Invoke(IsOpen);
        }

        #endregion
    }
}

