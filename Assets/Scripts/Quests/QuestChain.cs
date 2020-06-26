using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class QuestChain
    {
        public int ChainId;
        public List<int> QuestListId = new List<int>();
        public List<Quest> QuestList = new List<Quest>();

        public QuestChain(int id)
        {
            ChainId = id;
        }
    }
}