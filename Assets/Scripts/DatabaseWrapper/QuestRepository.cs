using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


namespace BeastHunter
{
    public static class QuestRepository
    {
        #region Fields

        private static DataTable _dialogueCache = new DataTable();
        private static DataTable _questTaskCache = new DataTable();


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

        #endregion
    }
}