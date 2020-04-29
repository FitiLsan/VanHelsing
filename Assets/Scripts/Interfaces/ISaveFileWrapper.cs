using System.Collections.Generic;
using Quests;
using SaveSystem.SaveDto;

namespace Interfaces
{
    public interface ISaveFileWrapper
    {
        void CreateNewSave(string file);
        void LoadSave(string file);
        IEnumerable<int> GetCompletedQuests();
        Dictionary<int, int> GetActiveQuests();
        Dictionary<int, int> GetActiveObjectives();

        void SaveQuestLog(IEnumerable<Quest> quests, List<int> completeQuests);

        int GetNextItemEntry();
        void AddSaveData(string key, string value);
        void AddSaveData(KeyValuePair<string, string> param);

        void SaveInventory(IEnumerable<SaveItemDto> items);

        void SaveEquipment(IEnumerable<SaveItemDto> items);

        IEnumerable<SaveItemDto> LoadInventory();
        IEnumerable<SaveItemDto> LoadEquipment();
    }
}