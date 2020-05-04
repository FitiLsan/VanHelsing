using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using UnityEngine;


namespace BeastHunter
{
    public static class DatabaseWrapper
    {
        #region Fields

        private const string FILE_NAME = "world.bytes";

        private static string _dbPath;
        private static SQLiteConnection _connection;

        #endregion


        #region Methods

        public static void ExecuteQueryWithoutAnswer(string query)
        {
            OpenConnection();
            var command = new SQLiteCommand(_connection) {CommandText = query};
            command.ExecuteNonQuery();
            CloseConnection();
        }

        public static string ExecuteQueryWithAnswer(string query)
        {
            OpenConnection();
            var command = new SQLiteCommand(_connection) {CommandText = query};
            var answer = command.ExecuteScalar();
            CloseConnection();
            return answer?.ToString();
        }

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

        private static void OpenConnection()
        {
            if (_connection == null) Init();
            _connection?.Open();
        }

        private static void Init()
        {
            _dbPath = GetDatabasePath();
            _connection = new SQLiteConnection("Data Source=" + _dbPath);
            EventManager.StartListening(GameEventTypes.GameExit, CloseConnection);
        }

        private static void CloseConnection(EventArgs eventArgs = null)
        {
            _connection?.Close();
        }

        private static string GetDatabasePath()
        {
#if UNITY_EDITOR
            return Path.Combine(Application.streamingAssetsPath, FILE_NAME);
#elif UNITY_STANDALONE
                var filePath = Path.Combine(Application.dataPath, FileName);
                if(!File.Exists(filePath)) UnpackDatabase(filePath);
                return filePath;
#endif
        }

        private static void UnpackDatabase(string filePath)
        {
            var fromPath = Path.Combine(Application.streamingAssetsPath, FILE_NAME);
            File.Copy(fromPath, filePath);
        }

        #endregion
    }
}