using System.Collections.Generic;
using Quests;


namespace BeastHunter
{
    public interface ISaveManager
    {
        List<Quest> LoadQuestLog();
        List<int> GetAllCompletedQuests();
        List<Quest> GetAllActiveQuests();
        void SaveQuestLog(List<Quest> quests);
        void QuestCompleted(int id);
        void SaveGame(string file);
        void LoadGame(string file);
        
    }
}