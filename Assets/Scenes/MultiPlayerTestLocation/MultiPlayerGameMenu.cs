using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Photon.Pun;

namespace BeastHunter
{
    public class MultiPlayerGameMenu : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Button _exitButton;

        private LobbyController _lobbyController;

        #endregion

        #region UnityMethods

        void Start()
        {
            _exitButton.onClick.AddListener(Exit);
            _lobbyController = gameObject.GetComponent<LobbyController>();
        }

        #endregion

        #region Methods
        private void Exit()
        {
            Debug.Log("Exit");
            _lobbyController.LeaveRoom();
        }
        
        #endregion
    }
}