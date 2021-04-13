using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class HorizontalAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.1f;

        public HorizontalAttackSkill(bool IsEnable, int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(IsEnable, Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public HorizontalAttackSkill((bool, int, float, float, float, bool, bool, float) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }
        public override void UseSkill(int id)
        {
            Debug.Log("HorizontalAttackSkill");
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);
            var attackDirection = UnityEngine.Random.Range(2, 4);
            _bossModel.BossAnimator.Play($"BossFeastsAttack_{attackDirection}", 0, 0f);
            switch (attackDirection)
            {
                case 2:
                    _currenTriggertHand = _bossModel.RightHandBehavior;
                    _currenColliderHand = _bossModel.RightHandCollider;
                    break;
                case 3:
                    _currenTriggertHand = _bossModel.LeftHandBehavior;
                    _currenColliderHand = _bossModel.LeftHandCollider;
                    break;
                default:
                    break;
            }
            TurnOnHitBoxTrigger(_currenTriggertHand,_stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER, SkillDamage);
            TurnOnHitBoxCollider(_currenColliderHand, _stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);

            ReloadSkill(id);
        }

        public override void StopSkill()
        {
        }
    }
}