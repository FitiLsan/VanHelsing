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
                    if (!context._questModel.CompletedQuests.Contains(answerQuestId))
                    {
                        dialogueNode[j].PlayerAnswers.Add(new PlayerAnswer(answerId, answerText, answerToNode, answerEndDialogue, answerIsStartQuest, answerIsEndQuest, answerQuestId, answerTaskQuest));
                    }
                }
            }
            return dialogueNode;
        }

        #endregion
    }
}