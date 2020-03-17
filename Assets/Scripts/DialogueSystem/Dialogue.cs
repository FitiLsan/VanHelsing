using System;
using System.Collections.Generic;


namespace DialogueSystem
{
    [Serializable]
    public sealed class Dialogue
    {
        #region Properties

        public string NpcText { get; private set; }
        
        public List<PlayerAnswer> PlayerAnswers { get; private set; }

        #endregion


        #region ClassLifeCycles

        public Dialogue(string npcText, List<PlayerAnswer> playerAnswers)
        {
            NpcText = npcText;
            PlayerAnswers = playerAnswers;
        }

        #endregion
    }
}