using System.Collections.Generic;


namespace BeastHunter
{
    public class DbQuestStorage : IQuestStorage
    {
        #region Fields

        private readonly ISaveManager _agent;

        #endregion


        #region Methods

        public DbQuestStorage(ISaveManager agent)
        {
            _agent = agent;
        }
        
        public Quest GetQuestById(int id)
        {
            return new Quest(QuestRepository.GetById(id));
        }

        public void SaveQuestLog(List<Quest> quests)
        {
            _agent.SaveQuestLog(quests);
        }

        public List<Quest> LoadQuestLog()
        {
            return _agent.LoadQuestLog();
        }

        public void QuestCompleted(int id)
        {
            _agent.QuestCompleted(id);
        }

        public List<int> GetAllCompletedQuests()
        {
            return _agent.GetAllCompletedQuests();
        }

        public List<Quest> GetAllActiveQuests()
        {
            return _agent.GetAllActiveQuests();
        }

        public void SaveGame(string file)
        {
            _agent.SaveGame(file);
        }

        public void LoadGame(string file)
        {
            _agent.LoadGame(file);
        }

        #endregion
    }
}