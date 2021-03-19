using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class FingerAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0f;
        private Transform _aimTarget;
        public FingerAttackSkill(bool IsEnable, int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
            : base(IsEnable, Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }
        public FingerAttackSkill((bool, int, float, float, float, bool, bool, float) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("FingerAttackSkill");
            _bossModel.RightHandAimIKTarget.position = _bossModel.BossCurrentTarget.transform.position + new Vector3(0, 1, 0);
            _bossModel.RightHandAimIK.solver.target = _bossModel.RightHandAimIKTarget;
            _bossModel.RightHandAimIK.solver.IKPositionWeight = 1f;
           _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);
            _bossModel.BossAnimator.Play("FingerAttack", 0, 0);
            DOVirtual.DelayedCall(0.1f, () => _bossModel.RightFingerTrigger.IsInteractable = true);
           // TurnOnHitBoxTrigger(_bossModel.RightFingerTrigger, _stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);
            DelayCall(() => ResetAimIk(), 2f);
            ReloadSkill(id);
        }

        private void ResetAimIk()
        {
            _bossModel.RightHandAimIK.solver.target = null;
            _bossModel.RightHandAimIK.solver.IKPositionWeight = 0f;
        }
        public override void StopSkill()
        {
        }
    }
}