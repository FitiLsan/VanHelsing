using System;
using System.Collections.Generic;
using UnityEngine.Events;


namespace BeastHunter
{
    public sealed class EventManager : Service
    {
        #region Fileds

        private Dictionary<GameEventTypes, GameEvent> _eventDictionary;

        public EventManager(Contexts contexts) : base(contexts)
        {
            _eventDictionary = new Dictionary<GameEventTypes, GameEvent>();
        }

        #endregion


        #region Methods

        private void Init()
        {
            if (_eventDictionary == null) _eventDictionary = new Dictionary<GameEventTypes, GameEvent>();
        }

        public void StartListening(GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new GameEvent();
                thisEvent.AddListener(listener);
                _eventDictionary.Add(eventName, thisEvent);
            }
        }

        public  void StopListening(GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.RemoveListener(listener);
        }

        public  void TriggerEvent(GameEventTypes eventName, EventArgs eventArgs)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.Invoke(eventArgs);
        }

        public void Reset()
        {
            foreach (var gameEvent in _eventDictionary) gameEvent.Value.RemoveAllListeners();
        }

        #endregion
    }
}