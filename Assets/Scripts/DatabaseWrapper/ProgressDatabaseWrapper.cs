using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Extensions;
using Interfaces;
using Quests;
using SaveSystem.SaveDto;
using UnityEngine;

namespace DatabaseWrapper
{
    public class ProgressDatabaseWrapper : ISaveFileWrapper
    {
        private const string SaveFileTemplate = "progress.bytes";
        private SQLiteConnection _connection;
        private readonly DataSet _saveData = new DataSet();
        
        public void CreateNewSave(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
            var fromPath = Path.Combine(Application.streamingAssetsPath, SaveFileTemplate);
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

        public IEnumerable<int> GetCompletedQuests()
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
            var cmd = new SQLiteCommand { CommandText = $"select count(*) from 'save_info' where ParamName ='{key}'"};
           // cmd.Parameters.Add(new SQLiteParameter("@key", key));
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

        public void SaveInventory(IEnumerable<SaveItemDto> items)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append("BEGIN TRANSACTION; ");
            foreach (var x in items)
            {
                strBuilder.Append(
                    $@"insert into '{SaveTables.inventory.ToString()}' 
                    (Entry,
                    ItemId,
                    Count,
                    TimeLeft,
                    ScriptUsed,
                    SpellCharges1,
                    SpellCharges2,
                    Durability) 
                    values 
                    ({x.Entry},
                     {x.ItemId},
                     {x.Count},
                     {x.TimeLeft},
                     {(x.ScriptUsed?1:0)},
                     {x.SpellCharges1},
                     {x.SpellCharges2},
                     {x.Durability});");
            }
            strBuilder.Append("COMMIT;");
            ExecuteQueryWithoutAnswer(strBuilder.ToString());
        }
        public void SaveEquipment(IEnumerable<SaveItemDto> items)
        {
            var strBuilder = new StringBuilder();    
            strBuilder.Append("BEGIN TRANSACTION; ");
            foreach (var x in items)
            {
                strBuilder.Append(
                    $@"insert into '{SaveTables.equipment.ToString()}' 
                    (Entry,
                    ItemId,
                    Count,
                    TimeLeft,
                    ScriptUsed,
                    SpellCharges1,
                    SpellCharges2,
                    Durability,
                    Slot) 
                    values 
                    ({x.Entry},
                     {x.ItemId},
                     {x.Count},
                     {x.TimeLeft},
                     {(x.ScriptUsed?1:0)},
                     {x.SpellCharges1},
                     {x.SpellCharges2},
                     {x.Durability},
                     {x.Slot});");
            }
            strBuilder.Append("COMMIT;");
            ExecuteQueryWithoutAnswer(strBuilder.ToString());
        }

        public IEnumerable<SaveItemDto> LoadInventory()
        {
            foreach (DataRow row in _saveData.Tables[SaveTables.inventory.ToString()].Rows)
            {
                yield return new SaveItemDto
                {
                    Entry = row.GetInt("Entry"),
                    ItemId = row.GetInt("ItemId"),
                    Count = row.GetInt("Count"),
                    TimeLeft = row.GetInt("TimeLeft"),
                    ScriptUsed = row.GetInt("ScriptUsed")==1,
                    Durability = row.GetInt("Durability"),
                    SpellCharges1 = row.GetInt("SpellCharges1"),
                    SpellCharges2 = row.GetInt("SpellCharges2")
                };
            }
        }

        public IEnumerable<SaveItemDto> LoadEquipment()
        {
            foreach (DataRow row in _saveData.Tables[SaveTables.equipment.ToString()].Rows)
            {
                yield return new SaveItemDto
                {
                    Entry = row.GetInt("Entry"),
                    ItemId = row.GetInt("ItemId"),
                    Count = row.GetInt("Count"),
                    TimeLeft = row.GetInt("TimeLeft"),
                    ScriptUsed = row.GetInt("ScriptUsed")==1,
                    Durability = row.GetInt("Durability"),
                    SpellCharges1 = row.GetInt("SpellCharges1"),
                    SpellCharges2 = row.GetInt("SpellCharges2"),
                    Slot = row.GetInt("Slot")
                };
            }
        }

        private enum SaveTables
        {
            // ReSharper disable once InconsistentNaming
            equipment,
            // ReSharper disable once InconsistentNaming
            inventory,
            // ReSharper disable once InconsistentNaming
            quest,
            // ReSharper disable once InconsistentNaming
            quest_objectives,
            // ReSharper disable once InconsistentNaming
            save_info,
            // ReSharper disable once InconsistentNaming
            completed_quests
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
    }
}