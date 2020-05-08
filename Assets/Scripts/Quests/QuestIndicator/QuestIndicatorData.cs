using System;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

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
        public DataTable cache = QuestRepository.GetDialogueCache();

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

        public void QuestIndicatorCheck(GameContext context)
        {
            foreach (QuestIndicatorModel questIndicatorModel in context.QuestIndicatorModelList)
            {
                questIndicatorModel.QuestIndicatorTransform.LookAt(context.CharacterModel.CharacterCamera.transform);

                questIndicatorModel.QuestIndicatorData.GetQuestInfo(questIndicatorModel.NpcTransform.GetComponent<IGetNpcInfo>().GetInfo().Item1, questIndicatorModel);
            }
        }

        public void GetQuestInfo(int npcID, QuestIndicatorModel model)
        {

            var questModel = model.Context.QuestModel;
            var completedTasksInQuest = questModel.AllTaskCompletedInQuests;
            var activeQuests = questModel.ActiveQuests;
            var completedQuests = questModel.CompletedQuests;

            if (cache.Rows.Count != 0)
            {
                for (int i = 0; i < cache.Rows.Count; i++)
                {
                    var currentQuestID = cache.Rows[i].GetInt(8);

                    if (cache.Rows[i].GetInt(6) == 1 & cache.Rows[i].GetInt(5) == npcID)
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

                    if (cache.Rows[i].GetInt(9) == 1 & cache.Rows[i].GetInt(5) == npcID)
                    {
                        if (activeQuests.Contains(currentQuestID) & !completedTasksInQuest.Contains(currentQuestID))
                        {
                            TaskQuestionMarkShow(true, model);
                        }
                        else
                        {
                            TaskQuestionMarkShow(false, model);
                        }
                    }

                    if (cache.Rows[i].GetInt(7) == 1 & cache.Rows[i].GetInt(5) == npcID)
                    {
                        if (completedTasksInQuest.Contains(currentQuestID))
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
        #endregion
    }
}
