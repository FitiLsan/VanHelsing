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

        public CatchAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }
        public CatchAttackSkill((int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("CatchtAttackSkill");
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);

            
            if (_bossModel.ClosestTriggerIndex != -1)
            {
                _bossModel.InteractionSystem.TriggerInteraction(_bossModel.ClosestTriggerIndex, false, out _bossModel.CatchTarget);
                _bossModel.targetParent = _bossModel.InteractionTarget.transform.root.gameObject;
            }
            ReloadSkill(id);
        }

        public override void StopSkill()
        {
            
        }
    }
}