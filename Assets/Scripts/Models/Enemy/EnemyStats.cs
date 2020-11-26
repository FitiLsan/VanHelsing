using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public class EnemyStats
    {
        #region Fields

        public BaseStatsClass MainStats;
        public List<BaseAttack> AttackList;
        public DefenceStatsClass DefenceStats;

        //SkillList (passive & non-attack skills)
        //BehaviorStats (BattleBehavior, NonbattleBehavior)

        public GameObject Prefab;

        #endregion
    }
}
