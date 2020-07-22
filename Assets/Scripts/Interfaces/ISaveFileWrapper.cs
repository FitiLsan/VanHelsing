using System.Collections.Generic;

namespace BeastHunter
{
    public interface ISaveFileWrapper
    {
        #region Methods

        void CreateNewSave(string file);
        void LoadSave(string file);
        IEnumerable<int> GetCompletedQuestsId();
        Dictionary<int, bool> GetCompletedQuests();
        Dictionary<int, int> GetActiveQuests();
        Dictionary<int, int> GetActiveObjectives();
        Dictionary<int, Quest> GetGeneratedQuests();
        Dictionary<int, QuestTask> GetGeneratedObjectives();        
        Dictionary<int, int> GetCompletedObjectives();
        void SaveQuestLog(IEnumerable<Quest> quests, List<Quest> completeQuests, List<Quest> generatedQuests);
        void SaveGeneratedQuest(Quest quest);
        int GetNextItemEntry();
        void AddSaveData(string key, string value);
        void AddSaveData(KeyValuePair<string, string> param);

        #endregion
    }
}