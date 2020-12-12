using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "BossSkillData")]
    public sealed class BossSkills : ScriptableObject
    {
        #region PrivateData

        public AttackStateSkillsSettings AttackStateSkillsSettings;
        public DefenceStateSkillsSettings DefenceStateSkillsSettings;

        #endregion

        private BossStateMachine _stateMachine;

        public Dictionary<int, BossBaseSkill> AttackStateSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> DefenceStateSkillDictionary { get; private set; }

        public BossBaseSkill DefaultSkill { get; private set; }
        public BossBaseSkill HorizontalAttackSkill { get; private set; }
        public BossBaseSkill StompSplashSkill { get; private set; }
        public BossBaseSkill RageOfForestSkill { get; private set; }
        public BossBaseSkill PoisonSporesSkill { get; private set; }

        public BossBaseSkill SelfHealSkill { get; private set; }
        public BossBaseSkill HardBarkSkill { get; private set; }
        public BossBaseSkill CanibalHealingSkill { get; private set; }
        public BossBaseSkill CallOfForestSkill { get; private set; }


        public BossSkills(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            AttackStateSkillDictionary = new Dictionary<int, BossBaseSkill>();
            DefenceStateSkillDictionary = new Dictionary<int, BossBaseSkill>();
         
            AttackStateSkillsSettings = _stateMachine._model.BossData.AttackStateSkills;
            DefenceStateSkillsSettings = _stateMachine._model.BossData.DefenceStateSkills;

            #region AttackState

            DefaultSkill = new DefaultSkill(AttackStateSkillsSettings.GetDefaultSkillInfo() , AttackStateSkillDictionary, _stateMachine);
            HorizontalAttackSkill = new HorizontalAttackSkill(AttackStateSkillsSettings.GetHorizontalSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            StompSplashSkill = new StompSplashAttackSkill(AttackStateSkillsSettings.GetStompSplashSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            RageOfForestSkill = new RageOfForestAttackSkill(AttackStateSkillsSettings.GetRageOfForestSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            PoisonSporesSkill = new PoisonSporesAttackSkill(AttackStateSkillsSettings.GetPoisonSporesSkillInfo(), AttackStateSkillDictionary, _stateMachine);

            AttackStateSkillDictionary.Add(DefaultSkill.AttackId, DefaultSkill);
            AttackStateSkillDictionary.Add(HorizontalAttackSkill.AttackId, HorizontalAttackSkill);
            AttackStateSkillDictionary.Add(StompSplashSkill.AttackId, StompSplashSkill);
            AttackStateSkillDictionary.Add(RageOfForestSkill.AttackId, RageOfForestSkill);
            AttackStateSkillDictionary.Add(PoisonSporesSkill.AttackId, PoisonSporesSkill);

            #endregion

            #region DefenceState

            SelfHealSkill = new SelfHealSkill(DefenceStateSkillsSettings.GetSelfHealSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            HardBarkSkill = new HardBark(DefenceStateSkillsSettings.GetHardBarkSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            CallOfForestSkill = new CallOfForest(DefenceStateSkillsSettings.GetCallOfForestSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            CanibalHealingSkill = new CanibalHealingSkill (DefenceStateSkillsSettings.GetCanibalHealingSkillInfo(), DefenceStateSkillDictionary, _stateMachine);

            DefenceStateSkillDictionary.Add(SelfHealSkill.AttackId, SelfHealSkill);
            DefenceStateSkillDictionary.Add(HardBarkSkill.AttackId, HardBarkSkill);
            DefenceStateSkillDictionary.Add(CallOfForestSkill.AttackId, CallOfForestSkill);
            DefenceStateSkillDictionary.Add(CanibalHealingSkill.AttackId, CanibalHealingSkill);

            #endregion
        }
    }
}