using System;
using System.Collections.Generic;
using UnityEngine.Events;


namespace BeastHunter
{
    public sealed class EventManager : Service
    {
        #region Fileds

        private Dictionary<GameEventTypes, GameEvent> _eventDictionary;
        private Dictionary<InputEventTypes, UnityEvent> _inputDictionary;

        public EventManager(Contexts contexts) : base(contexts)
        {
            Init();
        }

        #endregion


        #region Methods

        private void Init()
        {
            if (_eventDictionary == null) _eventDictionary = new Dictionary<GameEventTypes, GameEvent>();
            if (_inputDictionary == null) _inputDictionary = new Dictionary<InputEventTypes, UnityEvent>();
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

        public void StartListening(InputEventTypes inputName, UnityAction listener)
        {
            if (_inputDictionary.TryGetValue(inputName, out var thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                _inputDictionary.Add(inputName, thisEvent);
            }
        }

        public void StopListening(GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.RemoveListener(listener);
        }

        public void StopListening(InputEventTypes inputName, UnityAction listener)
        {
            if (_inputDictionary.TryGetValue(inputName, out var thisEvent)) thisEvent.RemoveListener(listener);
        }

        public void TriggerEvent(GameEventTypes eventName, EventArgs eventArgs)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.Invoke(eventArgs);
        }

        public void TriggerEvent(InputEventTypes inputName)
        {
            if (_inputDictionary.TryGetValue(inputName, out var thisEvent)) thisEvent.Invoke();
        }

        public void Reset()
        {
            foreach (var gameEvent in _eventDictionary) gameEvent.Value.RemoveAllListeners();
            foreach (var inputEvent in _inputDictionary) inputEvent.Value.RemoveAllListeners();
        }

        #endregion
    }
}