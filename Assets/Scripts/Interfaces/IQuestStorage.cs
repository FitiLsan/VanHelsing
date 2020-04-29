using Quests;

namespace Interfaces
{
    /// <summary>
    /// Инфтерфес для работы с базой и с сохранениями
    /// </summary>
    public interface IQuestStorage : IQuestSaveAgent
    {
        /// <summary>
        /// Получаем квест из базы
        /// </summary>
        /// <param name="id">Ид квеста</param>
        /// <returns>квест</returns>
        Quest GetQuestById(int id);
    }
}