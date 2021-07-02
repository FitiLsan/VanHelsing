using System;
using System.Collections;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class GameTimeModel
    {
        #region Fields

        private float _timePassingDelay;
        private GameTimeStruct _currentTime;

        #endregion


        #region Properties

        public Action<GameTimeStruct> OnChangeTimeHandler { get; set; }
        public Action<bool> OnSwitchTimeSkipHandler { get; set; }

        public GameTimeStruct CurrentTime => _currentTime;
        public int HoursAmountPerDay { get; private set; }
        public bool IsTimePassing { get; private set; }

        #endregion


        #region ClassLifeCycle

        public GameTimeModel(GameTimeStruct timeStruct)
        {
            GameTimeGlobalData settings = BeastHunter.Data.HubUIData.GameTimeGlobalData;
            _timePassingDelay = settings.TimePassingDelay;
            HoursAmountPerDay = settings.HoursAmountPerDay;

            _currentTime = new GameTimeStruct(timeStruct.Day, timeStruct.Hour);

            IsTimePassing = false;
        }

        #endregion


        #region Methods

        public IEnumerator StartTimeSkip()
        {
            IsTimePassing = true;
            OnSwitchTimeSkipHandler?.Invoke(true);

            while (IsTimePassing)
            {
                yield return new WaitForSeconds(_timePassingDelay);
                if (!IsTimePassing)
                {
                    break;
                }
                OneHourPass();
                OnChangeTimeHandler?.Invoke(CurrentTime);
            }

            OnSwitchTimeSkipHandler?.Invoke(false);
        }

        public void StopTimeSkip()
        {
            IsTimePassing = false;
        }

        public GameTimeStruct AddTime(GameTimeStruct time)
        {
            return AddTime(GameTimeStructToHours(time));
        }

        public GameTimeStruct AddTime(int hours)
        {
            GameTimeStruct newTime = CurrentTime;

            if (hours > 0)
            {
                if (hours < HoursAmountPerDay - CurrentTime.Hour)
                {
                    newTime.Hour += hours;
                }
                else
                {
                    int h = hours;
                    int d = 0;
                    while (h >= HoursAmountPerDay - newTime.Hour)
                    {
                        h -= HoursAmountPerDay;
                        d++;
                    }
                    newTime.Day += d;
                    newTime.Hour += h;
                }
            }

            return newTime;
        }

        private void OneHourPass()
        {
            if (_currentTime.Hour + 1 >= HoursAmountPerDay)
            {
                _currentTime.Day += 1;
                _currentTime.Hour = 0;
            }
            else
            {
                _currentTime.Hour += 1;
            }
        }

        private int GameTimeStructToHours(GameTimeStruct time)
        {
            return time.Hour + time.Day * HoursAmountPerDay;
        }

        #endregion
    }
}
