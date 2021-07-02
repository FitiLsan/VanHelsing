using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class ItemOrderModel
    {
        #region Fields

        private HubUIEventModel _orderEvent;
        private int _hoursNumberToComplete;
        private int _baseHoursToComplete;

        #endregion


        #region Properties

        public Action<ItemOrderModel> OnCompleteHandler { get; set; }
        public Action<int> OnChangeHoursNumberToCompleteHandler { get; set; }

        public bool IsCompleted { get; private set; }
        public float ProgressToComplete { get; private set; }
        public ItemRecipeSO Recipe { get; private set; }
        public BaseItemModel MakedItem { get; private set; }
        public int HoursNumberToComplete
        {
            get => _hoursNumberToComplete;
            private set
            {
                if (value != _hoursNumberToComplete)
                {
                    _hoursNumberToComplete = value;
                    OnChangeHoursNumberToCompleteHandler?.Invoke(_hoursNumberToComplete);
                }
            }
        }

        #endregion


        #region ClassLifeCycle

        public ItemOrderModel(ItemRecipeSO recipe, float timeReducePercent, float progressToComplete = 0.0f)
        {
            IsCompleted = false;
            Recipe = recipe;
            ProgressToComplete = progressToComplete;

            if (ProgressToComplete == 1.0f)
            {
                IsCompleted = true;
                HoursNumberToComplete = 0;
                MakeItem();
            }
            else
            {
                IsCompleted = false;
                RecountHoursToComplete(timeReducePercent);
            }
        }

        #endregion


        #region Methods

        public void AddOrderEvent()
        {
            _orderEvent = new HubUIEventModel(HoursNumberToComplete, true);
            _orderEvent.OnInvokeHandler = Complete;
            _orderEvent.OnTickTimeHandler += TimeTickUpdate;
            HubUIServices.SharedInstance.EventsService.AddEventToScheduler(_orderEvent);
        }

        public void RemoveOrderEvent()
        {
            if (_orderEvent != null)
            {
                HubUIServices.SharedInstance.EventsService.RemoveEventFromScheduler(_orderEvent);
                _orderEvent.OnInvokeHandler = null;
                _orderEvent.OnTickTimeHandler = null;
                _orderEvent = null;
            }
        }

        public void RecountHoursToComplete(float timeReducePercent)
        {
            //Debug.Log("RecountHoursToComplete()");
            //Debug.Log("timeReducePercent=" + timeReducePercent);

            _baseHoursToComplete = (int)Mathf.Round(Recipe.BaseHoursNumberToComplete * timeReducePercent);
            HoursNumberToComplete = (int)Mathf.Round(_baseHoursToComplete - (_baseHoursToComplete * ProgressToComplete));

            if(_orderEvent != null)
            {
                RemoveOrderEvent();
                AddOrderEvent();
            }

            //Debug.Log("_baseHoursToComplete=" + _baseHoursToComplete);
            //Debug.Log("HoursNumberToComplete=" + HoursNumberToComplete);
        }

        private void TimeTickUpdate()
        {
            //Debug.Log("TimeTickUpdate()");

            int spentHour = 1;
            HoursNumberToComplete = HoursNumberToComplete - spentHour < 0 ? 0 : HoursNumberToComplete - spentHour;
            ProgressToComplete = (float)(_baseHoursToComplete - HoursNumberToComplete) / _baseHoursToComplete;

            //Debug.Log("HoursNumberToComplete="+ HoursNumberToComplete);
            //Debug.Log("ProgressToComplete=" + ProgressToComplete);
        }

        private void MakeItem()
        {
            //todo: add fail chance
            MakedItem = HubUIServices.SharedInstance.ItemInitializeService.InitializeItemModel(Recipe.Item);
        }

        private void Complete()
        {
            IsCompleted = true;

            _orderEvent.OnInvokeHandler = null;
            _orderEvent.OnTickTimeHandler = null;
            _orderEvent = null;

            MakeItem();
            HubUIServices.SharedInstance.GameMessages.OnWindowMessageHandler($"Recipe {Recipe.Item.Name} is completed!");
            OnCompleteHandler?.Invoke(this);
        }

        #endregion
    }
}
