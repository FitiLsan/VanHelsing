using System.Collections.Generic;


namespace BeastHunter
{
    public interface ISaveManager
    {
        #region Methods

        List<Quest> LoadQuestLog();
        List<Quest> LoadGeneratedQuestLog();
        List<Quest> GetAllActiveQuests();
        List<int> GetAllCompletedQuestsById();
        List<Quest> GetAllCompletedQuests();
        List<Quest> GetAllGeneratedQuest();
        List<int> GetAllActiveQuestsById();
        void SaveQuestLog(List<Quest> quests);
        void SaveGeneratedQuest(Quest quest);
        void QuestCompleted(int id);
        void SaveGame(string file);
        void LoadGame(string file);

        #endregion
    }
}