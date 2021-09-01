using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UniRx;
using System;
using Random = UnityEngine.Random;


namespace BeastHunter
{
    public class LobbyController : MonoBehaviourPunCallbacks
    {
        public  Action<bool> ConnectToServer { get; set; }

        private void Start()
        {
            InitializedNewNetWorkClient();
        }

        private void InitializedNewNetWorkClient()
        {
            PhotonNetwork.NickName = $"Player{Random.Range(0, 999)}";
            Debug.Log(PhotonNetwork.NickName);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "v1.2";
            PhotonNetwork.ConnectUsingSettings();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom(null);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connect to Master");
            PhotonNetwork.JoinLobby();
            ConnectToServer?.Invoke(true);
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined to Lobby");
        }
        public override void OnCreatedRoom()
        {
            PhotonNetwork.LoadLevel("LDI Example");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Join to Room");
            //    PhotonNetwork.LoadLevel("MultiPlayerGameScene");
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MultiPlayerMenuScene");
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.NickName} entered to room");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"Player {otherPlayer.NickName} left room");
        }

    }
}