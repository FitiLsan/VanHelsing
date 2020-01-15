using System;

namespace Events.Args
{
    public class DialogArgs : EventArgs
    {
        public DialogArgs(int id, int npcId)
        {
            Id = id;
            NpcId = npcId;
        }

        public int Id { get; }
        public int NpcId { get; }
    }
}