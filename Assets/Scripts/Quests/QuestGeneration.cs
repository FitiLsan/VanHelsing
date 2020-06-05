using System.Collections.Generic;

namespace BeastHunter
{
    public class QuestGeneration
    {
        //public static (int, int) lastGeneratedId;
        public static int lastGeneratedQuestId;//= lastGeneratedId.Item1;
        private static int lastGeneratedObjectiveId;// = lastGeneratedId.Item2;
        public static int lastGeneratedAnswerId;
        public static int lastGeneratedNodeId;
        public static QuestDto TempGenerationQuest;

        public static QuestDto QuestGenerate()
        {
            if (TempGenerationQuest == null)
            {
                var newQuest = new QuestDto
                {
                    Id = ++lastGeneratedQuestId, //
                    Title = $"test Generate Quest {lastGeneratedQuestId}",
                    Description = "generating bla bla bla",
                    ZoneId = 1,
                    MinLevel = 1,
                    QuestLevel = 1,
                    TimeAllowed = -1,
                    RewardExp = 0,
                    RewardMoney = 0,
                    StartDialogId = ++lastGeneratedAnswerId, //?
                    EndDialogId = ++lastGeneratedAnswerId, //?
                    IsRepetable = 0,
                    Tasks = QuestTasksGenerate(3)

                };
                if (QuestRepository.GetById(newQuest.Id) == null)
                {     
                    TempGenerationQuest = newQuest;
                   // QuestRepository.AddRowToDialogueCaches(newQuest);
                    Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.SaveGeneratedQuest, new QuestArgs(TempGenerationQuest));
                }
            }
            QuestRepository.AddRowToDialogueCaches(TempGenerationQuest);
            return TempGenerationQuest;
        }

        public static List<QuestTaskDto> QuestTasksGenerate(int count)
        {
            var taskList = new List<QuestTaskDto>();
            for (int i = 1; i <= count; i++)
            {
                var newTask = new QuestTaskDto
                {
                    Id = ++lastGeneratedObjectiveId, //need use last generate ID +1
                    Description = $"new task #{lastGeneratedObjectiveId}",
                    NeededAmount = 1,
                    IsOptional = false,
                    TargetId = ++lastGeneratedAnswerId, // need unic
                    Type = QuestTaskTypes.AnswerSelect
                };
                taskList.Add(newTask);
            }
            return taskList;
        }
        public static QuestDto GetTempQuest()
        {
                return TempGenerationQuest; //?? QuestGenerate();
        }
        public static void ClearTempQuest()
        {
            TempGenerationQuest = null;
        }
        public static void SetTempLastGeneratedQuest()
        {
            var quest = QuestRepository.GetById(lastGeneratedQuestId);
            if (quest != null)
            {
                TempGenerationQuest = quest;
            }
        }
        public static void LoadLastGeneratedId()
        {
            var id = ProgressDatabaseWrapper.GetLastGeneratedID();
            lastGeneratedQuestId = id.Item1;
            lastGeneratedObjectiveId = id.Item2;
            lastGeneratedAnswerId = id.Item3;
            lastGeneratedNodeId = id.Item4;
        }
    }
}
