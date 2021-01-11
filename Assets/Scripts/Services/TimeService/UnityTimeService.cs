using System;
using UnityEngine;


namespace BeastHunter
{
    public sealed class UnityTimeService : IService
    {
        #region Fields

        private int _deltaTimeResetFrame;

        #endregion


        #region Properties
        
        public float DeltaTime() => _deltaTimeResetFrame == Time.frameCount ? 0.0f : Time.deltaTime;
        public float UnscaledDeltaTime() => _deltaTimeResetFrame == Time.frameCount ? 0.0f : Time.unscaledDeltaTime;
        public float FixedDeltaTime() => _deltaTimeResetFrame == Time.frameCount ? 0.0f : Time.fixedDeltaTime;
        public float RealtimeSinceStartup() => Time.realtimeSinceStartup;
        public float GameTime() => Time.time;

        #endregion

        
        #region Methods
        
        public long Timestamp()
        {
            long timestamp = DateTime.UtcNow.ToUnixTimestamp();

            return timestamp;
        }

        public void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
        
        public void ResetDeltaTime()
        {
            _deltaTimeResetFrame = Time.frameCount;
        }

        #endregion
    }
}
