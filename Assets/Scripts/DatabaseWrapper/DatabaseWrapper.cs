using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Events;
using UnityEngine;

namespace DatabaseWrapper
{
    /// <summary>
    ///     Базовая обертка для работы с базой
    /// </summary>
    public static class DatabaseWrapper
    {
        private const string FileName = "world.bytes";
        private static string _dbPath;
        private static SQLiteConnection _connection;


        /// <summary>
        ///     Выполняет запрос без ответа
        /// </summary>
        /// <param name="query">Запрос к базе</param>
        public static void ExecuteQueryWithoutAnswer(string query)
        {
            OpenConnection();
            var command = new SQLiteCommand(_connection) {CommandText = query};
            command.ExecuteNonQuery();
            CloseConnection();
        }

        /// <summary> Этот метод выполняет запрос query и возвращает ответ запроса. </summary>
        /// <param name="query"> Собственно запрос. </param>
        /// <returns> Возвращает значение 1 строки 1 столбца, если оно имеется. </returns>
        public static string ExecuteQueryWithAnswer(string query)
        {
            OpenConnection();
            var command = new SQLiteCommand(_connection) {CommandText = query};
            var answer = command.ExecuteScalar();
            CloseConnection();
            return answer?.ToString();
        }

        /// <summary> Этот метод возвращает таблицу, которая является результатом выборки запроса query. </summary>
        /// <param name="query"> Собственно запрос. </param>
        public static DataTable GetTable(string query)
        {
            OpenConnection();

            var adapter = new SQLiteDataAdapter(query, _connection);

            var ds = new DataSet();
            adapter.Fill(ds);
            adapter.Dispose();

            CloseConnection();

            return ds.Tables[0];
        }

        /// <summary>
        ///     Открываем соединение с базой
        /// </summary>
        private static void OpenConnection()
        {
            if (_connection == null) Init();
            _connection?.Open();
        }


        /// <summary>
        ///     Инитиализация соединения с базой и подписывается на событие закрытия игры
        /// </summary>
        private static void Init()
        {
            _dbPath = GetDatabasePath();
            _connection = new SQLiteConnection("Data Source=" + _dbPath);
            EventManager.StartListening(GameEventTypes.GameExit, CloseConnection);
        }

        /// <summary>
        ///     Закрывает соединение с базой, если оно есть
        /// </summary>
        /// <param name="eventArgs"></param>
        private static void CloseConnection(EventArgs eventArgs = null)
        {
            _connection?.Close();
        }

        /// <summary>
        ///     В зависимости от билда использует нужную базу
        /// </summary>
        /// <returns></returns>
        private static string GetDatabasePath()
        {
#if UNITY_EDITOR
            return Path.Combine(Application.streamingAssetsPath, FileName);
#elif UNITY_STANDALONE
                var filePath = Path.Combine(Application.dataPath, FileName);
                if(!File.Exists(filePath)) UnpackDatabase(filePath);
                return filePath;
#endif
        }

        /// <summary>
        ///     Распаковывает в папку с правами на изменение
        /// </summary>
        /// <param name="filePath"></param>
        private static void UnpackDatabase(string filePath)
        {
            var fromPath = Path.Combine(Application.streamingAssetsPath, FileName);
            File.Copy(fromPath, filePath);
        }
    }
}