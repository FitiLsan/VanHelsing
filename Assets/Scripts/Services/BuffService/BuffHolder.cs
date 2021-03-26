using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class BuffHolder
    {
        #region Fields
        public event Action<TemporaryBuff> TemporaryBuffAdded;
        public event Action<PermanentBuff> PerammentBuffAdded;

        public List<PermanentBuff> PermanentBuffList = new List<PermanentBuff>();
        public List<TemporaryBuff> TemporaryBuffList = new List<TemporaryBuff>();

        #endregion

        #region Methods

        public void AddPermanetBuff(PermanentBuff buff)
        {
            PermanentBuffList.Add(buff);
            PerammentBuffAdded?.Invoke(buff);
           
        }

        public void AddTemporaryBuff(TemporaryBuff buff)
        {
            TemporaryBuffList.Add(buff);
            TemporaryBuffAdded?.Invoke(buff);
        }

        #endregion
    }
}

