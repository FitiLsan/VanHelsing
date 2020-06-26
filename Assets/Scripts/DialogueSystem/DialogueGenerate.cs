using System.Collections.Generic;
using System.Data;
using DialogueSystem;


namespace BeastHunter
{
    public sealed class DialogueGenerate
    {
        #region Methods

        public static List<Dialogue> DialogueCreate(int npcID, GameContext context)
        {
            if (npcID.Equals(666))
            {
                QuestGeneration.QuestGenerate();
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
            }

            List<Dialogue> dialogueNode = new List<Dialogue>();
            
            var npcDt = QuestRepository.GetDialogueNodesCache();
            DataTable tempTable = npcDt.Copy();
            for (var k = tempTable.Rows.Count-1; k >= 0; k--)
            {
                if (tempTable.Rows[k].GetInt(1) != npcID)
                {
                    tempTable.Rows.RemoveAt(k);
                }
            }
            
            for (var j = 0; j < tempTable.Rows.Count; j++)
            {
                var npcText = tempTable.Rows[j].GetString(3);

                dialogueNode.Add(new Dialogue(npcText, new List<PlayerAnswer>()));

                var answerDt = QuestRepository.GetDialogueAnswersCache();
                for (var i = 0; i < answerDt.Rows.Count; i++)
                {
                    if (answerDt.Rows[i].GetInt(5) == npcID & answerDt.Rows[i].GetInt(1) == j)
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
                        if (answerQuestId != 0)
                        {
                            var tempQuest = QuestRepository.GetById(answerQuestId);
                            var hasRequiredQuest = false;
                            var hasForbiddenQuest = false;

                            foreach (var questId in tempQuest.RequiredQuests)
                            {
                                if (!completedQuests.Contains(questId))
                                {
                                    hasRequiredQuest = true;
                                }
                            }
                            foreach (var questId in tempQuest.ForbiddenQuests)
                            {
                                if (completedQuests.Contains(questId))
                                {
                                    hasForbiddenQuest = true;
                                }
                            }
                            if (hasRequiredQuest || hasForbiddenQuest)
                            {
                                continue;
                            }
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
                                        if (!allTaskCompletedWithOptinal.Contains(answerQuestId))
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
                            if (quest.Id == answerQuestId)
                            {
                                foreach (var task in quest.Tasks)
                                {
                                    if (task.TargetId == answerId & task.IsCompleted)
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
            }
                return dialogueNode;            
        }

        #endregion
    }
}
