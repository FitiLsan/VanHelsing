using System.Collections.Generic;
using Items;
using Quests;

namespace Interfaces
{
    public interface ISaveManager : IItemSaveAgent, IQuestSaveAgent
    {
        void SaveGame(string file);
    }
}