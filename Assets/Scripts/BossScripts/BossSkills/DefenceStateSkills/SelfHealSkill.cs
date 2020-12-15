using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class SelfHealSkill : BossBaseSkill
    {
        private float _healPower = 25f;

        public SelfHealSkill((int, float, float, float, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public SelfHealSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public override void StopSkill()
        {
            _bossModel.healAura.Stop();
        }

        public override void UseSkill(int id)
        {
            Debug.Log(" SelfHeal Skill");
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);
            _bossModel.BossAnimator.Play($"SelfHeal", 0, 0f);
            _bossModel.healAura.Play(true);
            TimeRemaining timeRemaining = new TimeRemaining(() => Heal(), 0.5f, true);
            timeRemaining.AddTimeRemaining(0.5f);

            
            //Heal();
            ReloadSkill(id);
        }

        private void Heal()
        {
            _stateMachine._model.CurrentHealth += _healPower * Time.deltaTime;
        }
        
    }
}