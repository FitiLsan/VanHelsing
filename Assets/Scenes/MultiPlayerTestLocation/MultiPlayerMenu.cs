using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public class MultiPlayerMenu : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Button _createButton;
        [SerializeField]
        private Button _joinButton;

        private LobbyController _lobbyController;


        #endregion


        #region UnityMethods

        private void Start()
        {

            ButtonStartOff(false);

            _createButton.onClick.AddListener(Create);
            _joinButton.onClick.AddListener(Join);
            _lobbyController = GetComponent<LobbyController>();
            _lobbyController.ConnectToServer += ButtonStartOff;
        }

        private void ButtonStartOff(bool value)
        {
            _createButton.gameObject.SetActive(value);
            _joinButton.gameObject.SetActive(value);
        }

        private void OnDestroy()
        {
            _lobbyController.ConnectToServer -= ButtonStartOff;
        }

        #endregion


        #region Methods

        private void Create()
        {
            Debug.Log("Create");
            _lobbyController.CreateRoom();

        }

        private void Join()
        {
            Debug.Log("Join");
            _lobbyController.JoinRoom();
        }

        #endregion
    }
}