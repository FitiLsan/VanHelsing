using System.Collections.Generic;
using Quests;

namespace Interfaces
{
    /// <summary>
    /// Инфтерфес для работы с базой и с сохранениями
    /// </summary>
    public interface IQuestStorage
    {
        /// <summary>
        /// Получаем квест из базы
        /// </summary>
        /// <param name="id">Ид квеста</param>
        /// <returns>квест</returns>
        Quest GetQuestById(int id);
        /// <summary>
        /// Сохраняем текущий квестлог
        /// </summary>
        /// <param name="quests">квесты</param>
        void SaveQuestLog(List<Quest> quests);
        /// <summary>
        /// Загружаем квестлог из сейва
        /// </summary>
        /// <returns></returns>
        List<Quest> LoadQuestLog();

        /// <summary>
        /// Сообщаем что квест выполнен
        /// </summary>
        /// <param name="id">Ид квеста</param>
        void QuestCompleted(int id);

        /// <summary>
        /// Получаем список всех выполненных квестов
        /// </summary>
        /// <returns>Ид выполненных квестов</returns>
        List<int> GetAllCompletedQuests();
    }
}