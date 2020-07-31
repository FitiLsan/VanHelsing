using System;


namespace BeastHunter
{
    public sealed class CardArgs : EventArgs
    {
        #region Properties

        public int CardId { get; }

        #endregion


        #region ClassLifeCycle

        public CardArgs(int cardId)
        {
            CardId = cardId;
        }

        #endregion
    }
}