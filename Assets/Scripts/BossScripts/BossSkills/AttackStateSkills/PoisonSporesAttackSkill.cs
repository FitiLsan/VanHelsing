using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class PoisonSporesAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        public PoisonSporesAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public PoisonSporesAttackSkill((int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("POISONAttackSkill");
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);
            _bossModel.BossAnimator.Play("PoisonAttack", 0, 0f);
            CreateSpores();

            TurnOnHitBoxTrigger(_currenTriggertHand,_stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);

            ReloadSkill(id);
        }

        private void CreateSpores()
        {
            var bossPos = _bossModel.BossTransform.position;
            var targetPos = _bossModel.BossCurrentTarget.transform.position;
            var distance = _bossModel.BossData.GetTargetDistance(bossPos, targetPos);
            var shortDistance = (int)distance / 2;
            var vector = targetPos - bossPos;
            var shortVector = vector / shortDistance;
            for (var j = 1; j <= shortDistance + 3; j++)
            {
                float horizontalOffset = UnityEngine.Random.Range(-2f, 2f);
                if (j % 2 == 0)
                {
                    horizontalOffset *= -1;
                }
                var multPos = shortVector * j + new Vector3(horizontalOffset, 0, 0);
                var groundedPosition = Services.SharedInstance.PhysicsService.GetGroundedPosition(bossPos + multPos, 20f);
                var TimeRem = new TimeRemaining(() => GameObject.Destroy(GameObject.Instantiate(_bossModel.SporePrefab, groundedPosition, Quaternion.identity), 5f), j * 0.1f);
                TimeRem.AddTimeRemaining(j * 0.1f);
            }
        }
        public override void StopSkill()
        {
        }
    }
}