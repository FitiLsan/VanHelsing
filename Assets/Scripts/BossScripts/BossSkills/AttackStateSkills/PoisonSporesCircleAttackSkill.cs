using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class PoisonSporesCircleAttackSkill : BossBaseSkill
    {
        private const float SPORE_LIFE_TIME = 5f;

        public PoisonSporesCircleAttackSkill((bool, int, float, float, float, bool, bool, GameObject, float) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("POISON CIRCLE AttackSkill");
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);
            _bossModel.BossAnimator.Play("PoisonAttack", 0, 0f);
            CreateSpores(5f, 10);
            ReloadSkill(id);
        }

        private void CreateSpores(float radius, int sporeCount)
        {
            var bossPos = _bossModel.BossTransform.position;
            for (var j = 0; j < sporeCount; j++)
            {
                var groundedPosition = Services.SharedInstance.PhysicsService.GetGroundedPosition(CreateCircle(bossPos, radius), bossPos.y + 2);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, bossPos - groundedPosition);

                var spore = GameObject.Instantiate(SkillPrefab, groundedPosition, rotation);
                spore.GetComponent<SporesController>().SetDamage(SkillDamage);
                GameObject.Destroy(spore, SPORE_LIFE_TIME);
                
            }
        }
        public override void StopSkill()
        {
        }

        private Vector3 CreateCircle(Vector3 center, float radius)
        {
            var ang = Random.value * 360;
            Vector3 spawnPosition;
            spawnPosition.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            spawnPosition.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            spawnPosition.y = center.y;
            return spawnPosition;
        }
    }
}