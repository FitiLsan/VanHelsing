using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Events
{
    //Todo: maybe convert to simple static class?
    /// <summary>
    /// Менеджер событий. 
    /// </summary>
    public class EventManager
    {
        private static EventManager _eventManager;

        private Dictionary<GameEventTypes, GameEvent> _eventDictionary;

        private static EventManager Instance
        {
            get
            {
                if (_eventManager != null) return _eventManager;
                _eventManager = new EventManager();
                _eventManager.Init();
                return _eventManager;
            }
        }

        private void Init()
        {
            if (_eventDictionary == null) _eventDictionary = new Dictionary<GameEventTypes, GameEvent>();
        }

        /// <summary>
        ///     Регистриция слушателя события
        /// </summary>
        /// <param name="eventName">Тип события в игре</param>
        /// <param name="listener">Метод, вызывающийся на событии</param>
        public static void StartListening(GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (Instance._eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new GameEvent();
                thisEvent.AddListener(listener);
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        ///     Отписываемся от прослушки событий
        /// </summary>
        /// <param name="eventName">Тип события в игре</param>
        /// <param name="listener">Метод, который вызывался на событии</param>
        public static void StopListening(GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (_eventManager == null) return;
            if (Instance._eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.RemoveListener(listener);
        }

        /// <summary>
        ///     Вызываем событие
        /// </summary>
        /// <param name="eventName">Тип события</param>
        /// <param name="eventArgs">Параметры события</param>
        public static void TriggerEvent(GameEventTypes eventName, EventArgs eventArgs)
        {
            if (Instance._eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.Invoke(eventArgs);
        }

        /// <summary>
        ///     Сброс всех слушателей
        /// </summary>
        public static void Reset()
        {
            foreach (var gameEvent in Instance._eventDictionary) gameEvent.Value.RemoveAllListeners();
            _eventManager = new EventManager();
            _eventManager.Init();
        }
    }
}