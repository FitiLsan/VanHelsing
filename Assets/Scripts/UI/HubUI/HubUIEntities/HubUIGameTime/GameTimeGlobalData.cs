using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class GameTimeGlobalData
    {
        #region Fields

        [SerializeField] private int _hoursAmountPerDay;
        [SerializeField] private float _timePassingDelay;

        #endregion


        #region Properties

        public int HoursAmountPerDay => _hoursAmountPerDay;
        public float TimePassingDelay => _timePassingDelay;

        #endregion
    }
}
