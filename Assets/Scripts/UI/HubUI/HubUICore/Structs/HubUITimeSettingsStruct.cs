using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class HubUITimeSettingsStruct
    {
        #region Fields

        [SerializeField] private int _hoursAmountPerDay;
        [SerializeField] private int _hoursOnStartGame;
        [SerializeField] private int _dayOnStartGame;
        [SerializeField] private float _timePassingDelay;

        #endregion


        #region Properties

        public int HoursAmountPerDay => _hoursAmountPerDay;
        public int HoursOnStartGame => _hoursOnStartGame;
        public int DayOnStartGame => _dayOnStartGame;
        public float TimePassingDelay => _timePassingDelay;

        #endregion
    }
}
