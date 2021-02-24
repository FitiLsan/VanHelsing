using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class ThrowAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;
        public static event Action HandDrop;

        //public ThrowAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        //    : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        //{
        //}
        public ThrowAttackSkill((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            if(!_bossModel.IsPickUped)
            {
                return;
            }

            Debug.Log("ThrowAttackSkill");
            var side = _bossModel.CurrentHand;
            switch (side)
            {
                case FullBodyBipedEffector.RightHand:
                    _bossModel.BossAnimator.Play($"ThrowAttack_R", 0, 0f);
                    break;
                case FullBodyBipedEffector.LeftHand:
                    _bossModel.BossAnimator.Play($"ThrowAttack_L", 0, 0f);
                    break;
                default:
                    break;
            }
            DelayCall(() => Drop(), 0.65f);
            ReloadSkill(id);
        }

        public override void StopSkill()
        {
        }
        private void Drop()
        {
            _bossModel.InteractionSystem.StopInteraction(_bossModel.CurrentHand);
            HandDrop?.Invoke();
        }
    }
}