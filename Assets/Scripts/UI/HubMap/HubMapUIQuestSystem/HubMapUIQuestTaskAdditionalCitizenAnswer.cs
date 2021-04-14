using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class HubMapUIQuestTaskAdditionalCitizenAnswer
    {
        #region Fields

        [SerializeField] private HubMapUICitizenData _citizen;
        [SerializeField] private int _answerId;

        #endregion


        #region Properties

        public HubMapUICitizenData Citizen => _citizen;
        public int QuestAnswerId => _answerId;

        #endregion
    }
}
