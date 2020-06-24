using System.Collections.Generic;
using DialogueSystem;


namespace BeastHunter
{
    public sealed class DialogueGenerate
    {
        #region Methods

        public static List<Dialogue> DialogueCreate(int npcID, GameContext context)
        {
            List<Dialogue> dialogueNode = new List<Dialogue>();

            var npcDt = DatabaseWrapper.GetTable($"select * from 'dialogue_node' where Npc_id={npcID};");

            for (var j = 0; j < npcDt.Rows.Count; j++)
            {
                var npcText = npcDt.Rows[j].GetString(3);

                dialogueNode.Add(new Dialogue(npcText, new List<PlayerAnswer>()));

                var answerDt = DatabaseWrapper.GetTable($"select * from 'dialogue_answers' where Node_id={j} and Npc_id={npcID};");

                for (var i = 0; i < answerDt.Rows.Count; i++)
                {
                    var answerId = answerDt.Rows[i].GetInt(0);
                    var answerText = answerDt.Rows[i].GetString(2);
                    var answerToNode = answerDt.Rows[i].GetInt(3);
                    var answerEndDialogue = answerDt.Rows[i].GetInt(4);
                    var answerIsStartQuest = answerDt.Rows[i].GetInt(6);
                    var answerIsEndQuest = answerDt.Rows[i].GetInt(7);
                    var answerQuestId = answerDt.Rows[i].GetInt(8);
                    var answerTaskQuest = answerDt.Rows[i].GetInt(9);

                    var completedQuests = context.QuestModel.CompletedQuests;
                    var activeQuests = context.QuestModel.ActiveQuests;
                    var allTaskCompleted = context.QuestModel.AllTaskCompletedInQuests;
                    var allTaskCompletedWithOptinal = context.QuestModel.AllTaskCompletedInQuestsWithOptional;
                    if (completedQuests.Contains(answerQuestId))
                    {
                        continue;
                    }

                    if (activeQuests.Count != 0)
                    {
                        if (activeQuests.Contains(answerQuestId))
                        {
                            if (answerIsStartQuest == 1)
                            {
                                continue;
                            }
                            if (answerTaskQuest == 1)
                            {
                                if (allTaskCompleted.Count != 0)
                                {
                                    if (allTaskCompleted.Contains(answerQuestId))
                                    {
                                        continue;
                                    }
                                }
                            }
                            if (answerIsEndQuest == 1) 
                            {
                                if (!allTaskCompleted.Contains(answerQuestId))  
                                {
                                    if (!allTaskCompletedWithOptinal.Contains(answerQuestId))//
                                    continue;
                                }
                            }
                        }
                        else if (answerIsEndQuest == 1 || answerTaskQuest == 1)
                        {
                            continue;
                        }
                    }
                    else if (answerIsEndQuest == 1 || answerTaskQuest == 1)
                    {
                        continue;
                    }

                    var flag = false;
                    foreach (var quest in context.QuestModel.Quests)
                    {                       
                        if(quest.Id == answerQuestId)
                        {
                            foreach (var task in quest.Tasks)
                            {
                                if(task.TargetId==answerId & task.IsCompleted)
                                {
                                    flag = true;
                                    continue;
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        continue;
                    }
                    dialogueNode[j].PlayerAnswers.Add(new PlayerAnswer(answerId, answerText, answerToNode, answerEndDialogue, answerIsStartQuest, answerIsEndQuest, answerQuestId, answerTaskQuest));

                }
            }
            return dialogueNode;
        }

        #endregion
    }
}