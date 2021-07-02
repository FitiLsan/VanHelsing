using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public struct GameTimeStruct
    {
        #region Fields

        [SerializeField] private int _day;
        [SerializeField] private int _hour;

        #endregion


        #region Properties

        public int Day
        {
            get
            {
                return _day;
            }
            set
            {
                _day = value >= 0 ? value : 0;
            }
        }
        public int Hour
        {
            get
            {
                return _hour;
            }
            set
            {
                _hour = value >= 0 ? value : 0;
            }
        }

        #endregion

        #region ClassLifeCycle

        public GameTimeStruct(int day, int hour)
        {
            _day = 0;
            _hour = 0;

            Day = day;
            Hour = hour;
        }

        #endregion


        #region Methods

        public override string ToString()
        {
            return $"day {Day}, hour {Hour}";
        }

        #endregion
    }
}
