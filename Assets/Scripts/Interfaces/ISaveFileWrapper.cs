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
        int GetNextItemEntry();
        void AddSaveData(string key, string value);
        void AddSaveData(KeyValuePair<string, string> param);

        #endregion
    }
}