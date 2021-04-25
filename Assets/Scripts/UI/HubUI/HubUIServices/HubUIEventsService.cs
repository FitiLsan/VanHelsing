using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class HubUIEventsService
    {
        private const float RANDOM_EVENT_CHANCE = 0.05f; 


        private HubUIContext _context;
        private Dictionary<HubUITimeStruct, List<HubUIEventModel>> _scheduledEvents;


        public HubUIEventsService(HubUIContext context)
        {
            _context = context;
            _scheduledEvents = new Dictionary<HubUITimeStruct, List<HubUIEventModel>>();
        }


        public void RemoveOrderEvent(HubUITimeStruct invokeTime, HubUIEventModel eventModel)
        {
            if (_scheduledEvents.ContainsKey(invokeTime))
            {
                _scheduledEvents[invokeTime].Remove(eventModel);
            }
        }

        public HubUIEventModel CreateNewOrderEvent(OrderModel order)
        {
            HubUIEventModel newEvent = new HubUIEventModel(HubUIEventType.OrderCompleted);
            AddEventToScheduler(order.CompletionTime.Value, newEvent);
            return newEvent;
        }

        public void AddEventToScheduler(HubUITimeStruct invokeTime, HubUIEventModel eventModel)
        {
            if (!_scheduledEvents.ContainsKey(invokeTime))
            {
                _scheduledEvents.Add(invokeTime, new List<HubUIEventModel>());
            }
            _scheduledEvents[invokeTime].Add(eventModel);
        }

        public void OnChangedGameTime(HubUITimeStruct currentTime)
        {
            RandomEventCheck();
            ScheduleEventsCheck(currentTime);
        }

        private void ScheduleEventsCheck(HubUITimeStruct invokeTime)
        {
            if (_scheduledEvents.ContainsKey(invokeTime))
            {
                _context.GameTime.StopTimeSkip();
                for (int i = 0; i < _scheduledEvents[invokeTime].Count; i++)
                {
                    _scheduledEvents[invokeTime][i].Invoke();
                    HubUIServices.SharedInstance.GameMessages.Window($"Event {_scheduledEvents[invokeTime][i].EventType} has happened!");
                }
                _scheduledEvents.Remove(invokeTime);
            }
        }

        private void RandomEventCheck() 
        { 
            if (Random.Range(0, 101) <= RANDOM_EVENT_CHANCE * 100)
            {
                _context.GameTime.StopTimeSkip();
                HubUIServices.SharedInstance.GameMessages.Window("A random event has happened!");
            }
        }
    }
}
