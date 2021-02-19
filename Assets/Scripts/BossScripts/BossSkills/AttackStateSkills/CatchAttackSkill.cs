using RootMotion.Dynamics;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class CatchAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        public CatchAttackSkill(bool IsEnable, int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
            : base(IsEnable, Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }
        public CatchAttackSkill((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            if (_bossModel.IsPickUped)
            {
                return;
            }
            Debug.Log("CatchtAttackSkill");
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);

            if (_bossModel.ClosestTriggerIndex != -1)
            {
                var interactionTrigger = _bossModel.InteractionSystem.triggersInRange[_bossModel.InteractionSystem.GetClosestTriggerIndex()];

                if (interactionTrigger.name.Equals("TriggerLeft"))
                {
                    _bossModel.BossAnimator.Play("BossCatch_L", 0, 0);
                    Catching();
                }
                if (interactionTrigger.name.Equals("TriggerRight"))
                {
                    _bossModel.BossAnimator.Play("BossCatchAttack_R", 0, 0);
                    DelayCall(() => Catching(), 0.3f);
                }
                _bossModel.targetParent = _bossModel.InteractionTarget.transform.root.gameObject;
            }
            DelayCall(() => _stateMachine.BossSkills.ForceUseSkill(_stateMachine.BossSkills.NonStateSkillDictionary, 1), 3f);
            ReloadSkill(id);
        }

        public override void StopSkill()
        {
            
        }
        
        private void Catching()
        {
            _bossModel.InteractionSystem.TriggerInteraction(_bossModel.ClosestTriggerIndex, false, out _bossModel.CatchTarget);
        }
    }
}