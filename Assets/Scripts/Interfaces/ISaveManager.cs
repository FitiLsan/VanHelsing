using System.Collections.Generic;


namespace BeastHunter
{
    public interface ISaveManager
    {
        #region Methods

        List<Quest> LoadQuestLog();
        List<Quest> GetAllActiveQuests();
        List<int> GetAllCompletedQuests();
        List<int> GetAllActiveQuestsById();
        void SaveQuestLog(List<Quest> quests);
        void QuestCompleted(int id);
        void SaveGame(string file);
        void LoadGame(string file);

        #endregion
    }
}