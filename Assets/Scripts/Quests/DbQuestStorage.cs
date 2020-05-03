using System.Collections.Generic;
using DatabaseWrapper;
using Quests;

namespace BeastHunter
{
    /// <summary>
    ///     Обертка для работы с источниками данных о квестах
    /// </summary>
    public class DbQuestStorage : IQuestStorage
    {
        private readonly ISaveManager _agent;
        
        public DbQuestStorage(ISaveManager agent)
        {
            _agent = agent;
        }
        
        /// <summary>
        ///     Возвращает квест по его ИД
        /// </summary>
        /// <param name="id">ИД квеста</param>
        /// <returns></returns>
        public Quest GetQuestById(int id)
        {
            return new Quest(QuestRepository.GetById(id));
        }

        /// <summary>
        ///     Сохраняет данные о текущих квестах
        /// </summary>
        /// <param name="quests">Лист квестов из квестлога</param>
        public void SaveQuestLog(List<Quest> quests)
        {
            _agent.SaveQuestLog(quests);
        }

        /// <summary>
        ///     Загружаем квестлог из сейва
        /// </summary>
        /// <returns></returns>
        public List<Quest> LoadQuestLog()
        {
            return _agent.LoadQuestLog();
        }

        /// <summary>
        ///     Фиксируем сдачу квеста в сейве
        /// </summary>
        /// <param name="id"></param>
        public void QuestCompleted(int id)
        {
            _agent.QuestCompleted(id);
        }

        /// <summary>
        ///     Получаем список выполненых квестов из сейва
        /// </summary>
        /// <returns></returns>
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
    }
}