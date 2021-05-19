using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "BossSkillData", menuName = "Enemy/BossSkillsData")]
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
        public BossBaseSkill PoisonSporesSkill { get; private set; }
        public BossBaseSkill PoisonSporesCircleSkill { get; private set; }
        public BossBaseSkill CatchAttackSkill { get; private set; }
        public BossBaseSkill FingerAttackSkill { get; private set; }
        //defence
        public BossBaseSkill DefaultDefenceSkill { get; private set; }
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
        public BossBaseSkill SelfHealSkill { get; private set; }
        public BossBaseSkill RageOfForestSkill { get; private set; }

        public BossSkills(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            AttackStateSkillsSettings = _stateMachine._model.BossData.BossSkills.AttackStateSkills;
            DefenceStateSkillsSettings = _stateMachine._model.BossData.BossSkills.DefenceStateSkills;
            ChasingStateSkillsSettings = _stateMachine._model.BossData.BossSkills.ChasingStateSkills;
            RetreatingStateSkillsSettings = _stateMachine._model.BossData.BossSkills.RetreatingStateSkills;
            SearchingStateSkillsSettings = _stateMachine._model.BossData.BossSkills.SearchingStateSkills;
            DeadStateSkillsSettings = _stateMachine._model.BossData.BossSkills.DeadStateSkills;
            NonStateSkillsSettings = _stateMachine._model.BossData.BossSkills.NonStateSkills;



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
            PoisonSporesSkill = new PoisonSporesAttackSkill(AttackStateSkillsSettings.GetPoisonSporesSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            PoisonSporesCircleSkill = new PoisonSporesCircleAttackSkill(AttackStateSkillsSettings.GetPoisonSporesCircleSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            CatchAttackSkill = new CatchAttackSkill(AttackStateSkillsSettings.GetCatchSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            FingerAttackSkill = new FingerAttackSkill(AttackStateSkillsSettings.GetFingerSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            RageOfForestSkill = new RageOfForestAttackSkill(AttackStateSkillsSettings.GetRageOfForestSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            VineFishingSkill = new VineFishingAttackSkill(AttackStateSkillsSettings.GetVineFishingSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            BushTriggerSkill = new BushTriggerAttackSkill(SearchingStateSkillsSettings.GetBushTriggerSkillInfo(), AttackStateSkillDictionary, _stateMachine);
            CallOfForestSkill = new CallOfForest(DefenceStateSkillsSettings.GetCallOfForestSkillInfo(), AttackStateSkillDictionary, _stateMachine);

            AddSkillToDictionary(DefaultAttackSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(HorizontalAttackSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(StompSplashSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(PoisonSporesSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(PoisonSporesCircleSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(CatchAttackSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(FingerAttackSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(RageOfForestSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(VineFishingSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(BushTriggerSkill, AttackStateSkillDictionary);
            AddSkillToDictionary(CallOfForestSkill, AttackStateSkillDictionary);

            #endregion

            #region DefenceState

            DefaultDefenceSkill = new DefaultDefenceSkill(DefenceStateSkillsSettings.GetDefaultDefencelSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            SelfHealSkill = new SelfHealSkill(DefenceStateSkillsSettings.GetSelfHealSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            HardBarkSkill = new HardBark(DefenceStateSkillsSettings.GetHardBarkSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
          //  CallOfForestSkill = new CallOfForest(DefenceStateSkillsSettings.GetCallOfForestSkillInfo(), DefenceStateSkillDictionary, _stateMachine);
            CanibalHealingSkill = new CanibalHealingSkill (DefenceStateSkillsSettings.GetCanibalHealingSkillInfo(), DefenceStateSkillDictionary, _stateMachine);

            AddSkillToDictionary(DefaultDefenceSkill, DefenceStateSkillDictionary);
            AddSkillToDictionary(SelfHealSkill, DefenceStateSkillDictionary);
            AddSkillToDictionary(HardBarkSkill, DefenceStateSkillDictionary);
          //  AddSkillToDictionary(CallOfForestSkill, DefenceStateSkillDictionary);
            AddSkillToDictionary(CanibalHealingSkill, DefenceStateSkillDictionary);

            #endregion


            #region ChasingState

            #endregion


            #region  RetreatingState

            FakeTreeSkill = new FakeTreeAttackSkill(RetreatingStateSkillsSettings.GetFakeTreeSkillInfo(), RetreatingStateSkillDictionary, _stateMachine);
            AddSkillToDictionary(FakeTreeSkill, RetreatingStateSkillDictionary);

            #endregion


            #region   SearchingState

           // BushTriggerSkill = new BushTriggerAttackSkill(SearchingStateSkillsSettings.GetBushTriggerSkillInfo(), SearchingStateSkillDictionary, _stateMachine);
          //  AddSkillToDictionary(BushTriggerSkill, SearchingStateSkillDictionary);

            #endregion

            #region  DeadStateSkills

            ResurrectionSkill = new ResurrectionAttackSkill(DeadStateSkillsSettings.GetResurrectionSkillInfo(), DeadStateSkillDictionary, _stateMachine);
            AddSkillToDictionary(ResurrectionSkill, DeadStateSkillDictionary);

            #endregion


            #region  NonStateSkills

            TestSkill = new TestAttackSkill(NonStateSkillsSettings.GetTestSkillInfo(), NonStateSkillDictionary, _stateMachine);
            ThrowAttackSkill = new ThrowAttackSkill(NonStateSkillsSettings.GetThrowSkillInfo(), NonStateSkillDictionary, _stateMachine);
            AddSkillToDictionary(TestSkill, NonStateSkillDictionary);
            AddSkillToDictionary(ThrowAttackSkill, NonStateSkillDictionary);

            #endregion


        }

        public void UseSkill(Dictionary<int, BossBaseSkill> dic, int skillId)
        {
            if(dic.ContainsKey(skillId) && dic[skillId].IsSkillReady)
            {
                dic[skillId].UseSkill(skillId);
            }
        }
        public void ForceUseSkill(Dictionary<int,BossBaseSkill> dic, int skillId)
        {
            if (dic.ContainsKey(skillId))
            {
                if (!dic[skillId].IsSkillUsing)
                {
                    dic[skillId].UseSkill(skillId);
                    DOVirtual.DelayedCall(4.5f, () => dic[skillId].IsSkillUsing = false);
                }
            }
            else
            {
                Debug.LogError(skillId);
                throw new Exception($"Skill with ID = {skillId} is Disable");
            }
        }

        private void AddSkillToDictionary(BossBaseSkill skill, Dictionary<int, BossBaseSkill> dic)
        {
            if(skill.IsEnable)
            {
                dic.Add(skill.SkillId, skill);
            }
        }
    }
}