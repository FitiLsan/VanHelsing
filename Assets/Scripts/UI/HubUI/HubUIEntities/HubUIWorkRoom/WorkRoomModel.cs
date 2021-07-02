using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class WorkRoomModel : BaseWorkRoomModel<WorkRoomProgress>
    {
        private int _minOrderCompleteTime;

        #region Properties

        public Action<int> OnChangeMinOrderCompleteTimeHandler { get; set; }

        public OrderSlotStorage OrdersSlots { get; private set; }
        public override Dictionary<int, WorkRoomProgress> ProgressScheme { get; protected set; }

        public int MinOrderCompleteTime
        {
            get => _minOrderCompleteTime;
            private set
            {
                if (value != _minOrderCompleteTime)
                {
                    _minOrderCompleteTime = value;
                    OnChangeMinOrderCompleteTimeHandler?.Invoke(_minOrderCompleteTime);
                }
            }
        }

        #endregion


        #region ClassLifeCycle

        public WorkRoomModel(WorkRoomData roomStruct) : base(roomStruct.BaseWorkRoomData)
        {
            OrdersSlots = new OrderSlotStorage(ProgressScheme[Level].OrderSlots);

            if (roomStruct.Orders != null)
            {
                for (int i = 0; i< roomStruct.Orders.Length; i++)
                {
                    ItemOrderModel order = new ItemOrderModel(roomStruct.Orders[i].Recipe, OrderTimeReducePercent, roomStruct.Orders[i].ProgressToComplete);
                    if (!OrdersSlots.PutElement(roomStruct.Orders[i].SlotIndex, order))
                    {
                        Debug.LogError($"Failed to place order in slot {i}");
                    }
                }
            }

            OrdersSlots.OnPutElementToSlotHandler += OnOrderAdd;
            OrdersSlots.OnTakeElementFromSlotHandler += OnOrderRemove;
        }

        #endregion


        #region Methods

        protected override void RoomImprove()
        {
            OrdersSlots.AddSlots(ProgressScheme[Level].OrderSlots - OrdersSlots.GetSlotsCount());
        }

        protected override float CountOrderTimeReducePercent()
        {
            //todo: counting by design document
            //temporary for debug:
            float maxLevelSkill = 200.0f;
            float maxReducePercent = 0.5f;
            float totalLevelSkill = AssistansGeneralSkillLevel;
            if (ChiefWorkplace.GetElementBySlot(0) != null)
            {
                totalLevelSkill += ChiefWorkplace.GetElementBySlot(0).Skills[UsedSkill];
            }
            return 1 - (totalLevelSkill * maxReducePercent / maxLevelSkill);
        }

        protected override void OnChiefAdd(CharacterModel character)
        {
            for (int i = 0; i < OrdersSlots.GetSlotsCount(); i++)
            {
                ItemOrderModel order = OrdersSlots.GetElementBySlot(i);
                if (order != null)
                {
                    order.RecountHoursToComplete(OrderTimeReducePercent);
                    order.AddOrderEvent();
                }
            }
        }

        protected override void OnChiefRemove(CharacterModel character)
        {
            for (int i = 0; i < OrdersSlots.GetSlotsCount(); i++)
            {
                ItemOrderModel order = OrdersSlots.GetElementBySlot(i);
                if (order != null)
                {
                    order.RecountHoursToComplete(OrderTimeReducePercent);
                    order.RemoveOrderEvent();
                }
            }
        }

        protected override void OnAssistantAdd(CharacterModel character)
        {
            for (int i = 0; i < OrdersSlots.GetSlotsCount(); i++)
            {
                ItemOrderModel order = OrdersSlots.GetElementBySlot(i);
                if (order != null)
                {
                    order.RecountHoursToComplete(OrderTimeReducePercent);
                }
            }
        }

        protected override void OnAssistantRemove(CharacterModel character)
        {
            for (int i = 0; i < OrdersSlots.GetSlotsCount(); i++)
            {
                ItemOrderModel order = OrdersSlots.GetElementBySlot(i);
                if (order != null)
                {
                    order.RecountHoursToComplete(OrderTimeReducePercent);
                }
            }
        }

        private void OnOrderAdd(OrderStorageType storageType, int slotIndex, ItemOrderModel order)
        {
            if (ChiefWorkplace.GetElementBySlot(0) != null)
            {
                order.AddOrderEvent();
            }
            UpdateMinOrderCompleteTime();
            order.OnChangeHoursNumberToCompleteHandler += OnChangeHoursNumberToComplete;
        }

        private void OnOrderRemove(OrderStorageType storageType, int slotIndex, ItemOrderModel order)
        {
            order.OnCompleteHandler = null;
            order.OnChangeHoursNumberToCompleteHandler = null;
            order.RemoveOrderEvent();
            UpdateMinOrderCompleteTime();
        }

        private void OnChangeHoursNumberToComplete(int hours)
        {
            if (hours < MinOrderCompleteTime)
            {
                MinOrderCompleteTime = hours;
            }
        }

        private void UpdateMinOrderCompleteTime()
        {
            int? minNumber = null;
            for (int i = 0; i < OrdersSlots.GetSlotsCount(); i++)
            {
                ItemOrderModel order = OrdersSlots.GetElementBySlot(i);
                if (order != null)
                {
                    if (!minNumber.HasValue)
                    {
                        minNumber = order.HoursNumberToComplete;
                    }
                    else
                    {
                        if (minNumber.Value > order.HoursNumberToComplete)
                        {
                            minNumber = order.HoursNumberToComplete;
                        }
                    }
                }
            }
            if (!minNumber.HasValue)
            {
                minNumber = 0;
            }
            MinOrderCompleteTime = minNumber.Value;
        }

        #endregion
    }
}
