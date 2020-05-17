using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public sealed class QuestJournalModel
    {
        #region Fields

        public GameContext Context;

        #endregion


        #region Properties

        public Transform QuestJournalTransform { get; }
        public QuestJournalData QuestJournalData { get; }
        public QuestJournalStruct QuestJournalStruct { get; }
        public Image[] ChildComponents { get; }
        public Image QuestContentField { get; }
        public Image DescriptionContentField { get; }
        public Image TasksFieldField { get; }
        public Image RewardContentField { get; }

        public GameObject QuestButton { get; }
        public GameObject Task { get; }
        public GameObject Reward { get; }

        #endregion


        #region ClassLifeCycle

        public QuestJournalModel(GameObject prefab, QuestJournalData questJournalData, GameContext context)
        {
            QuestJournalData = questJournalData;
            QuestJournalStruct = questJournalData.QuestJournalStruct;
            QuestJournalTransform = prefab.transform;
            QuestJournalData.Model = this;
            Context = context;
            
            var str = "QuestJournalBackground/RightPagePanel/ScrollView";
            QuestContentField = QuestJournalTransform.Find("QuestJournalBackground/LeftPagePanel/QuestsScrollView/QuestsContent").GetComponent<Image>();
            DescriptionContentField = QuestJournalTransform.Find(str+"Description/DescriptionContent").GetComponent<Image>();
            TasksFieldField = QuestJournalTransform.Find(str + "Objectives/TasksContent").GetComponent<Image>();
            RewardContentField = QuestJournalTransform.Find(str + "Reward/RewardContent").GetComponent<Image>();

            QuestButton = QuestJournalStruct.QuestPrefab;
            Task = QuestJournalStruct.QuestTaskPrefab;
            Reward = QuestJournalStruct.RewardPrefab;

            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestAccepted, QuestJournalData.LoadQuestInfo);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestButtonClickEvent, QuestJournalData.ShowQuestInfo);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestJournalCreated, QuestJournalData.InitializeQuestJournal);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestJournalOpened, QuestJournalData.ShowQuestInfo);
        }

        #endregion


        #region Metods

        public void Execute()
        {
        }

        #endregion
    }
}