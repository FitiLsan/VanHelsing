using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class QuestTaskAdditionalCitizenAnswer
    {
        #region Fields

        [SerializeField] private CitizenSO _citizen;
        [SerializeField] private int _answerId;

        #endregion


        #region Properties

        public CitizenSO Citizen => _citizen;
        public int QuestAnswerId => _answerId;

        #endregion
    }
}
