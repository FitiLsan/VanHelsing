using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Random = UnityEngine.Random;

namespace BeastHunter
{
    public sealed class BuffService :  IService
    {
        #region Fields
        private const float DELAY_TIME_MIN = 0.75f;
        private const float DELAY_TIME_MAX = 1.25f;

        private delegate void BuffDelegate(Stats stats, float parameter);
        private Dictionary<Buff, BuffDelegate> TemporaryBuffDictionary;
        private Dictionary<Buff, BuffDelegate> PermanentBuffDictionary;
        private GameContext _context;
        private BuffEffect _currentEffect;
        private float _currentDelayTime;

        #endregion


        #region ClassLifeCycles

        public BuffService(GameContext context)
        {
            _context = context;

            TemporaryBuffDictionary = new Dictionary<Buff, BuffDelegate>();
            PermanentBuffDictionary = new Dictionary<Buff, BuffDelegate>();

            PermanentBuffDictionary.Add(Buff.None, NoneDebugBuff);
            PermanentBuffDictionary.Add(Buff.HealthRegenSpeed, HealthRegenBuff);
            PermanentBuffDictionary.Add(Buff.HealthMaximalAmount, HealthMaximumBuff);
            PermanentBuffDictionary.Add(Buff.StaminaRegenSpeed, StaminaRegenBuff);
            PermanentBuffDictionary.Add(Buff.StaminaMaximalAmount, StaminaMaximumBuff);
            PermanentBuffDictionary.Add(Buff.CurrentHealthChangeValue, CurrentHealthChangeValue);
            PermanentBuffDictionary.Add(Buff.SpeedChangeValue, SpeedChangeValue);

            TemporaryBuffDictionary.Add(Buff.None, NoneDebugBuff);
            TemporaryBuffDictionary.Add(Buff.HealthRegenSpeed, HealthRegenBuff);
            TemporaryBuffDictionary.Add(Buff.HealthMaximalAmount, HealthMaximumBuff);
            TemporaryBuffDictionary.Add(Buff.StaminaRegenSpeed, StaminaRegenBuff);
            TemporaryBuffDictionary.Add(Buff.StaminaMaximalAmount, StaminaMaximumBuff);
            TemporaryBuffDictionary.Add(Buff.CurrentHealthChangeValue, CurrentHealthChangeValue);
            TemporaryBuffDictionary.Add(Buff.SpeedChangeValue, SpeedChangeValue);            
        }

        #endregion


        #region Methods

        public void AddPermanentBuff(int instanceID, PermanentBuff buff)
        {
            if (buff == null)
            {
                return;
            }

            var stats = GetStatsByInstanceID(instanceID);
            var buffHolder = stats.BuffHolder;
            if (buffHolder.PermanentBuffList.Contains(buff))
            {
                return;
            }
            foreach (var effect in buff.Effects)
            {
                if (effect.BuffEffectType != EffectType.None && buffHolder.PermanentBuffList.Find(x => x.Effects.Any(y => y.BuffEffectType.Equals(effect.BuffEffectType))))
                {
                    return;
                }
                if (!CalculateProbability(Services.SharedInstance.EffectsManager.GetEffectProbability(effect.BuffEffectType, stats)))
                {
                    return;
                }
                PermanentBuffDictionary[effect.Buff](stats, effect.Value);
            }

            buffHolder.AddPermanentBuff(buff);
        }

        public void RemovePermanentBuff(int instanceID, PermanentBuff buff)
        {
            if (buff == null)
            {
                return;
            }
            var stats = GetStatsByInstanceID(instanceID);
            var buffHolder = stats.BuffHolder;
            if (buffHolder.PermanentBuffList.Contains(buff))
            {
                foreach (var effect in buff.Effects)
                {
                    PermanentBuffDictionary[effect.Buff](stats, -effect.Value);
                }

                buffHolder.RemovePermanentBuff(buff);
            }
            else
            {
             //   throw new System.Exception("There is no such buff at that stats list!");
            }
        }

