using System;


namespace BeastHunter
{
    public sealed class NpcAreaEnterArgs : EventArgs
    {
        #region Properties

        public int NpcId { get; set; }
        public int AreaId { get; set; }

        #endregion
    }
}