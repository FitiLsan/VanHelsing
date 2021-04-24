using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class QuestTaskAdditionalCitizenAnswer
    {
        #region Fields

        [SerializeField] private CitizenData _citizen;
        [SerializeField] private int _answerId;

        #endregion


        #region Properties

        public CitizenData Citizen => _citizen;
        public int QuestAnswerId => _answerId;

        #endregion
    }
}
