using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Photon.Pun;

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

        #endregion
        void Start()
        {
            _createButton.onClick.AddListener(Create);
            _joinButton.onClick.AddListener(Join);
            _lobbyController = gameObject.GetComponent<LobbyController>();
        }

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