using System.Collections.Generic;


namespace BeastHunter
{
    [System.Serializable]
    public sealed class BuffHolder
    {
        #region Fields

        public List<PermanentBuff> PermanentBuffList;
        public List<TemporaryBuff> TemporaryBuffList;

        #endregion
    }
}

