using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class RageOfForestAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        public RageOfForestAttackSkill(bool IsEnable, int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(IsEnable, Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public RageOfForestAttackSkill((bool, int, float, float, float, bool, bool, float) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
            IsNeedRage = true;
        }

        public override void UseSkill(int id)
        {
            IsSkillUsing = true;

            Debug.Log("RAGEAttackSkill");
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);
            _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, (Vector3)_stateMachine._mainState.GetTargetCurrentPosition(), 5f);
            _bossModel.BossAnimator.Play("BossRageAttack", 0, 0f);

            TurnOnHitBoxTrigger(_bossModel.RightHandBehavior, _stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER, SkillDamage);
            TurnOnHitBoxCollider(_bossModel.RightHandCollider, _stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);
            TurnOnHitBoxTrigger(_bossModel.LeftHandBehavior, _stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER, SkillDamage);
            TurnOnHitBoxCollider(_bossModel.LeftHandCollider, _stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);

            ReloadSkill(id);
        }

        public override void StopSkill()
        {
            IsSkillUsing = false;
        }

    }
}