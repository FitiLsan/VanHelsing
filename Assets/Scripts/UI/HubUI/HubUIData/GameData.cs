using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class GameData
    {
        #region Fields

        [SerializeField] private GameTimeStruct _currentGameTime;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private LocationSO[] _locationsSO;
        [SerializeField] private CitySO[] _citiesSO;
        [SerializeField] private WorkRoomSO[] _workRoomsSO;

        #endregion


        #region Properties

        public GameTimeStruct CurrentGameTime => _currentGameTime;
        public PlayerData PlayerData => _playerData;
        public LocationSO[] LocationsSO => (LocationSO[])_locationsSO.Clone();
        public CitySO[] CitiesSO => (CitySO[])_citiesSO.Clone();
        public WorkRoomSO[] WorkRoomsSO => (WorkRoomSO[])_workRoomsSO.Clone();

        #endregion
    }
}
