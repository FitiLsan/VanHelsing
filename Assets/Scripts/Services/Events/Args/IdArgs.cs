using System;


namespace BeastHunter
{
    public sealed class IdArgs : EventArgs
    {
        #region Properties

        public int Id { get; }
        public bool isGenerate { get; }

        #endregion


        #region ClassLifeCycle

        public IdArgs(int id)
        {
            Id = id;
        }
        public IdArgs(int id, bool isGenerate)
        {
            Id = id;
            this.isGenerate = isGenerate;
        }

        #endregion
    }
}