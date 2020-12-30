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
        public ChasingStateSkillsSettings ChasingStateSkillsSettings;
        public RetreatingStateSkillsSettings RetreatingStateSkillsSettings;
        public SearchingStateSkillsSettings SearchingStateSkillsSettings;
        public DeadStateSkillsSettings DeadStateSkillsSettings;
        public NonStateSkillsSettings NonStateSkillsSettings;

        #endregion

        private BossStateMachine _stateMachine;

        public Dictionary<SkillDictionaryEnum, Dictionary<int, BossBaseSkill>> MainSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> AttackStateSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> DefenceStateSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> ChasingStateSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> RetreatingStateSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> SearchingStateSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> DeadStateSkillDictionary { get; private set; }
        public Dictionary<int, BossBaseSkill> NonStateSkillDictionary { get; private set; }

        //attack
        public BossBaseSkill DefaultAttackSkill { get; private set; }
        public BossBaseSkill HorizontalAttackSkill { get; private set; }
        public BossBaseSkill StompSplashSkill { get; private set; }
        public BossBaseSkill RageOfForestSkill { get; private set; }
        public BossBaseSkill PoisonSporesSkill { get; private set; }
        public BossBaseSkill CatchAttackSkill { get; private set; }
        public BossBaseSkill FingerAttackSkill { get; private set; }
        //defence
        public BossBaseSkill DefaultDefenceSkill { get; private set; }
        public BossBaseSkill SelfHealSkill { get; private set; }
        public BossBaseSkill HardBarkSkill { get; private set; }
        public BossBaseSkill CanibalHealingSkill { get; private set; }
        public BossBaseSkill CallOfForestSkill { get; private set; }
        //chasing
        public BossBaseSkill VineFishingSkill { get; private set; }
        //retreating
        public BossBaseSkill FakeTreeSkill { get; private set; }
        //searching
        public BossBaseSkill BushTriggerSkill { get; private set; }
        //dead
        public BossBaseSkill ResurrectionSkill { get; private set; }
        //non
        public BossBaseSkill TestSkill { get; private set; }
        public BossBaseSkill ThrowAttackSkill { get; private set; }


        public BossSkills(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            AttackStateSkillsSettings = _stateMachine._model.BossData.AttackStateSkills;
            DefenceStateSkillsSettings = _stateMachine._model.BossData.DefenceStateSkills;
            ChasingStateSkillsSettings = _stateMachine._model.BossData.ChasingStateSkills;
            RetreatingStateSkillsSettings = _stateMachine._model.BossData.RetreatingStateSkills;
            SearchingStateSkillsSettings = _stateMachine._model.BossData.SearchingStateSkills;
            DeadStateSkillsSettings = _stateMachine._model.BossData.DeadStateSkills;
            NonStateSkillsSettings = _stateMachine._model.BossData.NonStateSkills;



            MainSkillDictionary = new Dictionary<SkillDictionaryEnum, Dictionary<int, BossBaseSkill>>();
            AttackStateSkillDictionary = new Dictionary<int, BossBaseSkill>();
            DefenceStateSkillDictionary = new Dictionary<int, BossBaseSkill>();
            ChasingStateSkillDictionary = new Dictionary<int, BossBaseSkill>();
            RetreatingStateSkillDictionary = new Dictionary<int, BossBaseSkill>();
            SearchingStateSkillDictionary = new Dictionary<int, BossBaseSkill>();
            DeadStateSkillDictionary = new Dictionary<int, BossBaseSkill>();
            NonStateSkillDictionary = new Dictionary<int, BossBaseSkill>();

            MainSkillDictionary.Add(SkillDictionaryEnum.AttackStateSkillDictionary, AttackStateSkillDictionary);
            MainSkillDictionary.Add(SkillDictionaryEnum.ChasingStateSkillDictionary, ChasingStateSkillDictionary);
            MainSkillDictionary.Add(SkillDictionaryEnum.DefenceStateSkillDictionary, DefenceStateSkillDictionary);
            MainSkillDictionary.Add(SkillDictionaryEnum.RetreatingStateSkillDictionary, RetreatingStateSkillDictionary);
            MainSkillDictionary.Add(SkillDictionaryEnum.SearchingStateSkillDictionary, SearchingStateSkillDictionary);
            MainSkillDictionary.Add(SkillDictionaryEnum.DeadStateSkillDictionary, DeadStateSkillDictionary);
            MainSkillDictionary.Add(SkillDictionaryEnum.NonStateSkillDictionary, NonStateSkillDictionary);


            #region AttackState

            DefaultAttackSkill = new DefaultAttackSkill(AttackStateSkillsSettings.GetDefaultSkillInfo() , AttackStateSkillDictionary, _stateMachine);
            HorizontalAttackSkill = new HorizontalAttackSkill(AttackStateSkillsSettings.GetHorizontalSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            StompSplashSkill = new StompSplashAttackSkill(AttackStateSkillsSettings.GetStompSplashSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            RageOfForestSkill = new RageOfForestAttackSkill(AttackStateSkillsSettings.GetRageOfForestSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            PoisonSporesSkill = new PoisonSporesAttackSkill(AttackStateSkillsSettings.GetPoisonSporesSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            CatchAttackSkill = new CatchAttackSkill(AttackStateSkillsSettings.GetCatchSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            FingerAttackSkill = new FingerAttackSkill(AttackStateSkillsSettings.GetFingerSkillInfo(), AttackStateSkillDictionary, _stateMachine);

            AttackStateSkillDictionary.Add(DefaultAttackSkill.SkillId, DefaultAttackSkill);
            AttackStateSkillDictionary.Add(HorizontalAttackSkill.SkillId, HorizontalAttackSkill);
            AttackStateSkillDictionary.Add(StompSplashSkill.SkillId, StompSplashSkill);
            AttackStateSkillDictionary.Add(RageOfForestSkill.SkillId, RageOfForestSkill);
            AttackStateSkillDictionary.Add(PoisonSporesSkill.SkillId, PoisonSporesSkill);
            AttackStateSkillDictionary.Add(CatchAttackSkill.SkillId, CatchAttackSkill);
            AttackStateSkillDictionary.Add(FingerAttackSkill.SkillId, FingerAttackSkill);

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


            #region ChasingState

            VineFishingSkill = new VineFishingAttackSkill(ChasingStateSkillsSettings.GetVineFishingSkillInfo(), ChasingStateSkillDictionary, _stateMachine);
            ChasingStateSkillDictionary.Add(VineFishingSkill.SkillId, VineFishingSkill);

            #endregion


            #region  RetreatingState

            FakeTreeSkill = new FakeTreeAttackSkill(RetreatingStateSkillsSettings.GetFakeTreeSkillInfo(), RetreatingStateSkillDictionary, _stateMachine);
            RetreatingStateSkillDictionary.Add(FakeTreeSkill.SkillId, FakeTreeSkill);

            #endregion


            #region   SearchingState

            BushTriggerSkill = new BushTriggerAttackSkill(SearchingStateSkillsSettings.GetBushTriggerSkillInfo(), SearchingStateSkillDictionary, _stateMachine);
            SearchingStateSkillDictionary.Add(BushTriggerSkill.SkillId, BushTriggerSkill);

            #endregion

            #region  DeadStateSkills

            ResurrectionSkill = new ResurrectionAttackSkill(DeadStateSkillsSettings.GetResurrectionSkillInfo(), DeadStateSkillDictionary, _stateMachine);
            DeadStateSkillDictionary.Add(ResurrectionSkill.SkillId, ResurrectionSkill);
            #endregion


            #region  NonStateSkills

            TestSkill = new TestAttackSkill(NonStateSkillsSettings.GetTestSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            ThrowAttackSkill = new ThrowAttackSkill(NonStateSkillsSettings.GetThrowSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            NonStateSkillDictionary.Add(ThrowAttackSkill.SkillId, ThrowAttackSkill);
            NonStateSkillDictionary.Add(TestSkill.SkillId, TestSkill);

            #endregion


        }

        public void ForceUseSkill(Dictionary<int,BossBaseSkill> dic, int skillId)
        {
            dic[skillId].UseSkill(skillId);
        }
    }
}