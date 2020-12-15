using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class DefaultAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        public DefaultAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }
        public DefaultAttackSkill((int, float, float, float, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("DefaultAttackSkill");
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);
            var attackDirection = UnityEngine.Random.Range(0, 2);
            _bossModel.BossAnimator.Play($"BossFeastsAttack_{attackDirection}", 0, 0f);
            switch (attackDirection)
            {
                case 0:
                    _currenTriggertHand = _bossModel.RightHandBehavior;
                    break;
                case 1:
                    _currenTriggertHand = _bossModel.LeftHandBehavior;
                    break;
                default:
                    break;
            }
            TurnOnHitBoxTrigger(_currenTriggertHand,_stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);

            _skillDictionary[id].IsSkillReady = false;

            StartCooldown(id, _skillDictionary[id].SkillCooldown);

            _stateMachine.CurrentState.isAnimationPlay = true;
        }
    }
}