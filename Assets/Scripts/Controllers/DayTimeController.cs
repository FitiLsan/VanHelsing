using System;
using BaseScripts;
using Common;
using Events;
using Events.Args;
using Settings;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Контроллер времени суток и времени дня в игре
    /// </summary>
    public class DayTimeController : BaseController
    {
        private readonly int[] _settings = new int[Enum.GetNames(typeof(DayTimeTypes)).Length];
        private float _currentTime = 0;
        private DayTimeTypes _currentDayTime = DayTimeTypes.Morning;
        private bool _eventFlag = false;
        private int _i = 0;
        private float _mid = 0f;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="settings">Настройки времени каждой части суток</param>
        /// <param name="currentDayTime">Текущее время суток (если из сейваб например)</param>
        /// <param name="currentTime">Текущий момент времени (если из сейва, например)</param>
        public DayTimeController(DayTimeSettings settings,DayTimeTypes currentDayTime = DayTimeTypes.Morning, float currentTime = 0)
        {
            _settings[(int)DayTimeTypes.Morning] = settings.MorningLength;
            _settings[(int)DayTimeTypes.Day] = settings.DayLength;
            _settings[(int)DayTimeTypes.Evening] = settings.EveningLength;
            _settings[(int)DayTimeTypes.Night] = settings.NightLength;
            _currentTime = currentTime;
            _i = (int) currentDayTime;
            _mid = _settings[_i] / 2f;
        }
        /// <summary>
        /// Тик контроллера
        /// </summary>
        public override void ControllerUpdate()
        {
            var dt = Time.deltaTime;
            _currentTime += dt;
            if (_currentTime >= _settings[_i])
            {
                _currentTime -= _settings[_i];
                _i++;
                if (_i == _settings.Length) _i = 0;
                EventManager.TriggerEvent(GameEventTypes.DayTimeChanged, new DayTimeArgs((DayTimeTypes)_i));
                if (_currentDayTime == DayTimeTypes.Night)
                    _currentDayTime = DayTimeTypes.Morning;
                else
                {
                    _currentDayTime++;
                }

                _mid = _settings[_i] / 2f;
                _eventFlag = false;
            }

            if (!_eventFlag && _currentTime >= _mid)
            {
               EventManager.TriggerEvent(GameEventTypes.MidOfDayTimeComes, new DayTimeArgs((DayTimeTypes)_i));
               _eventFlag = true;
            }
        }

        /// <summary>
        /// Текущее время суток
        /// </summary>
        public DayTimeTypes CurrentDayTime => _currentDayTime;
        /// <summary>
        /// Возвращает внутреннее время для текущего времени суток (1) и его продолжительность (2)
        /// </summary>
        public (float, int) GetTime => (_currentTime, _settings[_i]);

        /// <summary>
        /// Устанавливает заданное время суток
        /// </summary>
        /// <param name="dayTime">Время суток</param>
        /// <param name="mid">Середина времени суток</param>
        public void SetDayTime(DayTimeTypes dayTime, bool mid = false)
        {
            _i = (int) dayTime;
            EventManager.TriggerEvent(GameEventTypes.DayTimeChanged, new DayTimeArgs(dayTime));
            _currentDayTime = dayTime;
            _mid = _settings[_i] / 2f;
            if (mid)
            {
                _currentTime = _mid;
                EventManager.TriggerEvent(GameEventTypes.MidOfDayTimeComes, new DayTimeArgs(dayTime));
                _eventFlag = true;
            }
            else
            {
                _eventFlag = false;
                _currentTime = 0;
            }
            #if UNITY_EDITOR
            var t = GetTime;
             Debug.Log($"DayTimeController.SetDayTime: daytime[{CurrentDayTime}], time[{t.Item1}/{t.Item2}]");
            #endif
        }
        
    }
}