using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct QuestJournalStruct
    {
        #region Fields

        public GameObject Prefab;
        public GameObject QuestPrefab;
        public GameObject QuestTaskPrefab;
        public GameObject RewardPrefab;

        #endregion
    }
}