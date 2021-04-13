using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class VineFishingAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        private GameObject _vinePrefab; //GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        //public VineFishingAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        //    : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        //{
        //}
        public VineFishingAttackSkill((bool, int, float, float, float, bool, bool, GameObject) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
            _vinePrefab = skillInfo.Item8;
        }

        public override void UseSkill(int id)
        {
            Debug.Log("VineFishingAttackSkill");
            _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _bossModel.BossTransform.position, 0);
            RotateToTarget();
            _bossModel.BossAnimator.Play("VineFishingAttack", 0, 0);
            DelayCall(CreateVine, 1f);
            ReloadSkill(id);
        }

        public override void StopSkill()
        {
        }

        private void CreateVine()
        {

            var vine1 = GameObject.Instantiate(_vinePrefab, _bossModel.BossCurrentTarget.transform.position + new Vector3(5, 0, 0), Quaternion.identity);
            var vine2 = GameObject.Instantiate(_vinePrefab, _bossModel.BossCurrentTarget.transform.position + new Vector3(-5, 0, 0), Quaternion.identity);
        }
    }
}