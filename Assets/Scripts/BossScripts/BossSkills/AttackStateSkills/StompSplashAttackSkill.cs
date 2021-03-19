using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class StompSplashAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;
        private const float UPWARDS_MODIFIER = 1.5f;
        private const float DELAY_BEFORE_STANDING = 1.5f;
        private const float DEFAULT_FORCE = 50f;
        private const float DELAY_AFTER_ANIM = 0.65f;

        private GameObject _target;
        private float _force;
        private float _radius;
        private Rigidbody _rigidBody;
        public StompSplashAttackSkill(bool IsEnable, int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(IsEnable, Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public StompSplashAttackSkill((bool, int, float, float, float, bool, bool, float) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
            _radius = SkillRangeMax;
        }

        public override void UseSkill(int id)
        {
            Debug.Log("StompAttackSkill");
            _bossModel.BossAnimator.Play("BossStompAttack", 0, 0f);
            var TimeRem = new TimeRemaining(() => StompShockWave(), DELAY_AFTER_ANIM);
            TimeRem.AddTimeRemaining(DELAY_AFTER_ANIM);
            ReloadSkill(id);
        }

        private void StompShockWave()
        {
            _bossModel.leftStompEffect.Play(true);
            var list = Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(_bossModel.LeftFoot.position, _radius, "Player");
            if (list.Count != 0)
            {
                _target = list.Find(x => x.name == "Player");

                var rb = _target.GetComponent<Rigidbody>();
                var pm = _target.transform.parent.Find("PuppetMaster").GetComponent<PuppetMaster>(); // убрать если PM будет только у игрока

                if (pm != null && rb != null)
                {
                    pm.state = PuppetMaster.State.Frozen; // Заменить на событие, которое будет менять стейт игроку на "в полете"

                    rb.AddExplosionForce(DEFAULT_FORCE, _bossModel.LeftFoot.transform.position, _radius, UPWARDS_MODIFIER, ForceMode.Impulse);
                    DelayCall(() => pm.state = PuppetMaster.State.Alive, DELAY_BEFORE_STANDING);
                    Damage();
                }
            }
        }
        public override void StopSkill()
        {
        }
        private void Damage()
        {
            var damage = new Damage();
            damage.PhysicalDamage = Random.Range(15f, 30f);
            Services.SharedInstance.AttackService.CountAndDealDamage(damage, _target.transform.root.gameObject.GetInstanceID());;
        }
    }
}