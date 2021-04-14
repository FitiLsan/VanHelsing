using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class DefaultDefenceSkill : BossBaseSkill
    {
        private float _healPower = 10f;

        public DefaultDefenceSkill((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        //public DefaultDefenceSkill(bool iint Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
        //    : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        //{
        //}

        public override void StopSkill()
        {
         
        }

        public override void UseSkill(int id)
        {
            Debug.Log("Default Defence Skilll");
            _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _bossModel.BossTransform.position, 0);
            _bossModel.BossAnimator.Play($"DefaultDefence", 0, 0f);
            Block();
            ReloadSkill(id);
        }

        private void Block()
        {
            //BuffService block
        }
        
    }
}