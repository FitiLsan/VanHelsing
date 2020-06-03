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
        private static string _dbPath;

        private static SQLiteConnection _connection;
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
            foreach (DataRow row in _saveData.Tables[SaveTables.active_quest.ToString()].Rows)
            {
                res.Add(row.GetInt("QuestId"),row.GetInt("TimeLeft"));
            }
            return res;
        }

        public Dictionary<int, Quest> GetGeneratedQuests() //
        {
            var res = new Dictionary<int, Quest>();
            var questRows = _saveData.Tables[SaveTables.quest.ToString()].Rows;
            for (int i=0;i< questRows.Count;i++)
            {
                res.Add(questRows[i].GetInt("Id"), new Quest(new QuestDto
                {
                    Id = questRows[i].GetInt("Id"),
                    Title = _saveData.Tables[SaveTables.quest_locale_ru.ToString()].Rows[i].GetString("Title"),
                    Description = _saveData.Tables[SaveTables.quest_locale_ru.ToString()].Rows[i].GetString("Description"),
                    ZoneId = questRows[i].GetInt("ZoneId"),
                    MinLevel = questRows[i].GetInt("MinLevel"),
                    QuestLevel = questRows[i].GetInt("QuestLevel"),
                    TimeAllowed = questRows[i].GetInt("TimeAllowed"),
                    RewardExp = questRows[i].GetInt("RewardExp"),
                    RewardMoney = questRows[i].GetInt("RewardMoney"),
                    StartDialogId = questRows[i].GetInt("StartDialogId"),
                    EndDialogId = questRows[i].GetInt("EndDialogId"),
                    StartQuestEventType = questRows[i].GetInt("StartQuestEventType"),
                    EndQuestEventType = questRows[i].GetInt("EndQuestEventType"),
                    IsRepetable = questRows[i].GetInt("Repeatable") 
                }));
            }
            return res;
        }

        public Dictionary<int, QuestTask> GetGeneratedObjectives()//
        {
            var res = new Dictionary<int, QuestTask>();
            var objectivesRow = _saveData.Tables[SaveTables.quest_objectives.ToString()].Rows;
            for (int i=0; i<objectivesRow.Count;i++)
            {

                bool tempBool = objectivesRow[i].GetInt("IsOptional") == 1 ? true : false;
                int tempTypeNum = objectivesRow[i].GetInt("Type");

                res.Add(objectivesRow[i].GetInt("Id"), new QuestTask(new QuestTaskDto
                {
                    Id = objectivesRow[i].GetInt("Id"),
                    TargetId = objectivesRow[i].GetInt("TargetId"),
                    NeededAmount = objectivesRow[i].GetInt("Amount"),
                    IsOptional = tempBool,
                    QuestId = objectivesRow[i].GetInt("QuestId"),
                    Description = _saveData.Tables[SaveTables.quest_objectives_locale_ru.ToString()].Rows[i].GetString("Title")
                   // Type = objectivesRow[i].GetInt("Type")
                }
                    
                    ));
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
            foreach (DataRow row in _saveData.Tables[SaveTables.active_quest_objectives.ToString()].Rows)
            {
                res.Add(row.GetInt("ObjectiveId"),row.GetInt("Value"));
            }
            return res;
        }

        public Dictionary<int, int> GetCompletedObjectives()
        {
            var res = new Dictionary<int, int>();
            foreach (DataRow row in _saveData.Tables[SaveTables.completed_quests_objectives.ToString()].Rows)
            {
                res.Add(row.GetInt("ObjectiveId"), row.GetInt("Value"));
            }
            return res;
        }

        public void SaveQuestLog(IEnumerable<Quest> quests, List<Quest> completeQuests, List<Quest> generatedQuest)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append("BEGIN TRANSACTION; ");
            foreach (var quest in quests)
            {
                strBuilder.Append($"insert into 'active_quest' (QuestId, TimeLeft) values ({quest.Id}, {(int) quest.RemainingTime}); ");
                foreach (var task in quest.Tasks)
                {
                    strBuilder.Append($"insert into 'active_quest_objectives' (ObjectiveId, Value) values ({task.Id}, {task.CurrentAmount}); ");
                }
            }

            foreach (var quest in completeQuests)
            {
                strBuilder.Append($"insert into 'completed_quests' (QuestId) values ({quest.Id}); ");
                foreach (var task in quest.Tasks)
                {
                    strBuilder.Append($"insert into 'completed_quests_objectives' (ObjectiveId, Value) values ({task.Id}, {task.CurrentAmount});");
                }
            }

            foreach (var quest in generatedQuest)
            {
                SaveGeneratedQuest(quest);
            }

            strBuilder.Append("COMMIT;");
            ExecuteQueryWithoutAnswer(strBuilder.ToString());
        }

        public void SaveGeneratedQuest(Quest quest)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append("BEGIN TRANSACTION; ");

            strBuilder.Append($"insert into 'quest' (Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp," +
                   $" RewardMoney, StartDialogId, EndDialogId, StartQuestEventType, EndQuestEventType, Repeatable) " +
                   $"values ({quest.Id}, {quest.MinLevel}, {quest.QuestLevel}, {quest.TimeAllowed}, {quest.ZoneId}, {quest.RewardExp}, " +
                   $"{quest.RewardMoney}, {quest.StartDialogId}, {quest.EndDialogId}, \"{quest.StartQuestEventType}\", \"{quest.EndQuestEventType}\", {quest.IsRepetable}); ");

            strBuilder.Append($"insert into 'quest_locale_ru' (QuestId, Title, Description) values ({quest.Id}, \"{quest.Title}\", \"{quest.Description}\"); ");
            strBuilder.Append($"update 'last_generated_Id' set questId={quest.Id}; " );

            foreach (var task in quest.Tasks)
            {
                var taskIsOptional = task.IsOptional ? 1 : 0;
                strBuilder.Append($"insert into 'quest_objectives' (Id, QuestId, Type, TargetId, Amount, IsOptional) " +
                    $"values ({task.Id}, {quest.Id}, \"{task.Type}\", {task.TargetId}, {task.NeededAmount}, {taskIsOptional}); ");
                strBuilder.Append($"insert into 'quest_objectives_locale_ru' (ObjectiveId, Title) values ({task.Id}, \"{task.Description}\"); ");
                strBuilder.Append($"update 'last_generated_Id' set objectiveId={task.Id}; ");
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

        public static (int,int) GetLastGeneratedID()
        {
            var lastGeneratedId = GetTable($"select * from 'last_generated_Id'");

            var lastQuestId = lastGeneratedId.Rows[0].GetInt(0);
            var lastObjectiveId = lastGeneratedId.Rows[0].GetInt(1);
            return (lastQuestId, lastObjectiveId);
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

        private static void CloseConnection(EventArgs eventArgs = null)
        {
            _connection?.Close();
        }

        private static void Init()
        {
            _dbPath = GetDatabasePath();
            _connection = new SQLiteConnection("Data Source=" + _dbPath);
            //   Services.SharedInstance.EventManager.StartListening(GameEventTypes.GameExit, CloseConnection); TODO: triggerEvent GameExit;
        }

        private static string GetDatabasePath()
        {
#if UNITY_EDITOR
            var dbPath = Path.Combine(Application.streamingAssetsPath, SAVE_FILE_TAMPLATE);;
            return dbPath;
#elif UNITY_STANDALONE
                var filePath = Path.Combine(Application.dataPath, FileName);
                if(!File.Exists(filePath)) UnpackDatabase(filePath);

                return filePath;
#endif
        }

        public static void ExecuteQueryWithoutAnswer(string query)
        {
            _connection.Open();
            var command = new SQLiteCommand(_connection) {CommandText = query};
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public static void ExecuteQueryWithoutAnswer(SQLiteCommand cmd)
        {
            _connection.Open();
            cmd.Connection = _connection;
            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        public static string ExecuteQueryWithAnswer(string query)
        {
            OpenConnection();
            var command = new SQLiteCommand(_connection) { CommandText = query };
            var answer = command.ExecuteScalar();
            CloseConnection();
            return answer?.ToString();
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