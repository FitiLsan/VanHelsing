using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class SelfHealSkill : BossBaseSkill
    {
        private float _healPower = 55f;
        private TimeRemaining _delayCall;

        public SelfHealSkill((int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
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
            _bossModel.healAura.Play(true);

            DelayCall(() => Heal(), 0.5f, out _delayCall, true);

            ReloadSkill(id);
        }

        public override void StopSkill()
        {
            _bossModel.healAura.Stop();
            _delayCall.RemoveTimeRemaining();
        }

        private void Heal()
        {
            var maxHealth = _stateMachine._model.BossData._bossStats.MainStats.MaxHealth;

            _stateMachine._model.CurrentHealth += _healPower * Time.deltaTime;
            if (_stateMachine._model.CurrentHealth >= maxHealth)
            {
                _stateMachine._model.CurrentHealth = maxHealth;
            }
        }

    }
}