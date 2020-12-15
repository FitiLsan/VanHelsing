using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class CallOfForest : BossBaseSkill
    {
        private float _callRadius = 50f;
        private GameObject MutationTreePrefab;

        public CallOfForest((int, float, float, float, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public CallOfForest(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        {
        }

        public override void StopSkill()
        {
            
        }

        public override void UseSkill(int id)
        {
            Debug.Log("Call of Forest Skill");
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);
            _bossModel.BossAnimator.Play($"CallOfForest", 0, 0f);
            Call();
            ReloadSkill(id);
        }

        private void Call()
        {
          var list =  Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(_bossModel.BossTransform.position, _callRadius, "Tree");
            foreach (var tree in list)
            {
                TreeMutation(tree);
            }
        }

        private void TreeMutation(GameObject tree)
        {
            var currentTreePosition = tree.transform.position;
            tree.SetActive(false);
            GameObject.Instantiate(MutationTreePrefab, currentTreePosition, Quaternion.identity);
        }
        
    }
}