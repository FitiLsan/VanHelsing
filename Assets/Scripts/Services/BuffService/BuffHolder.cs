using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class BuffHolder
    {
        #region Fields
        public event Action<TemporaryBuff> TemporaryBuffAdded;
        public event Action<PermanentBuff> PerammentBuffAdded;
        public event Action<TemporaryBuff> TemporaryBuffRemoved;
        public event Action<PermanentBuff> PerammentBuffRemoved;
        public event Action<EffectType, BaseBuff> BuffEffectEnable;
        public event Action<EffectType> BuffEffectDisable;

        public List<PermanentBuff> PermanentBuffList = new List<PermanentBuff>();
        public List<TemporaryBuff> TemporaryBuffList = new List<TemporaryBuff>();

        #endregion

        #region Methods

        public void AddPermanentBuff(PermanentBuff buff)
        {
            if(PermanentBuffList.Contains(buff))
            {
                return;
            }

            PermanentBuffList.Add(buff);
            PerammentBuffAdded?.Invoke(buff);
            foreach(var effect in buff.Effects)
            {
                BuffEffectEnable?.Invoke(effect.BuffEffectType, buff);
            }
           
        }

        public void AddTemporaryBuff(TemporaryBuff buff)
        {
            if(TemporaryBuffList.Contains(buff))
            {
                return;
            }
            TemporaryBuffList.Add(buff);
            TemporaryBuffAdded?.Invoke(buff);
            foreach (var effect in buff.Effects)
            {
                BuffEffectEnable?.Invoke(effect.BuffEffectType, buff);
            }
        }

        public void RemovePermanentBuff(PermanentBuff buff)
        {
            if (!PermanentBuffList.Contains(buff))
            {
                return;
            }
            PermanentBuffList.Remove(buff);
            PerammentBuffRemoved?.Invoke(buff);
            foreach (var effect in buff.Effects)
            {
                BuffEffectDisable?.Invoke(effect.BuffEffectType);
            }
        }

        public void RemoveTemporaryBuff(TemporaryBuff buff)
        {
            if (!TemporaryBuffList.Contains(buff))
            {
                return;
            }
            TemporaryBuffList.Remove(buff);
            TemporaryBuffRemoved?.Invoke(buff);
            foreach (var effect in buff.Effects)
            {
                BuffEffectDisable?.Invoke(effect.BuffEffectType);
            }

        }


        #endregion
    }
}

