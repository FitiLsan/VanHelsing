using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public abstract class BossBaseState
    {
        protected const float ANGLE_SPEED_MULTIPLIER = 10f;
        protected const float ANGLE_TARGET_RANGE_MIN = 20f;
        protected const int DEFAULT_ATTACK_ID = 0;
        protected const int SKIP_ID = -1;
        protected const float DISTANCE_TO_START_ATTACK = 4f;

        #region Fields

        protected BossStateMachine _stateMachine;
        protected BossData _bossData;
        protected BossModel _bossModel;
        protected BossMainState _mainState;
        protected BossSkills _bossSkills;

        #endregion


        #region Properties

        public bool CanExit { get; protected set; }
        public bool CanBeOverriden { get; protected set; }
        public bool CurrentStateType { get; protected set; }
        public bool IsBattleState { get; protected set; }

        public bool IsAnimationPlay { get; set; }
        public bool IsRotating { get; private set; }
        public bool IsAnySkillUsed { get; protected set; }
        public float CurrentAttackTime { get; protected set; }
        public BossBaseSkill CurrentSkill { get; protected set; }
        public EffectType CurrentEffectType { get; set; }

        #endregion


        #region ClassLifeCycle

        public BossBaseState(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _bossData = _stateMachine._model.BossData;
            _bossModel = _stateMachine._model;
            _mainState = _stateMachine._mainState;
            _bossSkills = _stateMachine.BossSkills;
        }

        #endregion


        #region Methods

        public abstract void OnAwake();

        public abstract void Initialise();

        public abstract void Execute();

        public abstract void OnExit();

        public abstract void OnTearDown();

        protected bool CheckDirection()
        {
            if (_bossModel.IsPickUped)
            {
                return true;
            }
            var isNear = _bossData.CheckIsLookAtTarget(_bossModel.BossTransform.rotation, _mainState.TargetRotation, ANGLE_TARGET_RANGE_MIN);
            return isNear;
        }

        protected void AnimateRotation()
        {
            if (_bossModel.BossCurrentTarget == null || !IsRotating)
            {
                return;
            }
            Vector3 heading = _bossModel.BossCurrentTarget.transform.position -
                _bossModel.BossTransform.position;

            int directionNumber = _bossData.AngleDirection(
                _bossModel.BossTransform.forward, heading, _bossModel.BossTransform.up);
            switch (directionNumber)
            {
                case -1:
                    _bossModel.BossAnimator.Play("TurningLeftState", 0, 0f);
                    IsAnimationPlay = true;
                    break;
                case 0:
                    _bossModel.BossAnimator.Play("IdleState", 0, 0f);
                    break;
                case 1:
                    _bossModel.BossAnimator.Play("TurningRightState", 0, 0f);
                    IsAnimationPlay = true;
                    break;
                default:
                    _bossModel.BossAnimator.Play("IdleState", 0, 0f);
                    break;
            }
        }


        protected void RotateToTarget()
        {
            if (_bossModel.BossCurrentTarget != null && !CheckDirection())
            {
                IsRotating = true;
                _bossModel.BossTransform.rotation = _bossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, _bossData._bossSettings.RotateSpeed * ANGLE_SPEED_MULTIPLIER);
            }
            else
            {
                IsRotating = false;
            }
        }

        protected bool CheckDistance(float distanceRangeMin, float distanceRangeMax)
        {
            if (distanceRangeMin == -1)
            {
                return true;
            }
            var isNear = false;
            if (_bossModel.BossCurrentTarget != null)
            {
                isNear = _bossData.CheckIsNearTarget(_bossModel.BossTransform.position, _bossModel.BossCurrentTarget.transform.position, distanceRangeMin, distanceRangeMax);
            }
            return isNear;
        }
        #region Skills
        protected void CurrentSkillStop()
        {
            _stateMachine.CurrentState.IsAnySkillUsed = false;
            _stateMachine.CurrentState.CurrentAttackTime = 0;
            if (CurrentSkill != null)
            {
                _stateMachine.CurrentState.CurrentSkill.StopSkill();
            }
        }

        protected virtual void StartCoolDownSkills(Dictionary<int, BossBaseSkill> dic)
        {
            foreach (var skill in dic)
            {
                dic[skill.Key].StartCooldown(skill.Key, dic[skill.Key].SkillCooldown);
            }
        }

        protected virtual void ChooseReadySkills(Dictionary<int, BossBaseSkill> dic, Dictionary<int, int> readyDic)
        {
            var count = 0;
            foreach (var skill in dic)
            {
                if (dic[skill.Key].IsSkillReady && CheckDistance(dic[skill.Key].SkillRangeMin, dic[skill.Key].SkillRangeMax))
                {
                    if (dic[skill.Key].IsNeedRage && !_bossModel.IsRage)
                    {
                        continue;
                    }
                    readyDic.Add(count, skill.Key);
                    count++;
                }
            }
        }
        #endregion

        #region EffectReaction

        protected virtual void FireReaction()
        {
        }

        protected virtual void WaterReaction()
        {

        }

        #endregion

        #endregion
    }
}

