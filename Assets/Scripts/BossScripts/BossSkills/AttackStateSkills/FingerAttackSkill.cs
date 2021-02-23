using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class FingerAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        public FingerAttackSkill(bool IsEnable, int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
            : base(IsEnable, Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }
        public FingerAttackSkill((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("FingerAttackSkill");
            _bossModel.RightHandAimIK.solver.target = _bossModel.BossCurrentTarget.transform;
           _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);
            _bossModel.BossAnimator.Play("FingerAttack", 0, 0);
            TurnOnHitBoxTrigger(_bossModel.RightFingerTrigger, _stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);
            DelayCall(() => _bossModel.RightHandAimIK.solver.target = null, 0.5f);
            ReloadSkill(id);
        }

        public override void StopSkill()
        {
        }
    }
}