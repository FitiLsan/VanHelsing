using UnityEngine;


namespace BeastHunter
{
    public sealed class DefaultMovementState : CharacterBaseState
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

        private float TargetDirection { get; set; }
        private float CurrentDirecton { get; set; }
        private float AdditionalDirection { get; set; }
        private float CurrentAngle { get; set; }
        private float MovementSpeed { get; set; }

        #endregion


        #region ClassLifeCycle

        public DefaultMovementState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            CanExit = true;
            CanBeOverriden = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _characterModel.CharacterSphereCollider.radius = _characterModel.CharacterCommonSettings.SphereColliderRadius;
            _animationController.PlayDefaultMovementAnimation();
            _characterModel.CameraCinemachineBrain.m_DefaultBlend.m_Time = 0f;
            _characterModel.CharacterTargetCamera.Priority = 5;
        }

        public override void Execute()
        {
            CountSpeed();
            MovementControl();
        }

        public override void OnExit()
        {
            
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
                TargetDirection = _characterModel.CharacterCamera.transform.localEulerAngles.y + AdditionalDirection;

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
                    _targetSpeed = _characterModel.CharacterCommonSettings.RunSpeed;
                }
                else
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.WalkSpeed;
                }
            }
            else
            {
                _targetSpeed = 0;
            }

            if (_characterModel.CurrentSpeed < _targetSpeed)
            {
               _speedChangeLag = _characterModel.CharacterCommonSettings.AccelerationLag;            
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.DecelerationLag;
            }

            MovementSpeed = Mathf.SmoothDamp(_characterModel.CurrentSpeed, _targetSpeed, ref _currentVelocity, 
                _speedChangeLag);
        }

        #endregion
    }
}


