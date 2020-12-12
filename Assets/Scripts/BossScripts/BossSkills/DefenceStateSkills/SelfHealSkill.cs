using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class SelfHealSkill : BossBaseSkill
    {
        private float _healPower = 10f;

        public SelfHealSkill((int, float, float, float, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public SelfHealSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log(" SelfHeal Skill");
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);
            _bossModel.BossAnimator.Play($"SelfHeal", 0, 0f);
            Heal();
            ReloadSkill(id);
        }

        private void Heal()
        {
            _stateMachine._model.CurrentHealth += _healPower * Time.deltaTime;
        }
        
    }
}