using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunterHubUI
{
    public class HubUIEventsService
    {
        private const float RANDOM_EVENT_CHANCE = 0.05f;


        private Action _onTimeTick;
        private HubUIContext _context;
        private Dictionary<GameTimeStruct, List<HubUIEventModel>> _scheduledEvents;


        public HubUIEventsService(HubUIContext context)
        {
            _context = context;
            _scheduledEvents = new Dictionary<GameTimeStruct, List<HubUIEventModel>>();
        }


        public void RemoveEventFromScheduler(HubUIEventModel eventModel)
        {
            if (_scheduledEvents.ContainsKey(eventModel.InvokeTime))
            {
                _scheduledEvents[eventModel.InvokeTime].Remove(eventModel);
                if (eventModel.IsEachTimeTickInvokeOn)
                {
                    _onTimeTick -= eventModel.TimeTick;
                }
                if(_scheduledEvents[eventModel.InvokeTime].Count == 0)
                {
                    _scheduledEvents.Remove(eventModel.InvokeTime);
                }
                Debug.Log($"Remove event from scheduler on time: {eventModel.InvokeTime}");
            }
        }

        public void AddEventToScheduler(HubUIEventModel eventModel)
        {
            if (!_scheduledEvents.ContainsKey(eventModel.InvokeTime))
            {
                _scheduledEvents.Add(eventModel.InvokeTime, new List<HubUIEventModel>());
            }
            _scheduledEvents[eventModel.InvokeTime].Add(eventModel);
            if (eventModel.IsEachTimeTickInvokeOn)
            {
                _onTimeTick += eventModel.TimeTick;
            }
            Debug.Log($"Add event to scheduler on time: {eventModel.InvokeTime}");
        }

        public void OnChangedGameTime(GameTimeStruct currentTime)
        {
            _onTimeTick?.Invoke();
            RandomEventCheck();
            ScheduleEventsCheck(currentTime);
        }

        private void ScheduleEventsCheck(GameTimeStruct invokeTime)
        {
            if (_scheduledEvents.ContainsKey(invokeTime))
            {
                if (_scheduledEvents[invokeTime].Count > 0)
                {
                    _context.GameTime.StopTimeSkip();
                    for (int i = 0; i < _scheduledEvents[invokeTime].Count; i++)
                    {
                        if (_scheduledEvents[invokeTime][i].IsEachTimeTickInvokeOn)
                        {
                            _onTimeTick -= _scheduledEvents[invokeTime][i].TimeTick;
                        }
                        _scheduledEvents[invokeTime][i].Invoke();
                    }
                }
                _scheduledEvents.Remove(invokeTime);
            }
        }

        private void RandomEventCheck() 
        { 
            if (UnityEngine.Random.Range(0, 101) <= RANDOM_EVENT_CHANCE * 100)
            {
                _context.GameTime.StopTimeSkip();
                HubUIServices.SharedInstance.GameMessages.Window("A random event has happened!");
            }
        }
    }
}
