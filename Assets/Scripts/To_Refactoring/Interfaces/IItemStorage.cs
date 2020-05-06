using System.Collections.Generic;
using Items;

namespace Interfaces
{
    /// <summary>
    /// Прослойка для работы с базой и сейвами
    /// </summary>
    public interface IItemStorage
    {
        /// <summary>
        /// Получаем шаблон предмета
        /// </summary>
        /// <param name="id">Ид предмета</param>
        /// <returns>Предмет</returns>
        Item GetItemById(int id);
        /// <summary>
        /// Регистрируем копию предмета у игрока
        /// </summary>
        /// <param name="item">Предмет</param>
        void NewEntry(Item item);
        /// <summary>
        /// Загрузка инвентаря из сейва
        /// </summary>
        /// <returns>инвентарь</returns>
        List<Item> LoadInventory();
        /// <summary>
        /// Загрузка экипировки из сейва
        /// </summary>
        /// <returns>Экипировка номер слота - вещь</returns>
        Dictionary<int, Item> LoadEquipment();
        /// <summary>
        /// Сохраняем экипировку
        /// </summary>
        /// <param name="equipment">экипировка номер слота - вещь</param>
        void SaveEquipment(Dictionary<int, Item> equipment);
        /// <summary>
        /// Сохраняем инвентраь
        /// </summary>
        /// <param name="inventory">инвентарь</param>
        void SaveInventory(List<Item> inventory);
    }
}