        public void AddTemporaryBuff(int instanceID, TemporaryBuff buff)
        {
            if(buff==null)
            {
                return;
            }

            var stats = GetStatsByInstanceID(instanceID);
            var buffHolder = stats.BuffHolder;
            var isEffectExist = false;

            if (buffHolder.TemporaryBuffList.Contains(buff))
            {
                return;
            }
            foreach (var effect in buff.Effects)
            {
                if (effect.BuffEffectType!= EffectType.None && buffHolder.TemporaryBuffList.Find(x => x.Effects.Any(y => y.BuffEffectType.Equals(effect.BuffEffectType))))
                {
                    isEffectExist = true;
                    return;
                }

                if(!CalculateProbability(Services.SharedInstance.EffectsManager.GetEffectProbability(effect.BuffEffectType, stats)))
                {
                    return;
                }

                var modifiedBuffValue = buff.Type.Equals(BuffType.Debuf) ? effect.Value : effect.Value * -1;
                
                if (effect.IsTicking)
                {
                    var time = buff.Time;
                     _currentDelayTime = _currentDelayTime == DELAY_TIME_MIN ? DELAY_TIME_MAX : DELAY_TIME_MIN;
                    DOVirtual.DelayedCall(_currentDelayTime, () => BuffUse(time));
                }
                else
                {
                    TemporaryBuffDictionary[effect.Buff](stats, modifiedBuffValue);
                }
                void BuffUse(float time)
                {
                    time--;
                    if (time < 0)
                    {
                        return;
                    }
                    _currentEffect = effect;
                    TemporaryBuffDictionary[effect.Buff](stats, modifiedBuffValue);
                    DOVirtual.DelayedCall(1f, () => BuffUse(time));
                }
            }
            if(isEffectExist)
            {
                return;
            }
            buff.onRemove = DOVirtual.DelayedCall(buff.Time, () => RemoveTemporaryBuff(stats, buff, buffHolder));
            buffHolder.AddTemporaryBuff(buff);
        }

        public void RemoveTemporaryBuff(Stats stats, TemporaryBuff buff, BuffHolder buffHolder)
        {
            if (buff == null)
            {
                return;
            }
            if (buffHolder.TemporaryBuffList.Contains(buff))
            {
                foreach (var effect in buff.Effects)
                {
                    if (!effect.IsTicking)
                    {
                        var modifiedBuffValue = buff.Type.Equals(BuffType.Debuf) ? effect.Value : effect.Value * -1;
                        TemporaryBuffDictionary[effect.Buff](stats, -modifiedBuffValue);
                    }
                }

                buffHolder.RemoveTemporaryBuff(buff);
                buff.onRemove.Kill();
            }
            else
            {
             //   throw new System.Exception("There is no such buff at that stats list!");
            }
        }

        #region Stats Impact

        private void NoneDebugBuff(Stats stats, float value)
        {
            Debug.Log("DebugBuff, Nothing happens ");
        }
        private void HealthRegenBuff(Stats stats, float value)
        {
            stats.BaseStats.HealthRegenPerSecond += value;
            Debug.Log("changed hp regen to " + stats.BaseStats.HealthRegenPerSecond);
        }

        private void HealthMaximumBuff(Stats stats, float value)
        {
            stats.BaseStats.MaximalHealthPoints += value;

            if (stats.BaseStats.CurrentHealthPoints > stats.BaseStats.MaximalHealthPoints)
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

        private void SpeedChangeValue(Stats stats, float value)
        {
            stats.BaseStats.SpeedModifier += value;   
        }

        private void CurrentHealthChangeValue(Stats stats, float value)
        {
            var trace = new System.Diagnostics.StackTrace();
            if (trace.GetFrame(1).GetMethod().Name.Equals("RemoveTemporaryBuff"))
            {
                return;
            }
            var damage = new Damage();
            damage.IsEffectDamage = true;
            damage.ElementDamageType = Services.SharedInstance.EffectsManager.GetElementByEffect(_currentEffect.BuffEffectType);
            damage.ElementDamageValue = value;
            Services.SharedInstance.AttackService.CountAndDealDamage(damage, stats.InstanceID);
        }

        #endregion

        public Stats GetStatsByInstanceID(int receiverID)
        {
            Stats receiverStats = _context.CharacterModel.InstanceID == receiverID ?
                _context.CharacterModel.CurrentStats : _context.NpcModels[receiverID].CurrentStats;
           return receiverStats;
        }

        private bool CalculateProbability(float probabilityValue)
        {
           return Random.Range(0f, 1f) > probabilityValue? true :  false;
        }

        
      

        #endregion
    }
}

