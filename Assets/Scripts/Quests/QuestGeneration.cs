using System.Collections.Generic;

namespace BeastHunter
{
    public  class QuestGeneration
    {
        public static Quest TempGenerationQuest;

        public static Quest QuestGenerate()
        {
            if (TempGenerationQuest == null)
            {
                var newQuest = new QuestDto
                {
                    Id = 667,
                    Title = "test Generate Quest",
                    Description = "generating bla bla bla",
                    ZoneId = 1,
                    MinLevel = 1,
                    QuestLevel = 1,
                    TimeAllowed = -1,
                    RewardExp = 0,
                    RewardMoney = 0,
                    StartDialogId = 668,
                    EndDialogId = 669,
                    IsRepetable = 0,
                    Tasks = QuestTasksGenerate(1)

                };

                QuestRepository.AddRowToDialogueCaches(newQuest);
                TempGenerationQuest = new Quest(newQuest);
            }
            return TempGenerationQuest;
        }

        public static List<QuestTaskDto> QuestTasksGenerate(int count)
        {
            var taskList = new List<QuestTaskDto>();
            for (int i = 0; i < count; i++)
            {
                var newTask = new QuestTaskDto
                {
                    Id = 510 + count,
                    Description = $"new task #{i + 1}",
                    NeededAmount = 1,
                    IsOptional = false,
                    TargetId = 45,
                    Type = QuestTaskTypes.AnswerSelect
                };
                taskList.Add(newTask);
            }
            return taskList;
        }
        public static Quest GetTempQuest()
        {
            return TempGenerationQuest ?? QuestGenerate();
        }
    }
}
