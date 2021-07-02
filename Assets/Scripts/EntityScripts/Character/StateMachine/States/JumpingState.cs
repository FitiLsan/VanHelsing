﻿using UnityEngine;


namespace BeastHunter
{
    public sealed class JumpingState : CharacterBaseState
    {
        #region Constants

        private const float NOT_CHECK_GROUND_TIME = 0.2f;
        private const float EXIT_TIME = 1f;
        private const float SPEED_FORCE_COMPENSATOR = 0.5f;

        #endregion


        #region Fields

        private Vector3 _jumpVector;
        private Vector3 _previousPosition;
        private Vector3 _currentPosition;
        private float _currentGroundCheckTime;
        private float _currentExitTime;
        private float _speedBeforeJump;
        
        #endregion


        #region Properties

        private float JumpVerticalForce { get; set; }
        private float JumpHorizontalForce { get; set; }

        #endregion


        #region ClassLifeCycle

        public JumpingState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            Type = StateType.Default;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = false;
            CanBeOverriden = true;
            JumpVerticalForce = _characterModel.CharacterCommonSettings.JumpVerticalForce;
            JumpHorizontalForce = _characterModel.CharacterCommonSettings.JumpHorizontalForce;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            CanExit = false;
            _speedBeforeJump = _characterModel.CurrentSpeed;
            _currentGroundCheckTime = NOT_CHECK_GROUND_TIME;
            _currentExitTime = EXIT_TIME;
            _animationController.PlayJumpAnimation();
            UpdatePosition();
        }

        public override void Execute()
        {
            ExitCheck();
            Jumping();
        }

        public override void OnExit()
        {

        }

        public override void OnTearDown()
        {
        }

        private void ExitCheck()
        {
            _currentGroundCheckTime -= Time.deltaTime;
            _currentExitTime -= Time.deltaTime;

            if (_currentGroundCheckTime < 0 && (_characterModel.IsGrounded || _currentExitTime <= 0))
            {
                CanExit = true;
            }
        }

        public void UpdatePosition()
        {
            _previousPosition = _currentPosition;
            _currentPosition = _characterModel.CharacterTransform.position;
        }

        public void Jumping()
        {
            UpdatePosition();

            if(_currentPosition.y >= _previousPosition.y)
            {
                _jumpVector = _characterModel.CharacterTransform.position + (Vector3.up * 
                    JumpVerticalForce * _currentExitTime / EXIT_TIME + _characterModel.CharacterTransform.forward *
                        (JumpHorizontalForce + _speedBeforeJump * SPEED_FORCE_COMPENSATOR)) * Time.smoothDeltaTime;
                _characterModel.VerticalSpeed = 1;
            }
            else
            {
                _jumpVector = _characterModel.CharacterTransform.position + _characterModel.CharacterTransform.forward * 
                    (JumpHorizontalForce + _speedBeforeJump * SPEED_FORCE_COMPENSATOR) * Time.smoothDeltaTime;
            }

            _characterModel.CharacterTransform.position = Vector3.Lerp(_characterModel.CharacterTransform.position, 
                _jumpVector, 1);
        }

        #endregion
    }
}

