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

        public Dictionary<SkillDictionaryEnum, Dictionary<int, BossBaseSkill>> MainSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> AttackStateSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> DefenceStateSkillDictionary { get; private set; }

        public BossBaseSkill DefaultAttackSkill { get; private set; }
        public BossBaseSkill HorizontalAttackSkill { get; private set; }
        public BossBaseSkill StompSplashSkill { get; private set; }
        public BossBaseSkill RageOfForestSkill { get; private set; }
        public BossBaseSkill PoisonSporesSkill { get; private set; }

        public BossBaseSkill DefaultDefenceSkill { get; private set; }
        public BossBaseSkill SelfHealSkill { get; private set; }
        public BossBaseSkill HardBarkSkill { get; private set; }
        public BossBaseSkill CanibalHealingSkill { get; private set; }
        public BossBaseSkill CallOfForestSkill { get; private set; }


        public BossSkills(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            AttackStateSkillsSettings = _stateMachine._model.BossData.AttackStateSkills;
            DefenceStateSkillsSettings = _stateMachine._model.BossData.DefenceStateSkills;

            MainSkillDictionary = new Dictionary<SkillDictionaryEnum, Dictionary<int, BossBaseSkill>>();
            AttackStateSkillDictionary = new Dictionary<int, BossBaseSkill>();
            DefenceStateSkillDictionary = new Dictionary<int, BossBaseSkill>();

            MainSkillDictionary.Add(SkillDictionaryEnum.AttackStateSkillDictionary, AttackStateSkillDictionary);
            MainSkillDictionary.Add(SkillDictionaryEnum.DefenceStateSkillDictionary, DefenceStateSkillDictionary);

            #region AttackState

            DefaultAttackSkill = new DefaultAttackSkill(AttackStateSkillsSettings.GetDefaultSkillInfo() , AttackStateSkillDictionary, _stateMachine);
            HorizontalAttackSkill = new HorizontalAttackSkill(AttackStateSkillsSettings.GetHorizontalSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            StompSplashSkill = new StompSplashAttackSkill(AttackStateSkillsSettings.GetStompSplashSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            RageOfForestSkill = new RageOfForestAttackSkill(AttackStateSkillsSettings.GetRageOfForestSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            PoisonSporesSkill = new PoisonSporesAttackSkill(AttackStateSkillsSettings.GetPoisonSporesSkillInfo(), AttackStateSkillDictionary, _stateMachine);

            AttackStateSkillDictionary.Add(DefaultAttackSkill.SkillId, DefaultAttackSkill);
            AttackStateSkillDictionary.Add(HorizontalAttackSkill.SkillId, HorizontalAttackSkill);
            AttackStateSkillDictionary.Add(StompSplashSkill.SkillId, StompSplashSkill);
            AttackStateSkillDictionary.Add(RageOfForestSkill.SkillId, RageOfForestSkill);
            AttackStateSkillDictionary.Add(PoisonSporesSkill.SkillId, PoisonSporesSkill);

            #endregion

            #region DefenceState

            DefaultDefenceSkill = new DefaultDefenceSkill(DefenceStateSkillsSettings.GetDefaultDefencelSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            SelfHealSkill = new SelfHealSkill(DefenceStateSkillsSettings.GetSelfHealSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            HardBarkSkill = new HardBark(DefenceStateSkillsSettings.GetHardBarkSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            CallOfForestSkill = new CallOfForest(DefenceStateSkillsSettings.GetCallOfForestSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            CanibalHealingSkill = new CanibalHealingSkill (DefenceStateSkillsSettings.GetCanibalHealingSkillInfo(), DefenceStateSkillDictionary, _stateMachine);

            DefenceStateSkillDictionary.Add(DefaultDefenceSkill.SkillId, DefaultDefenceSkill);
            DefenceStateSkillDictionary.Add(SelfHealSkill.SkillId, SelfHealSkill);
            DefenceStateSkillDictionary.Add(HardBarkSkill.SkillId, HardBarkSkill);
            DefenceStateSkillDictionary.Add(CallOfForestSkill.SkillId, CallOfForestSkill);
            DefenceStateSkillDictionary.Add(CanibalHealingSkill.SkillId, CanibalHealingSkill);

            #endregion


        }

        public void ForceUseSkill(Dictionary<int,BossBaseSkill> dic, int skillId)
        {
            dic[skillId].UseSkill(skillId);
        }
    }
}