using System;
using System.Collections;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class HubUIGameTime
    {
        #region Fields

        private float _timePassingDelay;

        #endregion


        #region Properties

        public Action<HubUITimeStruct> OnChangeTimeHandler { get; set; }

        public int HoursAmountPerDay { get; private set; }
        public HubUITimeStruct CurrentTime { get; private set; }
        public bool IsTimePassing { get; private set; }

        #endregion


        #region ClassLifeCycle

        public HubUIGameTime(HubUITimeSettingsStruct settings)
        {
            IsTimePassing = false;
            _timePassingDelay = settings.TimePassingDelay;
            HoursAmountPerDay = settings.HoursAmountPerDay;
            CurrentTime = new HubUITimeStruct(settings.DayOnStartGame, settings.HoursOnStartGame);
        }

        #endregion


        #region Methods

        public IEnumerator StartTimeSkip()
        {
            IsTimePassing = true;
            while (IsTimePassing)
            {
                yield return new WaitForSeconds(_timePassingDelay);
                if (!IsTimePassing)
                {
                    yield break;
                }
               OneHourPass();
            }
        }

        public void StopTimeSkip()
        {
            IsTimePassing = false;
        }

        public void OneHourPass()
        {
            CurrentTime = AddOneHour(CurrentTime);
            OnChangeTime();
        }

        public HubUITimeStruct AddTime(HubUITimeStruct time)
        {
            return AddTime(GameTimeStructToHours(time));
        }

        public HubUITimeStruct AddTime(int hours)
        {
            HubUITimeStruct newTime = CurrentTime;

            if (hours > 0)
            {
                if (hours < HoursAmountPerDay - CurrentTime.Hour)
                {
                    newTime = new HubUITimeStruct(newTime.Day, newTime.Hour + hours);
                }
                else
                {
                    int h = hours;
                    int d = 0;
                    while (h > HoursAmountPerDay - newTime.Hour)
                    {
                        h -= HoursAmountPerDay;
                        d++;
                    }
                    newTime = new HubUITimeStruct(newTime.Day + d, newTime.Hour + h);
                }
            }

            return newTime;
        }

        private HubUITimeStruct AddOneHour(HubUITimeStruct currentTime)
        {
            if (currentTime.Hour + 1 >= HoursAmountPerDay)
            {
                return new HubUITimeStruct(currentTime.Day + 1, 0);
            }
            else
            {
                return new HubUITimeStruct(currentTime.Day, currentTime.Hour + 1);
            }
        }

        private int GameTimeStructToHours(HubUITimeStruct time)
        {
            return time.Hour + time.Day * HoursAmountPerDay;
        }

        private void OnChangeTime()
        {
            OnChangeTimeHandler?.Invoke(CurrentTime);
        }

        #endregion
    }
}
