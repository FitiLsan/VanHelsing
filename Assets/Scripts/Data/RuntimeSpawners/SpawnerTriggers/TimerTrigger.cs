using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public sealed class TimerTrigger: SpawnerTrigger
    {
        #region Fields

        public int TimeToSpawn = 10;

        #endregion
    }
}