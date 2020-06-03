using System.Collections.Generic;

namespace BeastHunter
{
    public  class QuestGeneration
    {
        public static Quest TempGenerationQuest;
        public static (int,int) lastGeneratedId = ProgressDatabaseWrapper.GetLastGeneratedID();
        public static Quest QuestGenerate()
        {
            if (TempGenerationQuest == null)
            {
                var newQuest = new QuestDto
                {
                    Id = lastGeneratedId.Item1 + 1, //
                    Title = $"test Generate Quest {lastGeneratedId.Item1+1}",
                    Description = "generating bla bla bla",
                    ZoneId = 1,
                    MinLevel = 1,
                    QuestLevel = 1,
                    TimeAllowed = -1,
                    RewardExp = 0,
                    RewardMoney = 0,
                    StartDialogId = 668, //?
                    EndDialogId = 669, //?
                    IsRepetable = 0,
                    Tasks = QuestTasksGenerate(3)

                };
                if (QuestRepository.GetById(newQuest.Id) == null)
                {
                    QuestRepository.AddRowToDialogueCaches(newQuest);
                    TempGenerationQuest = new Quest(newQuest);
                    Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.SaveGeneratedQuest, new QuestArgs(TempGenerationQuest));
                    
                }               
            }
            return TempGenerationQuest;
        }

        public static List<QuestTaskDto> QuestTasksGenerate(int count)
        {
            var taskList = new List<QuestTaskDto>();
            for (int i = 1; i <= count; i++)
            {
                var newTask = new QuestTaskDto
                {
                    Id = lastGeneratedId.Item2+i, //need use last generate ID +1
                    Description = $"new task #{lastGeneratedId.Item2+i}",
                    NeededAmount = 1,
                    IsOptional = false,
                    TargetId = 888+i, // need unic
                    Type = QuestTaskTypes.AnswerSelect
                };
                taskList.Add(newTask);
            }
            return taskList;
        }
        public static Quest GetTempQuest()
        {
            return TempGenerationQuest; //?? QuestGenerate();
        }
    }
}
