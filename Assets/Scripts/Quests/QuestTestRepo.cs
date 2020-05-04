using System.Collections.Generic;
using System.Data;


namespace BeastHunter
{
    public sealed class QuestTest
    {
        public QuestTest(int id)
        {
            this.id = id;
            isCompleted = false;
            isActive = false;
        }
        public int id;
        public bool isCompleted;
        public bool isActive;
    }
    public sealed class QuestTestRepo
    {
        List<QuestTest> questList = new List<QuestTest>();

        public void GetAllQuest(int zoneId)
        {
            var dtZ = DatabaseWrapper.GetTable($"select Id from 'quest' where ZoneId={zoneId};");
            foreach (DataRow row in dtZ.Rows)
            {
                questList.Add(new QuestTest(row.GetInt(0)));
            }
        }

        public void SetCompletedQuest(int id)
        {
            foreach (QuestTest quest in questList)
            {
                if (quest.id == id)
                {
                    quest.isCompleted = true;
                }
            }
        }
        public void SetActiveQuest(int id)
        {
            foreach (QuestTest quest in questList)
            {
                if (quest.id == id)
                {
                    quest.isActive = true;
                }
            }
        }
        public QuestTest GetQuestById(int id)
        {
            foreach (QuestTest quest in questList)
            {
                if (quest.id == id)
                {
                    return quest;
                }
            }
            return null;
        }
    }
}
