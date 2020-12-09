using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "BossSkillData")]
    public sealed class BossSkills : ScriptableObject
    {
        #region PrivateData

        public AttackStateSkillsSettings AttackStateSkills;

        #endregion
        private BossStateMachine _stateMachine;
        public Dictionary<int, BossBaseSkill> AttackStateSkillDictionary { get; private set; } = new Dictionary<int, BossBaseSkill>();
        public Dictionary<int, int> _readyAttackSkillDictionary { get; private set; } = new Dictionary<int, int>();

        public BossBaseSkill DefaultSkill { get; private set; }
        public BossBaseSkill HorizontalAttackSkill { get; private set; }
        public BossBaseSkill StompSplashSkill { get; private set; }
        public BossBaseSkill RageOfForestSkill { get; private set; }
        public BossBaseSkill PoisonSporesSkill { get; private set; }


        public BossSkills(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            AttackStateSkills = _stateMachine._model.BossData.AttackStateSkills;
            DefaultSkill = new DefaultSkill(AttackStateSkills.GetDefaultSkillInfo() , AttackStateSkillDictionary, _stateMachine);
            HorizontalAttackSkill = new HorizontalAttackSkill(AttackStateSkills.GetHorizontalSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            StompSplashSkill = new StompSplashAttackSkill(AttackStateSkills.GetStompSplashSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            RageOfForestSkill = new RageOfForestAttackSkill(AttackStateSkills.GetRageOfForestSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            PoisonSporesSkill = new PoisonSporesAttackSkill(AttackStateSkills.GetPoisonSporesSkillInfo(), AttackStateSkillDictionary, _stateMachine);

            AttackStateSkillDictionary.Add(DefaultSkill.AttackId, DefaultSkill);
            AttackStateSkillDictionary.Add(HorizontalAttackSkill.AttackId, HorizontalAttackSkill);
            AttackStateSkillDictionary.Add(StompSplashSkill.AttackId, StompSplashSkill);
            AttackStateSkillDictionary.Add(RageOfForestSkill.AttackId, RageOfForestSkill);
            AttackStateSkillDictionary.Add(PoisonSporesSkill.AttackId, PoisonSporesSkill);
        }
    }
}