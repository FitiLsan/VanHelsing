using System;
using System.Collections.Generic;
using DatabaseWrapper;
using Interfaces;
using UnityEngine;

namespace Quests
{
    /// <summary>
    ///     Обертка для работы с источниками данных о квестах
    /// </summary>
    public class DbQuestStorage : IQuestStorage
    {
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
        /// <exception cref="NotImplementedException"></exception>
        public void SaveQuestLog(List<Quest> quests)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Загружаем квестлог из сейва
        /// </summary>
        /// <returns></returns>
        public List<Quest> LoadQuestLog()
        {
            Debug.LogError("DbQuestStorage::LoadQuestLog() not implemented yet. Returning empty List<Quest>...");
            return new List<Quest>();
        }

        /// <summary>
        ///     Фиксируем сдачу квеста в сейве
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void QuestCompleted(int id)
        {
            Debug.Log("УРА УРА ВЫПОЛНЕН ПЕРВЫЙ КВЕСТ,ДЕРЖИ НАГРАДУ");
        }

        /// <summary>
        ///     Получаем список выполненых квестов из сейва
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<int> GetAllCompletedQuests()
        {
            Debug.LogError("DbQuestStorage::GetAllCompletedQuests() not implemented yet. Returning empty List<int>...");
            return new List<int>();
        }
    }
}