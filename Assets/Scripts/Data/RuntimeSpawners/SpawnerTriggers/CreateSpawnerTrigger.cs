using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public sealed class CreateSpawnerTrigger
    {
        //private SpawnerTriggerType _triggerType = SpawnerTriggerType.None;

        //public System.Type GetClassType(AbilityType abilityType)
        //{
        //    return GetAbilityFromType(abilityType).GetType();
        //}

        #region Fields

        public TimerTrigger TimerTrigger = new TimerTrigger();

        #endregion


        #region Methods

        public SpawnerTrigger CreateTrigger(SpawnerTriggerType type)
        {
            switch (type)//redo with HasFlag and List
            {
                case SpawnerTriggerType.OneTime:
                    return null;
                case SpawnerTriggerType.Timer:
                    return TimerTrigger;
                default:
                    return null;
            }
        }

        #endregion
    }
}
