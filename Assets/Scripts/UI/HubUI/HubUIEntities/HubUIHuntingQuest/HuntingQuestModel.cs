using System;


namespace BeastHunterHubUI
{
    public class HuntingQuestModel
    {
        #region Fields

        private HubUIEventModel _questEvent;
        private int _hoursAmountBeforeHunt;

        #endregion


        #region Properties

        public Action<HuntingQuestModel> OnTimeOveredHandler { get; set; }
        public Action OnCompletedHandler { get; set; }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public BossDataSO BossData { get; private set; }
        public bool IsBossTypeKnown { get; set; }
        public bool IsBossSubtypeKnown { get; set; }
        public bool IsBossSizeKnown { get; set; }
        public bool IsAttractionKnown { get; set; }
        public bool IsAvoidanceKnown { get; set; }
        public bool IsVulnerabilityKnown { get; set; }
        public HuntingQuestStatus QuestStatus { get; private set; }
        public GameTimeStruct RunningOutTime => _questEvent.InvokeTime;

        #endregion


        #region ClassLifeCycle

        public HuntingQuestModel(HuntingQuestSO data, BossDataSO bossData)
        {
            _hoursAmountBeforeHunt = data.HoursAmountBeforeHunt;
            Title = data.Title;
            Description = data.Description;
            BossData = bossData;
            QuestStatus = HuntingQuestStatus.NotActived;
        }

        #endregion


        #region Methods

        public void Activate()
        {
            QuestStatus = HuntingQuestStatus.Actived;
            _questEvent = new HubUIEventModel(_hoursAmountBeforeHunt, false);
            _questEvent.OnInvokeHandler = OnTimeOvered;
            HubUIServices.SharedInstance.EventsService.AddEventToScheduler(_questEvent);
        }

        public void Complete()
        {
            QuestStatus = HuntingQuestStatus.Completed;

            if (_questEvent != null)
            {
                HubUIServices.SharedInstance.EventsService.RemoveEventFromScheduler(_questEvent);
                RemoveEvent();
            }

            OnCompletedHandler?.Invoke();
        }

        private void OnTimeOvered()
        {
            QuestStatus = HuntingQuestStatus.TimeOvered;
            HubUIServices.SharedInstance.GameMessages.Window("It's time to go hunting!");
            RemoveEvent();
            OnTimeOveredHandler?.Invoke(this);
        }

        private void RemoveEvent()
        {
            _questEvent.OnInvokeHandler = null;
            _questEvent = null;
        }

        #endregion
    }
}
