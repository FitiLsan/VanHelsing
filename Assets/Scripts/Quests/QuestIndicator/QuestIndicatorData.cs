using System;
using System.Collections.Generic;
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
        public DataTable DialogueCache = QuestRepository.GetDialogueAnswersCache();
        public DataTable QuestTasksCache = QuestRepository.GetQuestTaskCache();
        public GameContext Context;
        public Dictionary<int, GameObject> NpcList = new Dictionary<int,GameObject>();
        public Dictionary<int, GameObject> newNpcList = new Dictionary<int, GameObject>(); //test placeindicator
        #endregion


        #region Methods

        public void QuestionMarkShow(bool isOn, GameObject npc)//, QuestIndicatorModel model)
        {
            // model.QuestIndicatorTransform.GetChild(2).gameObject.SetActive(isOn);
            if (npc.tag != "Place")
            {
                npc.transform.Find("FinishQuest").GetChild(0).GetComponent<Image>().enabled = isOn;
            }
            else
            {
                npc.transform.Find("QuestIndicatorFinish").GetComponent<Image>().enabled = isOn;
            }
        }

        public void TaskQuestionMarkShow(bool isOn, GameObject npc)//, QuestIndicatorModel model)
        {
            // model.QuestIndicatorTransform.GetChild(1).gameObject.SetActive(isOn);
            if (npc.tag != "Place")
            {
                npc.transform.Find("TaskQuest").GetChild(0).GetComponent<Image>().enabled = isOn;
            }
            else
            {
                npc.transform.Find("QuestIndicatorTask").GetComponent<Image>().enabled = isOn;
            }
        }

        public void ExclamationMarkShow(bool isOn, GameObject npc)//, QuestIndicatorModel model)
        {
            //    model.QuestIndicatorTransform.GetChild(0).gameObject.SetActive(isOn);
            if (npc.tag != "Place")
            {
                npc.transform.Find("StartQuest").GetChild(0).GetComponent<Image>().enabled = isOn;
            }
            else
            {
                npc.transform.Find("QuestIndicatorStart").GetComponent<Image>().enabled = isOn;
            }
        }

        public void SetPosition(Transform npcTransform, Transform questIndicatorTransform)
        {
            questIndicatorTransform.position = new Vector3(npcTransform.position.x, npcTransform.position.y + CANVAS_OFFSET, npcTransform.position.z);
            questIndicatorTransform.parent = npcTransform;
        }

        public void QuestIndicatorCheck(EventArgs arg0)
        {
            //foreach (QuestIndicatorModel questIndicatorModel in Context.QuestIndicatorModelList)
            //{
            //    questIndicatorModel.QuestIndicatorData.GetQuestInfo(questIndicatorModel.NpcTransform.GetComponent<IGetNpcInfo>().GetInfo().Item1, questIndicatorModel);
            //}
            foreach(var npc in NpcList)
            {
                GetQuestInfo(npc.Key, npc.Value.gameObject);
                var placeName = npc.Value.transform.parent.parent.transform.Find("TitleImage/TitleText").GetComponent<Text>().text;              
            }
            foreach (var npc in newNpcList)
            {
                GetQuestInfo(npc.Key, npc.Value.gameObject);
            }
        }

        public void OnClickPlace(Place currentPlace)
        {
            NpcList.Clear();
            for(int i=0;i<currentPlace.npcList.Count;i++)
            {
                NpcList.Add(currentPlace.npcList[i].NpcId, PlaceInteractiveField.GetNpcList()[i]);
            }
            QuestIndicatorCheck(null);
        }

        public void OnPlaceChecker(List<Place> placeList)
        {
            foreach (var place in placeList)
            {
                for (int i = 0; i < place.npcList.Count; i++)
                {
                    newNpcList.Add(place.npcList[i].NpcId, place.gameObject);
                }
            }
            QuestIndicatorCheck(null);
            //foreach (var npc in newNpcList)
            //{
            //    GetQuestInfo(npc.Key, npc.Value.gameObject);
            //}
        }

        public void GetQuestInfo(int npcID, GameObject npc)//QuestIndicatorModel model)
        {

            var questModel = Context.QuestModel; //model.Context.QuestModel;
            var questsWithCompletedAllTask = questModel.AllTaskCompletedInQuests;
            var questsWithCompletedAllTaskWithOptional = questModel.AllTaskCompletedInQuestsWithOptional;
            var activeQuestsById = questModel.ActiveQuests;
            var completedQuests = questModel.CompletedQuests;
            var completedTasks = questModel.CompletedTasksById;
            var activeQuests = questModel.QuestsList;
            var hasRequiredQuest = false;
            var hasForbiddenQuest = false;

            if (DialogueCache.Rows.Count != 0)
            {
                for (int i = 0; i < DialogueCache.Rows.Count; i++)
                {
                    if (DialogueCache.Rows[i].GetInt(8) != 0) 
                    {
                        var currentQuestID = DialogueCache.Rows[i].GetInt(8);
                        var dialogueTargetID = DialogueCache.Rows[i].GetInt(0);
                        // RequiredCheck
                        if (currentQuestID != 0)
                        {
                            var tempQuest = QuestRepository.GetById(currentQuestID);
                             hasRequiredQuest = false;
                             hasForbiddenQuest = false;

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
                        }
                        //
                        if (DialogueCache.Rows[i].GetInt(5) == npcID)
                        {
                            if (DialogueCache.Rows[i].GetInt(6) == 1)
                            {
                                if (!completedQuests.Contains(currentQuestID) & !activeQuestsById.Contains(currentQuestID) & !hasRequiredQuest & !hasForbiddenQuest)
                                {
                                    ExclamationMarkShow(true, npc);
                                }
                                else
                                {
                                    ExclamationMarkShow(false, npc);
                                }
                            }

                            if (DialogueCache.Rows[i].GetInt(9) == 1)
                            {
                                for (int j = 0; j < QuestTasksCache.Rows.Count; j++)
                                {
                                    var q = QuestTasksCache.Rows[j].GetInt(1);
                                    if (QuestTasksCache.Rows[j].GetInt(1) == currentQuestID & QuestTasksCache.Rows[j].GetInt(3) == npcID)
                                    {
                                        var currentTaskID = QuestTasksCache.Rows[j].GetInt(0);
                                        var taskTargetID = QuestTasksCache.Rows[j].GetInt(2);

                                        if (!completedTasks.Contains(currentTaskID) &
                                            activeQuestsById.Contains(currentQuestID) &
                                            !questsWithCompletedAllTask.Contains(currentQuestID) &
                                            taskTargetID == dialogueTargetID &
                                            !hasRequiredQuest &
                                            !hasForbiddenQuest)
                                        {
                                            TaskQuestionMarkShow(true, npc);
                                            break;
                                        }
                                        else
                                        {
                                            var flag = false;
                                            for (int k = 0; k < activeQuests.Count; k++)
                                            {
                                                var tempQuestId = activeQuests[k].Id;
                                                foreach (var task in activeQuests[k].Tasks)
                                                {

                                                    if (activeQuestsById.Contains(tempQuestId) &
                                                        !questsWithCompletedAllTask.Contains(tempQuestId) &
                                                        !questsWithCompletedAllTaskWithOptional.Contains(tempQuestId) &
                                                        tempQuestId != currentQuestID &
                                                        !completedTasks.Contains(task.Id) & 
                                                        dialogueTargetID == task.TargetId &
                                                        !completedQuests.Contains(currentQuestID) &
                                                        !hasRequiredQuest &
                                                        !hasForbiddenQuest)
                                                    {
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                                if(flag)
                                                { break; }
                                            }
                                        

                                            if (!flag)
                                            {
                                                TaskQuestionMarkShow(false, npc);
                                            }
                                        }
                                    }
                                }
                            }

                            if (DialogueCache.Rows[i].GetInt(7) == 1)
                            {
                                if (questsWithCompletedAllTaskWithOptional.Contains(currentQuestID) || questsWithCompletedAllTask.Contains(currentQuestID))
                                {
                                    QuestionMarkShow(true, npc);
                                }
                                else
                                {
                                    QuestionMarkShow(false, npc);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
