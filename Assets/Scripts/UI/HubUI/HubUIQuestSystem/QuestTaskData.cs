using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class QuestTaskData
    {
        #region Fields

        [SerializeField, ReadOnlyInUnityInspector] private int _id;
        [SerializeField] private int _nextQuestTaskId;
        [Space]
        [SerializeField] private CitizenData _targetCitizen;
        [SerializeField] private int _targetQuestAnswerId;
        [SerializeField] private bool _isCitizenInitiateDialog;
        [SerializeField] private int _initiatedDialogId;
        [SerializeField] private BaseItemData _givenItemData;
        [SerializeField] private BaseItemData _takenItemData;
        [Space]
        [SerializeField] private QuestTaskAdditionalCitizenAnswer[] _additionalCitizensAnswers;

        #endregion


        #region Properties

        public int Id => _id;
        public int NextQuestTaskId => _nextQuestTaskId;

        public CitizenData TargetCitizen => _targetCitizen;
        public int TargetQuestAnswerId => _targetQuestAnswerId;
        public bool IsCitizenInitiateDialog => _isCitizenInitiateDialog;
        public int InitiatedDialogId => _initiatedDialogId;
        public BaseItemData GivenItemData => _givenItemData;
        public BaseItemData TakenItemData => _takenItemData;

        public QuestTaskAdditionalCitizenAnswer[] AdditionalCitizensAnswers => (QuestTaskAdditionalCitizenAnswer[])_additionalCitizensAnswers.Clone();

        #endregion


        #region Methods

        public void SetId(int id)
        {
            _id = id;
        }

        #endregion
    }
}
