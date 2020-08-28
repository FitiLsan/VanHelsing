﻿using UnityEngine;


namespace BeastHunter
{
    public sealed class BattleMovementState : CharacterBaseState
    {
        #region Constants

        private const float CAMERA_LOW_SIDE_ANGLE = 45f;
        private const float CAMERA_HALF_SIDE_ANGLE = 90f;
        private const float CAMERA_BACK_SIDE_ANGLE = 225f;
        private const float CAMERA_BACK_ANGLE = 180f;

        #endregion


        #region Fields

        private float _currentHorizontalInput;
        private float _currentVerticalInput;
        private float _currentAngleVelocity;
        private float _targetSpeed;
        private float _speedChangeLag;
        private float _currentVelocity;

        #endregion


        #region Properties

        private Transform CameraTransform { get; set; }
        private float TargetDirection { get; set; }
        private float CurrentDirecton { get; set; }
        private float AdditionalDirection { get; set; }
        private float CurrentAngle { get; set; }
        private float MovementSpeed { get; set; }
        private float SpeedIncreace { get; set; }

        #endregion


        #region ClassLifeCycle

        public BattleMovementState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = true;
            CanBeOverriden = true;
            CameraTransform = Services.SharedInstance.CameraService.CharacterCamera.transform;
            SpeedIncreace = _characterModel.CharacterCommonSettings.InBattleRunSpeed / 
                _characterModel.CharacterCommonSettings.InBattleWalkSpeed;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _animationController.PlayBattleMovementAnimation(_characterModel.LeftHandWeapon, _characterModel.RightHandWeapon);
        }

        public override void Execute()
        {
            CountSpeed();   
            MovementControl();
            StayInBattle();
        }

        public override void OnExit()
        {
            _characterModel.AnimationSpeed = _characterModel.CharacterCommonSettings.AnimatorBaseSpeed;
        }

        public override void OnTearDown()
        {
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        private void MovementControl()
        {
            if (_characterModel.IsGrounded)
            {
                _currentHorizontalInput = _inputModel.inputStruct._inputTotalAxisX;
                _currentVerticalInput = _inputModel.inputStruct._inputTotalAxisY;

                switch (_currentVerticalInput)
                {
                    case 1:
                        AdditionalDirection = CAMERA_LOW_SIDE_ANGLE * _currentHorizontalInput;
                        break;
                    case -1:
                        switch (_currentHorizontalInput)
                        {
                            case 0:
                                AdditionalDirection = CAMERA_BACK_ANGLE;
                                break;
                            default:
                                AdditionalDirection = -CAMERA_BACK_SIDE_ANGLE * _currentHorizontalInput;
                                break;
                        }
                        break;
                    case 0:
                        AdditionalDirection = CAMERA_HALF_SIDE_ANGLE * _currentHorizontalInput;
                        break;
                    default:
                        break;
                }

                CurrentDirecton = _characterModel.CharacterTransform.localEulerAngles.y;
                TargetDirection = CameraTransform.localEulerAngles.y + AdditionalDirection;

                CurrentAngle = Mathf.SmoothDampAngle(CurrentDirecton, TargetDirection, ref _currentAngleVelocity,
                    _characterModel.CharacterCommonSettings.DirectionChangeLag);

                _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0, CurrentAngle, 0);
                _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, MovementSpeed);
            }
        }

        private void CountSpeed()
        {
            if (_characterModel.IsMoving)
            {
                if (_inputModel.inputStruct._isInputRun)
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.InBattleRunSpeed;
                    _characterModel.AnimationSpeed = SpeedIncreace;
                }
                else
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.InBattleWalkSpeed;
                    _characterModel.AnimationSpeed = 1;
                }
            }
            else
            {
                _targetSpeed = 0;
            }

            if (_characterModel.CurrentSpeed < _targetSpeed)
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.InBattleAccelerationLag;
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.InBattleDecelerationLag;
            }

            MovementSpeed = Mathf.SmoothDamp(_characterModel.CurrentSpeed, _targetSpeed, ref _currentVelocity,
                _speedChangeLag);
        }

        #endregion
    }
}
