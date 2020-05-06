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

                    if (context.QuestModel.CompletedQuests.Contains(answerQuestId))
                    {
                        continue;
                    }

                    if (context.QuestModel._quests.Count != 0)
                    {
                        var flag =false;
                        foreach (Quest quest in context.QuestModel._quests)
                        {
                            if (context.QuestModel.AllTaskCompletedInQuests.Count != 0)
                            {
                                if (!context.QuestModel.AllTaskCompletedInQuests.Contains(answerQuestId))
                                {
                                    break;
                                }
                            }
                            else if (answerIsEndQuest == 1)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if(flag)
                        {
                            continue;
                        }
                    }
                    else if(answerIsEndQuest == 1 || answerTaskQuest == 1)
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