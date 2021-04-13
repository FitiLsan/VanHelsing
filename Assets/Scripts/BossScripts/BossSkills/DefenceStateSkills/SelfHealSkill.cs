using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace BeastHunter
{
    public class SelfHealSkill : BossBaseSkill
    {
        private float _healPower = 55f;
        private TimeRemaining _delayCall;
        private GameObject _guardPrefab = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        public SelfHealSkill((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
            : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        //public SelfHealSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        //    : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        //{
        //}

        public override void UseSkill(int id)
        {
            Debug.Log(" SelfHeal Skill");
            _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _bossModel.BossTransform.position, 0);
            _bossModel.BossAnimator.Play($"SelfHeal", 0, 0f);
            _bossModel.healAura.Play(true);

            DelayCall(() => Heal(), 0.5f, out _delayCall, true);
            CreateGuards();
            ReloadSkill(id);
        }

        public override void StopSkill()
        {
            _bossModel.healAura.Stop();
            _delayCall.RemoveTimeRemaining();
        }

        private void Heal()
        {
            _bossModel.CurrentStats.BaseStats.CurrentHealthPoints += _healPower * Time.deltaTime;
        }
        
        private void CreateGuards()
        {
           var GuardCount =  Random.Range(1, 6);
            for (var i = 0; i < GuardCount; i++)
            {
                var x = Random.Range(-8f, 8f);
                var y = Random.Range(-8f, 8f);

                var guard = GameObject.Instantiate(_guardPrefab, _bossModel.BossTransform.position + new Vector3(x, 0, y), Quaternion.identity);
                guard.transform.DOMove(_bossModel.BossCurrentTarget.transform.position, 5f); //test
                GameObject.Destroy(guard, 7f);
            }
            
        }

    }
}