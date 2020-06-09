using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


namespace BeastHunter
{
    public static class QuestRepository
    {
        #region Fields

        private static Dictionary<int, QuestDto> _cache = new Dictionary<int, QuestDto>();
        private static DataTable _dialogueCache = new DataTable();
        private static DataTable _questTaskCache = new DataTable();
        private static Locale _locale = Locale.RU;
        private static readonly Dictionary<Locale, (string, string)> _localeTables = new Dictionary<Locale, (string, string)>
            {
                //TODO: add another languages
                {Locale.RU, ("quest_locale_ru", "quest_objectives_locale_ru")}
            };

        #region ColumnsDefinitions

        //table : Quest
        private const byte QUEST_ID = 0;
        private const byte QUEST_MINLEVEL = 1;
        private const byte QUEST_LEVEL = 2;
        private const byte QUEST_TIMEALLOWED = 3;
        private const byte QUEST_ZONEID = 4;
        private const byte QUEST_REWARDEXP = 5;
        private const byte QUEST_REWARDMONEY = 6;
        private const byte QUEST_STARTDIALOGID = 7;
        private const byte QUEST_ENDDIALOGID = 8;
        private const byte QUEST_STARTQUESTEVENTTYPE = 9;
        private const byte QUEST_ENDQUESTEVENTTYPE = 10;
        private const byte QUEST_ISREPETABLE = 11;

        //table : Quest_locale_xx
        private const byte QUEST_LOCALE_ID = 0;
        private const byte QUEST_LOCALE_QUESTID = 1;
        private const byte QUEST_LOCALE_TITLE = 2;
        private const byte QUEST_LOCALE_DESCRIPTION = 3;

        //table : quest_objectives
        private const byte QUEST_OBJECTIVES_ID = 0;
        private const byte QUEST_OBJECTIVES_QUESTID = 1;
        private const byte QUEST_OBJECTIVES_TYPE = 2;
        private const byte QUEST_OBJECTIVES_TARGETID = 3;
        private const byte QUEST_OBJECTIVES_AMOUNT = 4;
        private const byte QUEST_OBJECTIVES_ISOPTIONAL = 5;

        //table : quest_objectives_locale_xx
        private const byte QUEST_OBJECTIVES_LOCALE_ID = 0;
        private const byte QUEST_OBJECTIVES_LOCALE_OBJECTIVEID = 1;
        private const byte QUEST_OBJECTIVES_LOCALE_TITLE = 2;

        //table : quest_poi
        private const byte QUEST_POI_ID = 0;
        private const byte QUEST_POI_QUESTID = 1;
        private const byte QUEST_POI_ZONEID = 2;
        private const byte QUEST_POI_X = 3;
        private const byte QUEST_POI_Y = 4;
        private const byte QUEST_POI_MARKERTYPE = 5;

        //table : quest_requirements
        private const byte QUEST_REQUIREMENTS_ID = 0;
        private const byte QUEST_REQUIREMENTS_TARGETQUESTID = 1;
        private const byte QUEST_REQUIREMENTS_REQUIREDQUEST = 2;

        //table : quest_rewards
        private const byte QUEST_REWARDS_ID = 0;
        private const byte QUEST_REWARDS_QUESTID = 1;
        private const byte QUEST_REWARDS_REWARDTYPE = 2;
        private const byte QUEST_REWARDS_REWARDOBJECTTYPE = 3;
        private const byte QUEST_REWARDS_REWARDOBJECTID = 4;
        private const byte QUEST_REWARDS_REWARDOBJECTCOUNT = 5;

        #endregion

        #region PoiMarkerType

        private const int POI_TASK = 1;
        private const int POI_START = 2;
        private const int POI_END = 3;

        #endregion

        #endregion


        #region Methods

        public static DataTable GetDialogueCache()
        {
            try
            {
                if (_dialogueCache.Rows.Count == 0)
                {
                    _dialogueCache = DatabaseWrapper.GetTable($"select * from 'dialogue_answers' where Quest_ID!= 0;");
                }
                return _dialogueCache;
            }
            catch (Exception e)
            {
                Debug.LogError($"{DateTime.Now.ToShortTimeString()}    dialogueCache error     {e}\n");
                throw;
            }
        }

        public static DataTable GetQuestTaskCache()
        {
            try
            {
                if (_questTaskCache.Rows.Count == 0)
                {
                    _questTaskCache = DatabaseWrapper.GetTable($"select quest_objectives.Id, QuestId, TargetID, dialogue_answers.Npc_id from 'quest_objectives'" +
                   $" INNER Join 'dialogue_answers' on quest_objectives.TargetId = dialogue_answers.Id where Type = 8;");
                }
                return _questTaskCache;
            }
            catch (Exception e)
            {
                Debug.LogError($"{DateTime.Now.ToShortTimeString()}    questTaskCache error     {e}\n");
                throw;
            }
        }

