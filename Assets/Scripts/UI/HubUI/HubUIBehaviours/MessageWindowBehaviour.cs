using System;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    class MessageWindowBehaviour: MonoBehaviour
    {
        #region Fields

        [SerializeField] Text _messageText;
        [SerializeField] Button _closeButton;

        #endregion


        #region Properties

        public Action OnCloseWindowHandler { get; set; }

        #endregion


        #region Methods

        public void FillInfo(string message)
        {
            _messageText.text = message;
            _closeButton.onClick.AddListener(OnClick_CloseButton);
        }

        private void OnClick_CloseButton()
        {
            gameObject.SetActive(false);
            OnCloseWindowHandler?.Invoke();
            Destroy(gameObject);
        }

        #endregion
    }
}
