using Items;

namespace Interfaces
{
    /// <summary>
    /// Прослойка для работы с базой и сейвами
    /// </summary>
    public interface IItemStorage : IItemSaveAgent
    {
        /// <summary>
        /// Получаем шаблон предмета
        /// </summary>
        /// <param name="id">Ид предмета</param>
        /// <returns>Предмет</returns>
        Item GetItemById(int id);
    }
}