using System.Collections.Generic;
using Items;

namespace Interfaces
{
    public interface IItemSaveAgent
    {
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