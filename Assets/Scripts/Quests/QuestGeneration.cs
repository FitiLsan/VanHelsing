using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class QuestGeneration
    {
        public static int lastGeneratedQuestId;
        private static int lastGeneratedObjectiveId;
        public static int lastGeneratedAnswerId;
        public static int lastGeneratedNodeId;
        public static QuestDto TempGenerationQuest;

        public static QuestDto QuestGenerate()
        {
            Random rnd = new Random();
            
            if (TempGenerationQuest == null)
            {
                var newQuest = new QuestDto
                {
                    Id = ++lastGeneratedQuestId,
                    Title = $"test Generate Quest {lastGeneratedQuestId}",
                    Description = "generating bla bla bla",
                    ZoneId = 1,
                    MinLevel = 1,
                    QuestLevel = 1,
                    TimeAllowed = -1,
                    RewardExp = 0,
                    RewardMoney = 0,
                    StartDialogId = ++lastGeneratedAnswerId, 
                    EndDialogId = ++lastGeneratedAnswerId, 
                    IsRepetable = 0,
                    Tasks = QuestTasksGenerate(rnd.Next(1, 4))

                };
                if (QuestRepository.GetById(newQuest.Id) == null)
                {     
                    TempGenerationQuest = newQuest;
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
                    Id = ++lastGeneratedObjectiveId,
                    Description = $"new task #{lastGeneratedObjectiveId}",
                    NeededAmount = 1,
                    IsOptional = false,
                    TargetId = ++lastGeneratedAnswerId,
                    Type = QuestTaskTypes.AnswerSelect
                };
                taskList.Add(newTask);
            }
            return taskList;
        }
        public static QuestDto GetTempQuest()
        {
                return TempGenerationQuest;
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
