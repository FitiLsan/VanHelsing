using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class CanibalHealingSkill : BossBaseSkill
    {
        private const float CATCH_DELAY = 0.3f;
        private float _catchRadius = 4f;

        public CanibalHealingSkill((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        //public CanibalHealingSkill(bool IsEnable, int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(IsEnable, Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        //{
        //}

        public override void UseSkill(int id)
        {
            Debug.Log("Canibal Healing Skill");
            _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _bossModel.BossTransform.position, 0);
            _bossModel.BossAnimator.Play($"CanibalHealing", 0, 0f);
            TimeRemaining timeRemaining = new TimeRemaining(() => CatchTarget(), CATCH_DELAY);
            timeRemaining.AddTimeRemaining(CATCH_DELAY);
            
            ReloadSkill(id);
        }

        public override void StopSkill()
        {
        }

        private void CatchTarget()
        {
            var catchlist = Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(_bossModel.BossTransform.position, _catchRadius, "Tree");
            if (catchlist.Count != 0)
            {
                var currentCatch = catchlist[0];
                currentCatch.transform.position = _bossModel.LeftHand.position;
                currentCatch.transform.parent = _bossModel.LeftHand;
                //  currentCatch.SetActive(false);
            }
        }
    }
}