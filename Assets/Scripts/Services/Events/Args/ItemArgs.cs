using System;


namespace BeastHunter
{
    public sealed class ItemArgs : EventArgs
    {
        #region Properties

        public int ItemId { get; }

        #endregion


        #region ClassLifeCycle

        public ItemArgs(int ItemId)
        {
            this.ItemId = ItemId;
        }

        #endregion
    }
}