﻿using UnityEngine;


namespace BeastHunter
{
    public sealed class RollingState : CharacterBaseState
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
        private float _currentHorizontalInput;
        private float _currentVerticalInput;
        private float _currentAngleVelocity;

        #endregion


        #region Properties

        private Transform CameraTransform { get; set; }
        private float RollTime { get; set; }
        private float TargetDirection { get; set; }
        private float CurrentDirecton { get; set; }
        private float AdditionalDirection { get; set; }
        private float CurrentAngle { get; set; }

        #endregion


        #region ClassLifeCycle

        public RollingState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = false;
            CanBeOverriden = false;
            CameraTransform = Services.SharedInstance.CameraService.CharacterCamera.transform;
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
                _currentHorizontalInput = _inputModel.inputStruct._inputTotalAxisX;
                _currentVerticalInput = _inputModel.inputStruct._inputTotalAxisY;
                _animationController.PlayRollForwardAnimation();
            }
            else
            {
                CheckNextState();
            }
        }

        public override void Execute()
        {
            ExitCheck();
            Roll();
            StayInBattle();
        }

        public override void OnExit()
        {
            _characterModel.AnimationSpeed = _characterModel.CharacterCommonSettings.AnimatorBaseSpeed;
        }

        public override void OnTearDown()
        {
        }

        private void ExitCheck()
        {
            RollTime -= Time.deltaTime;

            if (RollTime <= 0)
            {
                CheckNextState();
            }
        }

        private void CheckNextState()
        {
            CanExit = true;
            CanBeOverriden = true;

            if (NextState == null)
            {
                if (_characterModel.IsMoving)
                {
                    _stateMachine.SetState(_stateMachine._battleMovementState);
                }
                else
                {
                    _stateMachine.SetState(_stateMachine._battleIdleState);
                }             
            }
            else
            {
                _stateMachine.SetState(NextState);
            }
        }

        private void Roll()
        {
            if (RollTime > 0)
            {
                if (_characterModel.IsGrounded)
                {
                    switch (_currentVerticalInput)
                    {
                        case 1:
                            AdditionalDirection = ROLL_LOW_SIDE_ANGLE * _currentHorizontalInput;
                            break;
                        case -1:
                            switch (_currentHorizontalInput)
                            {
                                case 0:
                                    AdditionalDirection = ROLL_BACK_ANGLE;
                                    break;
                                default:
                                    AdditionalDirection = -ROLL_BACK_SIDE_ANGLE * _currentHorizontalInput;
                                    break;
                            }
                            break;
                        case 0:
                            AdditionalDirection = ROLL_HALF_SIDE_ANGLE * _currentHorizontalInput;
                            break;
                        default:
                            break;
                    }

                    CurrentDirecton = _characterModel.CharacterTransform.localEulerAngles.y;
                    TargetDirection = CameraTransform.localEulerAngles.y + AdditionalDirection;

                    CurrentAngle = Mathf.SmoothDampAngle(CurrentDirecton, TargetDirection, ref _currentAngleVelocity,
                        _characterModel.CharacterCommonSettings.DirectionChangeLag);

                    _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0, CurrentAngle, 0);
                    _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, _characterModel.CharacterData.
                        _characterCommonSettings.RollFrameDistance);
                }
            }
        }


        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}
