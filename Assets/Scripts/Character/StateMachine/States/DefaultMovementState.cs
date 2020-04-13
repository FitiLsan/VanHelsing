using UnityEngine;


namespace BeastHunter
{
    public sealed class DefaultMovementState : CharacterBaseState
    {
        #region Fields

        private float _currentHorizontalInput;
        private float _currentVerticalInput;
        private float _currentAngleVelocity;
        private float _targetSpeed;
        private float _speedChangeLag;
        private float _currentVelocity;

        #endregion


        #region Properties

        private bool IsMovingForward { get; set; }
        public float TargetDirection { get; private set; }
        public float CurrentDirecton { get; private set; }
        public float AdditionalDirection { get; private set; }
        public float CurrentAngle { get; private set; }
        public float MovementSpeed { get; private set; }

        #endregion


        #region ClassLifeCycle

        public DefaultMovementState(CharacterInputController inputController, CharacterController characterController,
            CharacterModel characterModel) : base(inputController, characterController, characterModel)
        {
            CanExit = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _characterController._characterAnimationsController.
                ChangeRuntimeAnimator(_characterModel.CharacterStruct.CharacterDefaultMovementAnimator);
            _characterModel.CharacterSphereCollider.radius = _characterModel.CharacterStruct.SphereColliderRadius;
            _characterController._characterAnimationsController.SetAnimationsBaseSpeed();
        }

        public override void Execute()
        {
            CountSpeed();
            CheckMovingForward();
            MovementControl();
            AnimationControl();
        }

        private void CheckMovingForward()
        {
            if(_inputController.InputAxisX != 0 || _inputController.InputAxisY != 0)
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
            if (IsMovingForward && _characterController.IsGrounded)
            {
                _currentHorizontalInput = _inputController.InputAxisX > 0 ? 1 : _inputController.InputAxisX < 0 ? -1 : 0;
                _currentVerticalInput = _inputController.InputAxisY > 0 ? 1 : _inputController.InputAxisY < 0 ? -1 : 0;

                switch (_currentVerticalInput)
                {
                    case 1:
                        AdditionalDirection = _characterModel.CharacterStruct.CameraLowSideAngle *
                            _currentHorizontalInput;
                        break;
                    case -1:
                        switch (_currentHorizontalInput)
                        {
                            case 0:
                                AdditionalDirection = _characterModel.CharacterStruct.CameraBackAngle;
                                break;
                            default:
                                AdditionalDirection = -_characterModel.CharacterStruct.CameraBackSideAngle *
                                    _currentHorizontalInput;
                                break;
                        }
                        break;
                    case 0:
                        AdditionalDirection = _characterModel.CharacterStruct.CameraHalfSideAngle *
                            _currentHorizontalInput;
                        break;
                    default:
                        break;
                }

                CurrentDirecton = _characterModel.CharacterTransform.localEulerAngles.y;
                TargetDirection = _characterModel.CharacterCamera.transform.localEulerAngles.y + AdditionalDirection;

                CurrentAngle = Mathf.SmoothDampAngle(CurrentDirecton, TargetDirection, ref _currentAngleVelocity,
                    _characterModel.CharacterStruct.DirectionChangeLag);

                _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0, CurrentAngle, 0);
                _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, MovementSpeed);
            }
        }

        private void AnimationControl()
        {
            _characterController._characterAnimationsController.
                SetDefaultMovementAnimation(IsMovingForward, _characterController.IsGrounded, MovementSpeed);
        }

        private void CountSpeed()
        {
            if (IsMovingForward)
            {
                if (_inputController.InputRun)
                {
                    _targetSpeed = _characterModel.CharacterStruct.RunSpeed;
                }
                else
                {
                    _targetSpeed = _characterModel.CharacterStruct.WalkSpeed;
                }
            }
            else
            {
                _targetSpeed = 0;
            }

            if (_characterController.CurrentSpeed < _targetSpeed)
            {
               _speedChangeLag = _characterModel.CharacterStruct.AccelerationLag;            
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterStruct.DecelerationLag;
            }

            MovementSpeed = Mathf.SmoothDamp(_characterController.CurrentSpeed, _targetSpeed, ref _currentVelocity, 
                _speedChangeLag);
        }

        #endregion
    }
}


