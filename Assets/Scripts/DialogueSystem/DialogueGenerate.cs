using Extensions;
using System.Collections.Generic;
using DialogueSystem;


namespace BeastHunter
{
    public sealed class DialogueGenerate
    {

        #region Methods
        
        public static List<Dialogue> DialogueCreate(int _npcID)
        {
            List<Dialogue> dialogueNode = new List<Dialogue>();

            var npc_dt = DatabaseWrapper.DatabaseWrapper.GetTable($"select * from 'dialogue_node' where Npc_id={_npcID};");

            for (var j = 0; j < npc_dt.Rows.Count; j++)
            {
                var npc_text = npc_dt.Rows[j].GetString(3);

                dialogueNode.Add(new Dialogue(npc_text, new List<PlayerAnswer>()));

                var answer_dt = DatabaseWrapper.DatabaseWrapper.GetTable($"select * from 'dialogue_answers' where Node_id={j} and Npc_id={_npcID};");

                for (var i = 0; i < answer_dt.Rows.Count; i++)
                {
                    var answer_id = answer_dt.Rows[i].GetInt(0);
                    var answer_text = answer_dt.Rows[i].GetString(2);
                    var answer_toNode = answer_dt.Rows[i].GetInt(3);
                    var answer_endDialogue = answer_dt.Rows[i].GetInt(4);
                    var answer_isStartQuest = answer_dt.Rows[i].GetInt(6);
                    var answer_isEndQuest = answer_dt.Rows[i].GetInt(7);
                    var answer_questId = answer_dt.Rows[i].GetInt(8);
                    var answer_taskQuest = answer_dt.Rows[i].GetInt(9);
                    dialogueNode[j].PlayerAnswers.Add(new PlayerAnswer(answer_id, answer_text, answer_toNode, answer_endDialogue, answer_isStartQuest, answer_isEndQuest, answer_questId, answer_taskQuest));
                }
            }
            return dialogueNode;
        }

        #endregion

    }
}