using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class HardBark : BossBaseSkill
    {
        public HardBark((int, float, float, float, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public HardBark(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("Hard Bark Skill");
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);
            _bossModel.BossAnimator.Play($"HardBark", 0, 0f);
            CreateBark();
            ReloadSkill(id);
        }

        private void CreateBark()
        {
          //BuffService
        }
        
    }
}