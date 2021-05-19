using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class CallOfForest : BossBaseSkill
    {
        private float _callRadius;

        public CallOfForest((bool, int, float, float, float, bool, bool, GameObject) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        //public CallOfForest(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
        //    : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        //{
        //}

        public override void StopSkill()
        {
            _bossModel.callOfForestEffect.Stop();
        }

        public override void UseSkill(int id)
        {
            Debug.Log("Call of Forest Skill");
            _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _bossModel.BossTransform.position, 0);
            _bossModel.BossAnimator.Play($"CallOfForest", 0, 0f);
            _bossModel.callOfForestEffect.Play();
            DG.Tweening.DOVirtual.DelayedCall(3f, Call);
            ReloadSkill(id);
        }

        private void Call()
        {
          var list =  Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(_bossModel.BossTransform.position, SkillRangeMax, "Tree");
            foreach (var tree in list)
            {
                TreeMutation(tree);
            }
            DG.Tweening.DOVirtual.DelayedCall(2f, _bossModel.callOfForestEffect.Stop);
        }

        private void TreeMutation(GameObject tree)
        {
            var currentTreePosition = tree.transform.position;
            tree.SetActive(false);
            GameObject.Instantiate(SkillPrefab, currentTreePosition, Quaternion.identity);
        }
        
    }
}