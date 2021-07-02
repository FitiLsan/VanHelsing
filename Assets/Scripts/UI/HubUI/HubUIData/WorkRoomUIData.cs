using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class WorkRoomUIData
    {
        #region Fields

        [Header("Prefabs")]
        [SerializeField] private GameObject _workRoomButtonPrefab;
        [SerializeField] private GameObject _workerSlotPrefab;
        [SerializeField] private GameObject _orderSlotPrefab;
        [SerializeField] private GameObject _characterListItemPrefab;

        #endregion


        #region Properties

        public GameObject WorkRoomButtonPrefab => _workRoomButtonPrefab;
        public GameObject WorkerSlotPrefab => _workerSlotPrefab;
        public GameObject OrderSlotPrefab => _orderSlotPrefab;
        public GameObject CharacterListItemPrefab => _characterListItemPrefab;

        #endregion
    }
}
