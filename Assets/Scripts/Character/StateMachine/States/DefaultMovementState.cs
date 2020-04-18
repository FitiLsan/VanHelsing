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
        private const float EXIT_TIME = 1f;

        #endregion


        #region Fields

        private float _currentHorizontalInput;
        private float _currentVerticalInput;
        private float _currentAngleVelocity;
        private float _targetSpeed;
        private float _speedChangeLag;
        private float _currentVelocity;
        private float _currentExitTime;

        #endregion


        #region Properties

        private bool IsMovingForward { get; set; }
        private float TargetDirection { get; set; }
        private float CurrentDirecton { get; set; }
        private float AdditionalDirection { get; set; }
        private float CurrentAngle { get; set; }
        private float MovementSpeed { get; set; }

        #endregion


        #region ClassLifeCycle

        public DefaultMovementState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
        {
            CanExit = false;
            _currentExitTime = EXIT_TIME;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _characterModel.CharacterSphereCollider.radius = _characterModel.CharacterCommonSettings.SphereColliderRadius;
        }

        public override void Execute()
        {
            ExitCheck();
            CountSpeed();
            CheckMovingForward();
            MovementControl();
        }

        private void ExitCheck()
        {
            if (!IsMovingForward)
            {
                _currentExitTime -= Time.deltaTime;

                if(_currentExitTime <= 0)
                {
                    CanExit = true;
                }
            }
            else
            {
                CanExit = false;
                _currentExitTime = EXIT_TIME;
            }
        }

        private void CheckMovingForward()
        {
            if(_inputModel.inputStruct._inputAxisX != 0 || _inputModel.inputStruct._inputAxisY != 0)
            {
                IsMovingForward = true;
            }
            else
            {
                IsMovingForward = false;
            }
        }

        private void MovementControl()
        {
            if (IsMovingForward && _characterModel.IsGrounded)
            {
                _currentHorizontalInput = _inputModel.inputStruct._inputAxisX > 0 ? 1 : _inputModel.inputStruct._inputAxisX < 0 ? -1 : 0;
                _currentVerticalInput = _inputModel.inputStruct._inputAxisY > 0 ? 1 : _inputModel.inputStruct._inputAxisY < 0 ? -1 : 0;

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
            if (IsMovingForward)
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


