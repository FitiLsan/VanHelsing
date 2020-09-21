using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class TimeRemainingController : IUpdate
    {
        #region Fields
        
        private readonly List<ITimeRemaining> _timeRemainings;
        private readonly UnityTimeService _timeService;
        
        #endregion

        
        #region ClassLifeCycles

        public TimeRemainingController(Contexts contexts)
        {
            _timeRemainings = TimeRemainingExtensions.TimeRemainings;
            _timeService = Services.SharedInstance.UnityTimeService;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            var time = _timeService.DeltaTime();
            for (var i = 0; i < _timeRemainings.Count; i++)
            {
                var obj = _timeRemainings[i];
                obj.CurrentTime -= time;
                if (obj.CurrentTime <= 0.0f)
                {
                    obj?.Method?.Invoke();
                    if (!obj.IsRepeating)
                    {
                        obj.RemoveTimeRemaining();
                    }
                    else
                    {
                        obj.CurrentTime = obj.Time;
                    }
                }
            }
        }
        
        #endregion
    }
}
