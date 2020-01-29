using System;

namespace Events.Args
{
    public class NpcAreaEnterArgs : EventArgs
    {
        public int NpcId { get; set; }
        public int AreaId { get; set; }
    }
}