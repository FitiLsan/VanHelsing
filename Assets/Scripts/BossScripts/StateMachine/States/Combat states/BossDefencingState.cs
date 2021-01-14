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
            isAnySkillUsed = false;
            base.CurrentAttackTime = 0f;
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);

            for (var i = 0; i < _stateMachine.BossSkills.DefenceStateSkillDictionary.Count; i++)
            {
                _stateMachine.BossSkills.DefenceStateSkillDictionary[i].StartCooldown(_stateMachine.BossSkills.DefenceStateSkillDictionary[i].SkillId, _stateMachine.BossSkills.DefenceStateSkillDictionary[i].SkillCooldown);
            }
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
            var j = 0;


            for (var i = 0; i < _stateMachine.BossSkills.DefenceStateSkillDictionary.Count; i++)
            {
                if (_stateMachine.BossSkills.DefenceStateSkillDictionary[i].IsSkillReady)
                {
                    if (CheckDistance(_stateMachine.BossSkills.DefenceStateSkillDictionary[i].SkillRangeMin, _stateMachine.BossSkills.DefenceStateSkillDictionary[i].SkillRangeMax))
                    {
                        _readySkillDictionary.Add(j, i);
                        j++;
                    }
                }
            }

            //if (_readySkillDictionary.Count == 0 & _bossData.GetTargetDistance(_bossModel.BossTransform.position, _bossModel.BossCurrentTarget.transform.position) >= DISTANCE_TO_START_ATTACK)
            //{
            //    _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
            //    return;
            //}

            if (!isDefault & _readySkillDictionary.Count != 0)
            {
                var readyId = UnityEngine.Random.Range(0, _readySkillDictionary.Count);
                _skillId = _readySkillDictionary[readyId];
            }
            else
            {
                _skillId = DEFAULT_ATTACK_ID;
            }

            CurrentSkill = _stateMachine.BossSkills.DefenceStateSkillDictionary[_skillId];
            CurrentSkill.UseSkill(_skillId);
            isAnySkillUsed = true;
        }


        private void CheckNextMove()
        {
            if (isAnimationPlay)
            {
                base.CurrentAttackTime = _bossModel.BossAnimator.GetCurrentAnimatorStateInfo(0).length + ANIMATION_DELAY;
                isAnimationPlay = false;
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
                if (!isAnimationPlay & isAnySkillUsed)
                {
                   _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
                    return;
                }
                DecideNextMove();
            }
        }

        private void DecideNextMove()
        {
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);
            _bossModel.LeftHandBehavior.IsInteractable = false;
            _bossModel.RightHandBehavior.IsInteractable = false;
            _bossModel.LeftHandCollider.enabled = false;
            _bossModel.RightHandCollider.enabled = false;

            if (!_bossModel.IsDead && CheckDirection())
            {
                ChoosingDefenceSkill();
            }
        }
        #endregion

    }
}