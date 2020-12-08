using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class HorizontalAttackSkill : BaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        public HorizontalAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("HorizontalAttackSkill");
            var attackDirection = UnityEngine.Random.Range(0, 2);
            _bossModel.BossAnimator.Play($"BossFeastsAttack_{attackDirection}", 0, 0f);
            switch (attackDirection)
            {
                case 0:
                    _currenTriggertHand = _bossModel.RightHandBehavior;
                    _currenColliderHand = _bossModel.RightHandCollider;
                    break;
                case 1:
                    _currenTriggertHand = _bossModel.LeftHandBehavior;
                    _currenColliderHand = _bossModel.LeftHandCollider;
                    break;
                default:
                    break;
            }
            TurnOnHitBoxTrigger(_currenTriggertHand,_stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);
            TurnOnHitBoxCollider(_currenColliderHand, _stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);
            _skillDictionary[id].IsAttackReady = false;

            SkillCooldown(id, _skillDictionary[id].AttackCooldown);
            _stateMachine.CurrentState.isAnimationPlay = true;
        }
    }
}