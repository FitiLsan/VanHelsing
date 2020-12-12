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
            base.CurrentAttackTime = 1.5f;
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);

            for (var i = 0; i < _stateMachine.BossSkills.DefenceStateSkillDictionary.Count; i++)
            {
                _stateMachine.BossSkills.DefenceStateSkillDictionary[i].SkillCooldown(_stateMachine.BossSkills.DefenceStateSkillDictionary[i].AttackId, _stateMachine.BossSkills.DefenceStateSkillDictionary[i].AttackCooldown);
            }
            ChoosingDefenceSkill();
        }

        public override void OnAwake()
        {

        }

        public override void OnExit()
        {

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
                if (_stateMachine.BossSkills.DefenceStateSkillDictionary[i].IsAttackReady)
                {
                    if (CheckDistance(_stateMachine.BossSkills.DefenceStateSkillDictionary[i].AttackRangeMin, _stateMachine.BossSkills.DefenceStateSkillDictionary[i].AttackRangeMax))
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

            _stateMachine.BossSkills.DefenceStateSkillDictionary[_skillId].UseSkill(_skillId);
        }


        private void CheckNextMove()
        {
            if (isAnimationPlay)
            {
                base.CurrentAttackTime = _bossModel.BossAnimator.GetCurrentAnimatorStateInfo(0).length + ANIMATION_DELAY;
                isAnimationPlay = false;
            }

            if (base.CurrentAttackTime > 0)
            {
                base.CurrentAttackTime -= Time.deltaTime;

            }
            if (base.CurrentAttackTime <= 0)
            {
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