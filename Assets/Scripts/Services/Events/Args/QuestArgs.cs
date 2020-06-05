using System;


namespace BeastHunter
{
    public sealed class QuestArgs : EventArgs
    {
        #region Properties

        public QuestDto Quest { get; }

        #endregion


        #region ClassLifeCycle

        public QuestArgs(QuestDto quest)
        {
            Quest = quest;
        }

        #endregion
    }
}