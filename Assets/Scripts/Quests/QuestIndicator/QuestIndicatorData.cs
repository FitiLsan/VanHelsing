using System;
using System.Data;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/QuestIndicator", order = 0)]
    public sealed class QuestIndicatorData : ScriptableObject
    {
        #region Fields

        public const float CANVAS_OFFSET = 2.4f;

        public QuestIndicatorStruct QuestIndicatorStruct;
        public Vector3 NpcPos;
        public int NpcID;
        public DataTable DialogueCache = QuestRepository.GetDialogueCache();
        public DataTable QuestTasksCache = QuestRepository.GetQuestTaskCache();
        public GameContext Context;
        #endregion


        #region Methods

        public void QuestionMarkShow(bool isOn, QuestIndicatorModel model)
        {
            model.QuestIndicatorTransform.GetChild(2).gameObject.SetActive(isOn);
        }

        public void TaskQuestionMarkShow(bool isOn, QuestIndicatorModel model)
        {
            model.QuestIndicatorTransform.GetChild(1).gameObject.SetActive(isOn);
        }

        public void ExclamationMarkShow(bool isOn, QuestIndicatorModel model)
        {
            model.QuestIndicatorTransform.GetChild(0).gameObject.SetActive(isOn);
        }

        public void SetPosition(Transform npcTransform, Transform questIndicatorTransform)
        {
            questIndicatorTransform.position = new Vector3(npcTransform.position.x, npcTransform.position.y + CANVAS_OFFSET, npcTransform.position.z);
            questIndicatorTransform.parent = npcTransform;
        }

        public void QuestIndicatorCheck(EventArgs arg0)
        {
            foreach (QuestIndicatorModel questIndicatorModel in Context.QuestIndicatorModelList)
            {
                questIndicatorModel.QuestIndicatorData.GetQuestInfo(questIndicatorModel.NpcTransform.GetComponent<IGetNpcInfo>().GetInfo().Item1, questIndicatorModel);
            }
        }

        public void GetQuestInfo(int npcID, QuestIndicatorModel model)
        {

            var questModel = model.Context.QuestModel;
            var questsWithCompletedAllTask = questModel.AllTaskCompletedInQuests;
            var questsWithCompletedAllTaskWithOptional = questModel.AllTaskCompletedInQuestsWithOptional;
            var activeQuests = questModel.ActiveQuests;
            var completedQuests = questModel.CompletedQuests;
            var completedTasks = questModel.CompletedTasks;

            if (DialogueCache.Rows.Count != 0)
            {
                for (int i = 0; i < DialogueCache.Rows.Count; i++)
                {
                    var currentQuestID = DialogueCache.Rows[i].GetInt(8);
                    var dialogueTargetID = DialogueCache.Rows[i].GetInt(0);

                    if (DialogueCache.Rows[i].GetInt(5) == npcID)
                    {
                        if (DialogueCache.Rows[i].GetInt(6) == 1)
                        {
                            if (!completedQuests.Contains(currentQuestID) & !activeQuests.Contains(currentQuestID))
                            {
                                ExclamationMarkShow(true, model);
                            }
                            else
                            {
                                ExclamationMarkShow(false, model);
                            }
                        }

                        if (DialogueCache.Rows[i].GetInt(9) == 1)
                        {
                            for (int j = 0; j < QuestTasksCache.Rows.Count; j++)
                            {
                                var q = QuestTasksCache.Rows[j].GetInt(1);
                                if (QuestTasksCache.Rows[j].GetInt(1) == currentQuestID)
                                {  
                                    var currentTaskID = QuestTasksCache.Rows[j].GetInt(0);
                                    var taskTargetID = QuestTasksCache.Rows[j].GetInt(2);
                                    
                                    if (!completedTasks.Contains(currentTaskID) &
                                        activeQuests.Contains(currentQuestID) &
                                        !questsWithCompletedAllTask.Contains(currentQuestID) &
                                        taskTargetID == dialogueTargetID)
                                    {
                                        TaskQuestionMarkShow(true, model);
                                        break;
                                    }
                                    else
                                    {
                                        var flag = false;
                                        for (int k = 0; k < activeQuests.Count; k++)
                                        {
                                            var tempQuestId = activeQuests[k];

                                            if (activeQuests.Contains(tempQuestId) &
                                                !questsWithCompletedAllTask.Contains(tempQuestId) &
                                                !questsWithCompletedAllTaskWithOptional.Contains(tempQuestId) &
                                                tempQuestId != currentQuestID &
                                                !completedTasks.Contains(currentTaskID) &
                                                !completedQuests.Contains(currentQuestID)) 
                                                {
                                                    flag = true;
                                                   break;
                                                }
                                        }
                                        
                                        if (!flag)
                                        {
                                            TaskQuestionMarkShow(false, model);
                                        }
                                    }
                                }
                            }
                        }

                        if (DialogueCache.Rows[i].GetInt(7) == 1)
                        {
                            if (questsWithCompletedAllTaskWithOptional.Contains(currentQuestID) || questsWithCompletedAllTask.Contains(currentQuestID))
                            {
                                QuestionMarkShow(true, model);
                            }
                            else
                            {
                                QuestionMarkShow(false, model);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
