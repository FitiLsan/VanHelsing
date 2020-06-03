using System;


namespace BeastHunter
{
    public sealed class QuestArgs : EventArgs
    {
        #region Properties

        public Quest Quest { get; }

        #endregion


        #region ClassLifeCycle

        public QuestArgs(Quest quest)
        {
            Quest = quest;
        }

        #endregion
    }
}