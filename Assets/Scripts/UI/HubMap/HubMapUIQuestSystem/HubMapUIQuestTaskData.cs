using UnityEngine;

namespace BeastHunter
{
    [System.Serializable]
    public class HubMapUIQuestTaskData
    {
        #region Fields

        [SerializeField, ReadOnlyInUnityInspector] private int _id;
        [SerializeField] private int _nextQuestTaskId;
        [Space]
        [SerializeField] private HubMapUICitizenData _targetCitizen;
        [SerializeField] private int _targetQuestAnswerId;
        [SerializeField] private bool _isCitizenInitiateDialog;
        [SerializeField] private int _initiatedDialogId;
        [SerializeField] private HubMapUIBaseItemData _givenItemData;
        [SerializeField] private HubMapUIBaseItemData _takenItemData;
        [Space]
        [SerializeField] private HubMapUIQuestTaskAdditionalCitizenAnswer[] _additionalCitizensAnswers;

        #endregion


        #region Properties

        public int Id => _id;
        public int NextQuestTaskId => _nextQuestTaskId;

        public HubMapUICitizenData TargetCitizen => _targetCitizen;
        public int TargetQuestAnswerId => _targetQuestAnswerId;
        public bool IsCitizenInitiateDialog => _isCitizenInitiateDialog;
        public int InitiatedDialogId => _initiatedDialogId;
        public HubMapUIBaseItemData GivenItemData => _givenItemData;
        public HubMapUIBaseItemData TakenItemData => _takenItemData;

        public HubMapUIQuestTaskAdditionalCitizenAnswer[] AdditionalCitizensAnswers => (HubMapUIQuestTaskAdditionalCitizenAnswer[])_additionalCitizensAnswers.Clone();

        #endregion


        #region Methods

        public void SetId(int id)
        {
            _id = id;
        }

        #endregion
    }
}
