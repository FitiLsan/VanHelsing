using System;
using System.Collections.Generic;
using UnityEngine.Events;


namespace BeastHunter
{
    public sealed class EventManager //Todo: maybe convert to simple static class?
    {
        #region Fileds

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

        #endregion


        #region Methods

        private void Init()
        {
            if (_eventDictionary == null) _eventDictionary = new Dictionary<GameEventTypes, GameEvent>();
        }

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

        public static void StopListening(GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (_eventManager == null) return;
            if (Instance._eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.RemoveListener(listener);
        }

        public static void TriggerEvent(GameEventTypes eventName, EventArgs eventArgs)
        {
            if (Instance._eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.Invoke(eventArgs);
        }

        public static void Reset()
        {
            foreach (var gameEvent in Instance._eventDictionary) gameEvent.Value.RemoveAllListeners();
            _eventManager = new EventManager();
            _eventManager.Init();
        }

        #endregion
    }
}