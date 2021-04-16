using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class BossDefencingState : BossBaseState
    {
        #region Constants

        private const float ANIMATION_DELAY = 0.2f;

        #endregion


        #region Fields

        private int _skillId;

        private Dictionary<int, int> _readySkillDictionary = new Dictionary<int, int>();

        #endregion


        #region ClassLifeCycle

        public BossDefencingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void Execute()
        {
            CheckNextMove();
        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = true;
            IsBattleState = true;
            IsAnySkillUsed = false;
            base.CurrentAttackTime = 0f;
            _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _bossModel.BossTransform.position, 0);
            StartCoolDownSkills(_bossSkills.DefenceStateSkillDictionary);
        }

        public override void OnAwake()
        {

        }

        public override void OnExit()
        {
            CurrentSkill?.StopSkill();
        }

        public override void OnTearDown()
        {

        }

        private void ChoosingDefenceSkill(bool isDefault = false)
        {
            _readySkillDictionary.Clear();

            ChooseReadySkills(_bossSkills.DefenceStateSkillDictionary, _readySkillDictionary);


            if (!isDefault & _readySkillDictionary.Count != 0)
            {
                var readyId = UnityEngine.Random.Range(0, _readySkillDictionary.Count);
                _skillId = _readySkillDictionary[readyId];
            }
            else
            {
                _skillId = DEFAULT_ATTACK_ID;
            }

            if (_bossSkills.DefenceStateSkillDictionary.ContainsKey(_skillId))
            {
                CurrentSkill = _bossSkills.DefenceStateSkillDictionary[_skillId];
                CurrentSkill.UseSkill(_skillId);
                IsAnySkillUsed = true;
            }
            else
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
            }
        }


        private void CheckNextMove()
        {
            if (IsAnimationPlay)
            {
                base.CurrentAttackTime = _bossModel.BossAnimator.GetCurrentAnimatorStateInfo(0).length + ANIMATION_DELAY;
                IsAnimationPlay = false;
                //TimeRemaining timeRemaining = new TimeRemaining(() => CurrentSkill.StopSkill(), CurrentAttackTime);
                //timeRemaining.AddTimeRemaining();
            }

            if (base.CurrentAttackTime > 0)
            {
                base.CurrentAttackTime -= Time.deltaTime;

            }
            if (base.CurrentAttackTime <= 0)
            {                                      
               // isAnySkillUsed = false;
                CurrentSkill?.StopSkill();
                if (!IsAnimationPlay & IsAnySkillUsed)
                {
                   _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
                    return;
                }
                DecideNextMove();
            }
        }

        private void DecideNextMove()
        {
            _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _bossModel.BossTransform.position, 0);
            _bossModel.LeftHandBehavior.IsInteractable = false;
            _bossModel.RightHandBehavior.IsInteractable = false;
            _bossModel.LeftHandCollider.enabled = false;
            _bossModel.RightHandCollider.enabled = false;

            if (!_bossModel.CurrentStats.BaseStats.IsDead && CheckDirection())
            {
                ChoosingDefenceSkill();
            }
        }
        #endregion

    }
}