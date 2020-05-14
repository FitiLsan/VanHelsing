using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/QuestJournalData", order = 0)]
    public sealed class QuestJournalData : ScriptableObject
    {
        #region Fields
        private List<GameObject> _taskList = new List<GameObject>();
        private List<GameObject> _rewardList = new List<GameObject>();
        private string _description;

        public QuestJournalStruct QuestJournalStruct;
        public QuestJournalModel Model;
        public QuestModel questModel;

        #endregion


        #region Methods

        public void LoadQuestInfo(EventArgs args)
        {
            questModel = Model.Context.QuestModel;
            var quest = questModel.QuestStorage.GetQuestById((args as IdArgs).Id);
            var questName = quest.Title;            
            AddQuestButton(Model.QuestContentField.transform, Model.QuestButton, questName, quest.Id );          
        }

        public void ShowQuestInfo(EventArgs args)
        {
            ClearQuestInfo();
            var quest = questModel.QuestStorage.GetQuestById((args as IdArgs).Id);

            var questDescription = quest.Description;
            var questTasks = quest.Tasks;
            var questReward = quest.Rewards;

            AddDescription(Model.DescriptionContentField, questDescription);

            foreach (QuestTask task in questTasks)
            {
                AddQuestTasks(Model.TasksFieldField.transform, Model.Task, task.Description, task.IsCompleted, task.CurrentAmount, task.NeededAmount, task.IsOptional);
            }

            foreach (QuestRewardGroup reward in questReward)
            {
                AddQuestRewards(Model.RewardContentField.transform, Model.Reward);     //TODO: Rewards
            }

            AddQuestRewards(Model.RewardContentField.transform, Model.Reward);
        }

        public void AddQuestButton(Transform questContent, GameObject questButton, string questName, int questId)
        {
            var tempQuestButton = Instantiate(questButton, questContent.position, questContent.rotation, questContent);
            tempQuestButton.GetComponentInChildren<Text>().text = questName;
            tempQuestButton.name +=$"/{questId}";
        }

        public void AddDescription(Image description, string text)
        {
            questModel = Model.Context.QuestModel;
            _description = description.GetComponentInChildren<Text>().text = text;
        }

        public void AddQuestTasks(Transform tasksContent, GameObject task, string taskText, bool isComplete, int curAmount, int needAmount, bool isOptional)
        {
            var tempTask = Instantiate(task, tasksContent.position, tasksContent.rotation, tasksContent);
            _taskList.Add(tempTask);

            var _taskText = tempTask.transform.Find("Text").GetComponent<Text>();
            var _curAmount = tempTask.transform.Find("curAmount").GetComponent<Text>();
            var _needAmount = tempTask.transform.Find("needAmount").GetComponent<Text>();
            var _isComplete = tempTask.transform.Find("isCompleted").GetComponent<Image>();
            var _isOptional = "*";

            if (isOptional)
            {
                taskText += _isOptional;
            }
            _taskText.text = taskText;
            _curAmount.text = curAmount.ToString();
            _needAmount.text = needAmount.ToString();
            _isComplete.enabled = isComplete;
        }

        public void AddQuestRewards(Transform rewardField, GameObject reward)
        {
            var tempReward = Instantiate(reward, rewardField.position, rewardField.rotation, rewardField);
            _rewardList.Add(tempReward);
        }

        public void ClearQuestInfo()
        {
            foreach (GameObject obj in _taskList)
            {
                Destroy(obj);
            }

            foreach (GameObject obj in _rewardList)
            {
                Destroy(obj);
            }
            _taskList.Clear();
            _rewardList.Clear();
            _description = "";
        }


        #endregion
    }
}