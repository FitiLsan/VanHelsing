using System;
using System.Collections.Generic;
using DialogueSystem;


namespace BeastHunter
{
    [Serializable]
    public sealed class Dialogue
    {
        #region Properties

        public string npcText { get; private set; }        
        public List<PlayerAnswer> PlayerAnswers { get; private set; }

        #endregion


        #region ClassLifeCycles

        public Dialogue(string npcText, List<PlayerAnswer> playerAnswers)
        {
            this.npcText = npcText;
            PlayerAnswers = playerAnswers;
        }

        #endregion
    }
}