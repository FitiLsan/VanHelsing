using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BeastHunter
{

    public class BushTriggerAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        public BushTriggerAttackSkill((bool, int, float, float, float, bool, bool, GameObject, int) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }


        public override void UseSkill(int id)
        {
            Debug.Log("BushTriggerAttackSkill");
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);
            _bossModel.BossAnimator.Play($"ThrowAttack_R", 0, 0f);
            DG.Tweening.DOVirtual.DelayedCall(0.7f, ThrowSeeds);
            ReloadSkill(id);
        }

        private void ThrowSeeds()
        {
            var bushCount = Random.Range(3, SkillCount);
            for (int i = 0; i < bushCount; i++)
            {
                var distance = _bossModel.BossData.GetTargetDistance(_bossModel.BossTransform.position, _bossModel.BossCurrentTarget.transform.position);
                var seed = GameObject.Instantiate(SkillPrefab, _bossModel.RightHand.position + RandomVector(), _bossModel.BossTransform.rotation);
                seed.transform.LookAt(_bossModel.BossCurrentTarget.transform);
                seed.transform.Rotate(RandomVector(-10, 10));
                seed.GetComponent<Rigidbody>().AddForce((seed.transform.forward + seed.transform.up / 2) * (distance / 2 - RandomFloat()), ForceMode.Impulse);
            }
        }

        private float RandomFloat(float from = -1, float to = 1)
        {
            return Random.Range(from, to);
        }

        private Vector3 RandomVector(float from = -1, float to = 1)
        {
            return new Vector3(RandomFloat(from, to), RandomFloat(from, to), RandomFloat(from, to));
        }
        public override void StopSkill()
        {
        }
    }
}