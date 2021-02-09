using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class BushTriggerAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        //public BushTriggerAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        //    : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        //{
        //}
        public BushTriggerAttackSkill((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("BushTriggerAttackSkill");
            

            ReloadSkill(id);
        }

        public override void StopSkill()
        {
        }
    }
}