using UnityEngine;
using System;


namespace BeastHunter
{
    public class RollingTargetState : CharacterBaseState
    {
        #region Constants

        private const float ROLL_LOW_SIDE_ANGLE = 45f;
        private const float ROLL_HALF_SIDE_ANGLE = 90f;
        private const float ROLL_BACK_SIDE_ANGLE = 225f;
        private const float ROLL_BACK_ANGLE = 180f;
        private const float ANGLE_SPEED_INCREASE = 1.225f;

        #endregion


        #region Fields

        private Vector3 MoveDirection;
        private Collider ClosestEnemy;
        private float _angleSpeedIncrease;

        #endregion


        #region Properties

        public Action OnRollEnd { get; set; }
        private float RollTime { get; set; }

        #endregion


        #region ClassLifeCycle

        public RollingTargetState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            CanExit = false;
            CanBeOverriden = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            if (_characterModel.IsMoving)
            {
                RollTime = _characterModel.CharacterCommonSettings.RollTime;
                _characterModel.AnimationSpeed = _characterModel.CharacterCommonSettings.RollAnimationSpeed;
                CanExit = false;
                CanBeOverriden = false;
                LockInputData();
                _animationController.PlayRollAnimation();
            }
            else
            {
                CanExit = true;
                CanBeOverriden = true;
                _stateMachine.ReturnState();
            }
        }

        public override void Execute()
        {
            ExitCheck();
            GetClosestEnemy();
            Roll();
            StayInBattle();
        }

        public override void OnExit()
        {
            _characterModel.AnimationSpeed = _characterModel.CharacterCommonSettings.AnimatorBaseSpeed;
            _characterModel.IsAxisInputsLocked = false;
        }

        private void ExitCheck()
        {
            RollTime -= Time.deltaTime;

            if (RollTime <= 0)
            {
                CanExit = true;
                CanBeOverriden = true;
                _stateMachine.ReturnState();
            }
        }

        private void Roll()
        {
            if (RollTime > 0)
            {
                if (ClosestEnemy != null)
                {
                    Vector3 lookPos = ClosestEnemy.transform.position - _characterModel.CharacterTransform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    _characterModel.CharacterTransform.rotation = rotation;
                }

                _characterModel.CharacterData.Move(_characterModel.CharacterTransform,
                    _characterModel.CharacterCommonSettings.RollFrameDistance, MoveDirection);
            }
        }

        private void GetClosestEnemy()
        {
            Collider enemy = null;

            float minimalDistance = _characterModel.CharacterCommonSettings.SphereColliderRadius;
            float countDistance = minimalDistance;

            foreach (var collider in _characterModel.EnemiesInTrigger)
            {
                countDistance = Mathf.Sqrt((_characterModel.CharacterTransform.position -
                    collider.transform.position).sqrMagnitude);

                if (countDistance < minimalDistance)
                {
                    minimalDistance = countDistance;
                    enemy = collider;
                }
            }

            ClosestEnemy = enemy;
        }

        private void LockInputData()
        {
            _characterModel.IsAxisInputsLocked = true;
            _angleSpeedIncrease = _characterModel.IsMoving ? ANGLE_SPEED_INCREASE : 1;

            MoveDirection = (Vector3.forward * _inputModel.inputStruct._inputTotalAxisY + Vector3.right *
                _inputModel.inputStruct._inputTotalAxisX) / (Mathf.Abs(_inputModel.inputStruct._inputTotalAxisY) +
                    Mathf.Abs(_inputModel.inputStruct._inputTotalAxisX)) * _angleSpeedIncrease;
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}
