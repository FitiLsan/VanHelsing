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
                    Id = 667, //need use last generate ID +1
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
                    IsRepetable = 1,
                    Tasks = QuestTasksGenerate(3)

                };
                if (QuestRepository.GetById(newQuest.Id) == null)
                {
                    QuestRepository.AddRowToDialogueCaches(newQuest);
                    Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.SaveGeneratedQuest, new QuestArgs(newQuest));
                    TempGenerationQuest = new Quest(newQuest);
                }
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
                    Id = 510 + i, //need use last generate ID +1
                    Description = $"new task #{i + 1}",
                    NeededAmount = 1,
                    IsOptional = false,
                    TargetId = i, // need unic
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
