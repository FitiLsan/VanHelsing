﻿using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class StompSplashAttackSkill : BossBaseSkill
    {
        private const float DELAY_HAND_TRIGGER = 0.2f;

        public StompSplashAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public StompSplashAttackSkill((int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("StompAttackSkill");
            _bossModel.BossAnimator.Play("BossStompAttack", 0, 0f);
            var TimeRem = new TimeRemaining(() => StompShockWave(), 0.65f);
            TimeRem.AddTimeRemaining(0.65f);

            TurnOnHitBoxTrigger(_currenTriggertHand,_stateMachine.CurrentState.CurrentAttackTime, DELAY_HAND_TRIGGER);

            ReloadSkill(id);
        }

        private void StompShockWave()
        {
            _bossModel.leftStompEffect.Play(true);
            var force = 50f;
            var list = Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(_bossModel.LeftFoot.position, 20f, "Player");
            if (list.Count != 0)
            {
                var target = list.Find(x => x.name == "Player");
                var rb = target.GetComponent<Rigidbody>();
                var pm = target.transform.parent.Find("PuppetMaster").GetComponent<PuppetMaster>();

                pm.state = PuppetMaster.State.Frozen;
                rb.AddExplosionForce(force, _bossModel.LeftFoot.transform.position, 15f, 1.5f, ForceMode.Impulse);
                DelayCall(() => pm.state = PuppetMaster.State.Alive, 2f);

            }


            //foreach (var obj in list)
            //{
            //    if (list.Count != 0)
            //    {
            //        obj.GetComponent<Rigidbody>().AddForce((_bossModel.LeftFoot.position - _bossModel.BossCurrentPosition) * force, ForceMode.Impulse);
            //    }
            //}

        }
        public override void StopSkill()
        {
        }
    }
}