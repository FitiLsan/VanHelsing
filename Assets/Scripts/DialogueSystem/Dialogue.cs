using System;
using System.Collections.Generic;

namespace DialogueSystem
{
    [Serializable]
    public class Dialogue
    {
        public string _npcText;

        public List<PlayerAnswer> playerAnswers;


        public Dialogue(string _npcText, List<PlayerAnswer> playerAnswers)
        {
            this._npcText = _npcText;
            this.playerAnswers = playerAnswers;
        }

        public Dialogue()
        {
        }
    }
}