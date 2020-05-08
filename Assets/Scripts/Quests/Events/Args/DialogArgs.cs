using System;


namespace BeastHunter
{
    public sealed class DialogArgs : EventArgs
    {
        #region Properties

        public int Id { get; }
        public int NpcId { get; }

        #endregion


        #region ClassLifeCycle

        public DialogArgs(int id, int npcId)
        {
            Id = id;
            NpcId = npcId;
        }

        #endregion
    }
}