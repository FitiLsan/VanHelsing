using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class BuffService : Service
    {
        #region Fields

        private delegate void TemporaryBuffDelegate(StatsClass stats, float parameter, float time);
        private delegate void PermanentBuffDelegate(StatsClass stats, float parameter);

        private Dictionary<Buff, TemporaryBuffDelegate> TemporaryBuffDictionary;
        private Dictionary<Buff, PermanentBuffDelegate> PermanentBuffDictionary;

        #endregion


        #region ClassLifeCycles

        public BuffService(Contexts contexts) : base(contexts)
        {
            TemporaryBuffDictionary = new Dictionary<Buff, TemporaryBuffDelegate>();
            PermanentBuffDictionary = new Dictionary<Buff, PermanentBuffDelegate>();

            PermanentBuffDictionary.Add(Buff.HealthRegenSpeed, HealthRegenBuff);
            PermanentBuffDictionary.Add(Buff.HealthMaximalAmount, HealthMaximumBuff);
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

        #endregion
    }
}

