using System.Collections.Generic;
using System;
using UnityEngine;


namespace BeastHunter
{
    public sealed class BuffService : Service
    {
        #region Fields

        private delegate void BuffDelegate(StatsClass stats, float parameter);
        private Dictionary<Buff, BuffDelegate> TemporaryBuffDictionary;
        private Dictionary<Buff, BuffDelegate> PermanentBuffDictionary;

        #endregion


        #region ClassLifeCycles

        public BuffService(Contexts contexts) : base(contexts)
        {
            TemporaryBuffDictionary = new Dictionary<Buff, BuffDelegate>();
            PermanentBuffDictionary = new Dictionary<Buff, BuffDelegate>();

            PermanentBuffDictionary.Add(Buff.HealthRegenSpeed, HealthRegenBuff);
            PermanentBuffDictionary.Add(Buff.HealthMaximalAmount, HealthMaximumBuff);
            PermanentBuffDictionary.Add(Buff.StaminaRegenSpeed, StaminaRegenBuff);
            PermanentBuffDictionary.Add(Buff.StaminaMaximalAmount, StaminaMaximumBuff);

            TemporaryBuffDictionary.Add(Buff.HealthRegenSpeed, HealthRegenBuff);
            TemporaryBuffDictionary.Add(Buff.HealthMaximalAmount, HealthMaximumBuff);
            TemporaryBuffDictionary.Add(Buff.StaminaRegenSpeed, StaminaRegenBuff);
            TemporaryBuffDictionary.Add(Buff.StaminaMaximalAmount, StaminaMaximumBuff);
        }

        #endregion


        #region Methods

        public void AddPermanentBuff(StatsClass stats, PermanentBuffClass buff)
        {
            foreach (var effect in buff.Effects)
            {
                PermanentBuffDictionary[effect.Buff](stats, effect.Value);
            }

            stats.PermantnsBuffList.Add(buff);
        }

        public void RemovePermanentBuff(StatsClass stats, PermanentBuffClass buff)
        {
            if (stats.PermantnsBuffList.Contains(buff))
            {
                foreach (var effect in buff.Effects)
                {
                    PermanentBuffDictionary[effect.Buff](stats, -effect.Value);
                }

                stats.PermantnsBuffList.Remove(buff);
            }
            else
            {
                throw new System.Exception("There is no such buff at that stats list!");
            }         
        }

        public void AddTemporaryBuff(StatsClass stats, TemporaryBuffClass buff)
        {
            foreach (var effect in buff.Effects)
            {
                TemporaryBuffDictionary[effect.Buff](stats, effect.Value);
            }

            stats.TemporaryBuffList.Add(buff);

            Action laterBuffRemove = () => RemoveTemporaryBuff(stats, buff);

            TimeRemaining buffRemove = new TimeRemaining(laterBuffRemove, buff.Time);
            buffRemove.AddTimeRemaining(buff.Time);
        }

        public void RemoveTemporaryBuff(StatsClass stats, TemporaryBuffClass buff)
        {
            if (stats.TemporaryBuffList.Contains(buff))
            {
                foreach (var effect in buff.Effects)
                {
                    TemporaryBuffDictionary[effect.Buff](stats, -effect.Value);
                }

                stats.TemporaryBuffList.Remove(buff);
            }
            else
            {
                throw new System.Exception("There is no such buff at that stats list!");
            }
        }

        private void HealthRegenBuff(StatsClass stats, float value)
        {
            stats._healthRegenPerSecond += value;
            Debug.Log("changed hp regen to " + stats._healthRegenPerSecond);
        }

        private void HealthMaximumBuff(StatsClass stats, float value)
        {
            stats._maximalHealthPoints += value;

            if(stats._currentHealthPoints > stats._maximalHealthPoints)
            {
                stats._currentHealthPoints = stats._maximalHealthPoints;
            }

            Debug.Log("changed maximum health to " + stats._maximalHealthPoints);
        }

        private void StaminaRegenBuff(StatsClass stats, float value)
        {
            stats._staminaRegenPerSecond += value;
            Debug.Log("changed stamina regen to " + stats._staminaRegenPerSecond);
        }

        private void StaminaMaximumBuff(StatsClass stats, float value)
        {
            stats._maximalStaminaPoints += value;

            if (stats._currentStaminaPoints > stats._maximalStaminaPoints)
            {
                stats._currentStaminaPoints = stats._maximalStaminaPoints;
            }

            Debug.Log("changed maximum stamina to " + stats._maximalStaminaPoints);
        }

        #endregion
    }
}

