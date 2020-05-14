using System;


namespace BeastHunter
{
    public sealed class IdArgs : EventArgs
    {
        #region Properties

        public int Id { get; }

        #endregion


        #region ClassLifeCycle

        public IdArgs(int id)
        {
            Id = id;
        }

        #endregion
    }
}