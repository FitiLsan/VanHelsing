using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class ResurrectionAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        public ResurrectionAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }
        public ResurrectionAttackSkill((int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("ResurrectionAttackSkill");
            _bossModel.BossAnimator.Play("Resurrection", 0, 0);
            _bossModel.CurrentHealth = _bossModel.BossData._bossStats.MainStats.MaxHealth / 2;
            _stateMachine._model.BossNavAgent.enabled = true;
            _stateMachine._model.IsDead = false ;
            ReloadSkill(id);
        }

        public override void StopSkill()
        {
        }
    }
}