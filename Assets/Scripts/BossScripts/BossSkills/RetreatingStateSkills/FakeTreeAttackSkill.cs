using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public class FakeTreeAttackSkill : BossBaseSkill
    {
        private const float DELAY = 0.1f;
        private const float ROOT_CATCH_DELAY = 2f;

        private GameObject FakeTreePrefab;

        //public FakeTreeAttackSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        //    : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        //{
        //}
        public FakeTreeAttackSkill((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        public override void UseSkill(int id)
        {
            Debug.Log("FakeTreeAttackSkill");

            _bossModel.BossAnimator.Play("RootCatch",0,0);

            DelayCall(FakeTreeCreated, ROOT_CATCH_DELAY);

            ReloadSkill(id);
        }

        public override void StopSkill()
        {
        }

        private void FakeTreeCreated()
        {
            var num = Random.Range(0, 2);
            _bossModel.BossAnimator.Play($"FakeTree {num}", 0, 0);
            var fakeTree = GameObject.Instantiate(_stateMachine.BossSkills.RetreatingStateSkillsSettings.FakeTreePrefab, _bossModel.BossTransform.position, _bossModel.BossTransform.rotation);
            fakeTree.GetComponent<Animator>().Play($"FakeTree {num}", 0, 0);
            DelayCall(()=> _bossModel.BossTransform.position = _bossModel.Lair.transform.position, DELAY);
        }
    }
}