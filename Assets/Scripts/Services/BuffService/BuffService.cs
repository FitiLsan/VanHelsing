using System.Collections.Generic;
using System;
using UnityEngine;


namespace BeastHunter
{
    public sealed class BuffService : IService
    {
        #region Fields

        private delegate void BuffDelegate(Stats stats, float parameter);
        private Dictionary<Buff, BuffDelegate> TemporaryBuffDictionary;
        private Dictionary<Buff, BuffDelegate> PermanentBuffDictionary;

        #endregion


        #region ClassLifeCycles

        public BuffService()
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

        public void AddPermanentBuff(Stats stats, PermanentBuff buff, BuffHolder buffHolder)
        {
            foreach (var effect in buff.Effects)
            {
                PermanentBuffDictionary[effect.Buff](stats, effect.Value);
            }

            buffHolder.PermanentBuffList.Add(buff);
        }

        public void RemovePermanentBuff(Stats stats, PermanentBuff buff, BuffHolder buffHolder)
        {
            if (buffHolder.PermanentBuffList.Contains(buff))
            {
                foreach (var effect in buff.Effects)
                {
                    PermanentBuffDictionary[effect.Buff](stats, -effect.Value);
                }

                buffHolder.PermanentBuffList.Remove(buff);
            }
            else
            {
                throw new System.Exception("There is no such buff at that stats list!");
            }
        }

        public void AddTemporaryBuff(Stats stats, TemporaryBuff buff, BuffHolder buffHolder)
        {
            foreach (var effect in buff.Effects)
            {
                TemporaryBuffDictionary[effect.Buff](stats, effect.Value);
            }

            buffHolder.TemporaryBuffList.Add(buff);

            Action laterBuffRemove = () => RemoveTemporaryBuff(stats, buff, buffHolder);

            TimeRemaining buffRemove = new TimeRemaining(laterBuffRemove, buff.Time);
            buffRemove.AddTimeRemaining(buff.Time);
        }

        public void RemoveTemporaryBuff(Stats stats, TemporaryBuff buff, BuffHolder buffHolder)
        {
            if (buffHolder.TemporaryBuffList.Contains(buff))
            {
                foreach (var effect in buff.Effects)
                {
                    TemporaryBuffDictionary[effect.Buff](stats, -effect.Value);
                }

                buffHolder.TemporaryBuffList.Remove(buff);
            }
            else
            {
                throw new System.Exception("There is no such buff at that stats list!");
            }
        }

        private void HealthRegenBuff(Stats stats, float value)
        {
            stats.BaseStats.HealthRegenPerSecond += value;
            Debug.Log("changed hp regen to " + stats.BaseStats.HealthRegenPerSecond);
        }

        private void HealthMaximumBuff(Stats stats, float value)
        {
            stats.BaseStats.MaximalHealthPoints += value;

            if(stats.BaseStats.CurrentHealthPoints > stats.BaseStats.MaximalHealthPoints)
            {
                stats.BaseStats.CurrentHealthPoints = stats.BaseStats.MaximalHealthPoints;
            }

            Debug.Log("changed maximum health to " + stats.BaseStats.MaximalHealthPoints);
        }

        private void StaminaRegenBuff(Stats stats, float value)
        {
            stats.BaseStats.StaminaRegenPerSecond += value;
            Debug.Log("changed stamina regen to " + stats.BaseStats.StaminaRegenPerSecond);
        }

        private void StaminaMaximumBuff(Stats stats, float value)
        {
            stats.BaseStats.MaximalStaminaPoints += value;

            if (stats.BaseStats.CurrentStaminaPoints > stats.BaseStats.MaximalStaminaPoints)
            {
                stats.BaseStats.CurrentStaminaPoints = stats.BaseStats.MaximalStaminaPoints;
            }

            Debug.Log("changed maximum stamina to " + stats.BaseStats.MaximalStaminaPoints);
        }

        #endregion
    }
}