        public static QuestDto GetById(int id)
        {
            try
            {
                if (_cache.ContainsKey(id)) return _cache[id];
                var dtQ = DatabaseWrapper.GetTable($"select * from 'quest' where Id={id};");
                var dtLoc = DatabaseWrapper.GetTable($"select * from '{GetQuestLocaleTable()}' where QuestId={id} limit 1;");
                var dtObj = DatabaseWrapper.GetTable($"select * from 'quest_objectives' where QuestId={id};");
                var dtPoi = DatabaseWrapper.GetTable($"select * from 'quest_poi' where QuestId={id};");
                var dtReq = DatabaseWrapper.GetTable($"select * from 'quest_requirements' where TargetQuestId={id};");
                var dtRew = DatabaseWrapper.GetTable($"select * from 'quest_rewards' where QuestId={id};");

                if (dtQ.Rows.Count == 0)
                {
                    throw new Exception($"Quest with Id[{id}] not found!");
                }
                
                var questDto = new QuestDto
                {
                    Id = id,
                    Title = dtLoc.Rows[0].GetString(QUEST_LOCALE_TITLE),
                    Description = dtLoc.Rows[0].GetString(QUEST_LOCALE_DESCRIPTION),
                    RewardExp = dtQ.Rows[0].GetInt(QUEST_REWARDEXP),
                    RewardMoney = dtQ.Rows[0].GetInt(QUEST_REWARDMONEY),
                    MinLevel = dtQ.Rows[0].GetInt(QUEST_MINLEVEL),
                    QuestLevel = dtQ.Rows[0].GetInt(QUEST_LEVEL),
                    ZoneId = dtQ.Rows[0].GetInt(QUEST_ZONEID),
                    TimeAllowed = dtQ.Rows[0].GetInt(QUEST_TIMEALLOWED),
                    StartDialogId = dtQ.Rows[0].GetInt(QUEST_STARTDIALOGID),
                    EndDialogId = dtQ.Rows[0].GetInt(QUEST_ENDDIALOGID),
                    IsRepetable = dtQ.Rows[0].GetInt(QUEST_ISREPETABLE)
                };

                foreach (DataRow row in dtReq.Rows)
                    questDto.RequiredQuests.Add(row.GetInt(QUEST_REQUIREMENTS_REQUIREDQUEST));

                foreach (DataRow row in dtPoi.Rows)
                {
                    var t = row.GetInt(QUEST_POI_MARKERTYPE);
                    var marker = new QuestMarkerDto
                    {
                        MapId = row.GetInt(QUEST_POI_ZONEID),
                        X = row.GetFloat(QUEST_POI_X),
                        Y = row.GetFloat(QUEST_POI_Y)
                    };
                    switch (t)
                    {
                        case POI_START:
                            questDto.QuestStart = marker;
                            break;
                        case POI_END:
                            questDto.QuestEnd = marker;
                            break;
                        default:
                            questDto.MapMarkers.Add(marker);
                            break;
                    }
                }

                foreach (DataRow row in dtObj.Rows)
                {
                    var tid = row.GetInt(QUEST_OBJECTIVES_ID);
                    var tmp = DatabaseWrapper.ExecuteQueryWithAnswer($"select Title from '{GetQuestObjectivesLocaleTable()}' where ObjectiveId={tid} limit 1;");
                    var task = new QuestTaskDto
                    {
                        Id = row.GetInt(QUEST_OBJECTIVES_ID),
                        TargetId = row.GetInt(QUEST_OBJECTIVES_TARGETID),
                        NeededAmount = row.GetInt(QUEST_OBJECTIVES_AMOUNT),
                        Type = (QuestTaskTypes) row.GetInt(QUEST_OBJECTIVES_TYPE),
                        Description = tmp,
                        IsOptional = row.GetInt(QUEST_OBJECTIVES_ISOPTIONAL) == 1
                    };
					questDto.Tasks.Add(task);
                }

                foreach (DataRow row in dtRew.Rows)
                    questDto.Rewards.Add(new QuestRewardDto
                    {
                        ObjectId = row.GetInt(QUEST_REWARDS_REWARDOBJECTID),
                        ObjectCount = row.GetInt(QUEST_REWARDS_REWARDOBJECTCOUNT),
                        ObjectType = (QuestRewardObjectTypes) row.GetInt(QUEST_REWARDS_REWARDOBJECTTYPE),
                        Type = (QuestRewardTypes) row.GetInt(QUEST_REWARDS_REWARDTYPE)
                    });

                _cache.Add(id, questDto);
                return questDto;
            }
            catch (Exception e)
            {
                Debug.LogError($"{DateTime.Now.ToShortTimeString()}    Quest with id[{id}] fires exception    {e}\n");
                throw;
            }
        }

        private static string GetQuestLocaleTable()
        {
            return _localeTables.ContainsKey(_locale) ? _localeTables[_locale].Item1 : _localeTables[Locale.RU].Item1;
        }

        private static string GetQuestObjectivesLocaleTable()
        {
            return _localeTables.ContainsKey(_locale) ? _localeTables[_locale].Item2 : _localeTables[Locale.RU].Item2;
        }

        public static IEnumerable<QuestDto> GetAllInZone(int zoneId)
        {
            var dtZ = DatabaseWrapper.GetTable($"select Id from 'quest' where ZoneId={zoneId};");
            foreach (DataRow row in dtZ.Rows) yield return GetById(row.GetInt(0));
        }

        public static void ClearCache()
        {
            _cache = new Dictionary<int, QuestDto>();
        }

        public static void SetLocale(Locale locale)
        {
            _locale = locale;
        }

        public static void Init()
        {
            //TODO: Событие смены языка 
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestReported, OnQuestReported);
        }

        private static void OnQuestReported(EventArgs arg0)
        {
            if (!(arg0 is IdArgs idArgs)) return;
            if (_cache.ContainsKey(idArgs.Id))
                _cache.Remove(idArgs.Id);
        }

        public static Locale GetCurrentLocale()
        {
            return _locale;
        }

        #endregion
    }
}