using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using UnityEngine;


namespace BeastHunter
{
    public class ProgressDatabaseWrapper : ISaveFileWrapper
    {
        #region Fields

        private const string SAVE_FILE_TAMPLATE = "progress.bytes";

        private SQLiteConnection _connection;
        private readonly DataSet _saveData = new DataSet();

        #endregion


        #region Methods

        public void CreateNewSave(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
            var fromPath = Path.Combine(Application.streamingAssetsPath, SAVE_FILE_TAMPLATE);
            File.Copy(fromPath, file);
            _connection = new SQLiteConnection("Data Source="+file);
        }

        public void LoadSave(string file)
        {
            _connection?.Close();
            if (!File.Exists(file))
                CreateNewSave(file);
            _connection = new SQLiteConnection("Data Source=" + file);
            foreach (var table in Enum.GetNames(typeof(SaveTables)))
            {
                var dt = GetDataTable($"select * from {table};");
                dt.TableName = table;
                _saveData.Tables.Add(dt);
            }
        }

        public IEnumerable<int> GetCompletedQuestsId()
        {
            foreach (DataRow row in _saveData.Tables[SaveTables.completed_quests.ToString()].Rows)
            {
                yield return row.GetInt("QuestId");
            }
        }

        public Dictionary<int, int> GetActiveQuests()
        {
            var res = new Dictionary<int,int>();
            foreach (DataRow row in _saveData.Tables[SaveTables.quest.ToString()].Rows)
            {
                res.Add(row.GetInt("QuestId"),row.GetInt("TimeLeft"));
            }
            return res;
        }

        public Dictionary<int, bool> GetCompletedQuests()
        {
            var res = new Dictionary<int, bool>();
            foreach (DataRow row in _saveData.Tables[SaveTables.completed_quests.ToString()].Rows)
            {
                res.Add(row.GetInt("QuestId"), true);
            }
            return res;
        }

        public Dictionary<int, int> GetActiveObjectives()
        {
            var res = new Dictionary<int,int>();
            foreach (DataRow row in _saveData.Tables[SaveTables.quest_objectives.ToString()].Rows)
            {
                res.Add(row.GetInt("ObjectiveId"),row.GetInt("Value"));
            }
            return res;
        }

        public void SaveQuestLog(IEnumerable<Quest> quests, List<int> completeQuests)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append("BEGIN TRANSACTION; ");
            foreach (var quest in quests)
            {
                strBuilder.Append($"insert into 'quest' (QuestId, TimeLeft) values ({quest.Id}, {(int) quest.RemainingTime}); ");
                foreach (var task in quest.Tasks)
                {
                    strBuilder.Append($"insert into 'quest_objectives' (ObjectiveId, Value) values ({task.Id}, {task.CurrentAmount}); ");
                }
            }

            foreach (var id in completeQuests)
            {
                strBuilder.Append($"insert into 'completed_quests' (QuestId) values ({id}); ");
            }

            strBuilder.Append("COMMIT;");
            ExecuteQueryWithoutAnswer(strBuilder.ToString());
        }

        public int GetNextItemEntry()
        {
            foreach (DataRow row in _saveData.Tables[SaveTables.save_info.ToString()].Rows)
            {
                if (row.GetString("ParamName") == "NextEntry") return row.GetInt("ParamValue");
            }
            return 1;
        }

        public void AddSaveData(string key, string value)
        {
            var cmd = new SQLiteCommand { CommandText = $"select count(*) from 'save_info' where ParamName = @key"};
            cmd.Parameters.Add(new SQLiteParameter("@key", key));
            var c = Convert.ToInt32( ExecuteScalar(cmd));
            cmd.CommandText = c != 0
                ? "update 'save_info' set ParamValue = @value where ParamName = @key;"
                : "insert into 'save_info' (ParamName, ParamValue) VALUES (@key, @value)";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SQLiteParameter("@key", key));
            cmd.Parameters.Add(new SQLiteParameter("@value", value));
            ExecuteQueryWithoutAnswer(cmd);
        }

        public void AddSaveData(KeyValuePair<string, string> param)
        {
            AddSaveData(param.Key, param.Value);
        }
        
        private DataTable GetDataTable(string sql)
        {
            try
            {
                var dt = new DataTable();
                using (var c = new SQLiteConnection(_connection))
                {
                    c.Open();
                    using (var cmd = new SQLiteCommand(sql, c))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }
        
        private void ExecuteQueryWithoutAnswer(string query)
        {
            _connection.Open();
            var command = new SQLiteCommand(_connection) {CommandText = query};
            command.ExecuteNonQuery();
            _connection.Close();
        }

        private void ExecuteQueryWithoutAnswer(SQLiteCommand cmd)
        {
            _connection.Open();
            cmd.Connection = _connection;
            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        private object ExecuteScalar(string query)
        {
            _connection.Open();
            var command = new SQLiteCommand(_connection) {CommandText = query};
            var r = command.ExecuteScalar();
            _connection.Close();
            return r;
        }
        
        private object ExecuteScalar(SQLiteCommand cmd)
        {
            _connection.Open();
            cmd.Connection = _connection;
            var r = cmd.ExecuteScalar();    
            _connection.Close();
            return r;
        }

        #endregion

    }
}