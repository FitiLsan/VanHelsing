using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class SelfHealSkill : BossBaseSkill
    {
        public SelfHealSkill(AttackStateSkillsSettings attackStateSkillsSettings, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(attackStateSkillsSettings, skillDictionary, stateMachine)
        {
        }

        public SelfHealSkill((int, float, float, float, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public SelfHealSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log(" AttackSkill");
            _bossModel.BossAnimator.Play($"SelfHeal", 0, 0f);
        }
    }
}