using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterInputController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;
        private CharacterModel _characterModel;
        private CharacterAnimationsController _characterAnimationsController;
        private PhysicsService _physics;
        private Vector2 _inputAxis;
        private Vector3 _groundHit;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;
        private float _additionalGravity;
        private float _currentHorizontalInput;
        private float _currentVerticalInput;
        private float _currentSpeed;
        private float _targetSpeed;
        private float _speedCountTime;
        private float _speedCountFrame;
        private float _speedChangeLag;
        private float _currentVelocity;
        private float _currentAngleVelocity;

        #endregion


        #region Properties

        public float TargetDirection { get; private set; }
        public float CurrentDirecton { get; private set; }
        public float AdditionalDirection { get; private set; }
        public float CurrentAngle { get; private set; }
        public float MovementSpeed { get; private set; }
        public float InBatleSpeedReduction { get; private set; }

        public bool IsNotMoving { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsInBattleMode { get; private set; }
        public bool IsMovingForward { get; private set; }
        public bool IsTurningRight { get; private set; }
        public bool IsTurningLeft { get; private set; }
        public bool IsFocusingObject { get; private set; }

        #endregion


        #region ClassLifeCycle

        public CharacterInputController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            Cursor.visible = false;
            IsNotMoving = true;
            IsRunning = false;
            IsMovingForward = false;
            IsTurningRight = false;
            IsTurningLeft = false;

            _characterModel = _context._characterModel;
            _characterAnimationsController =
                new CharacterAnimationsController(_characterModel.CharacterAnimator);
            _physics = new PhysicsService(_context);

            _speedCountFrame = _characterModel.CharacterStruct.SpeedMeasureFrame;
            _lastPosition = _characterModel.CharacterTransform.position;
            _currentPosition = _lastPosition;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            GroundCheck();
            GetInput();
            Move();
            CameraControl();

            _characterAnimationsController.UpdateAnimation(IsGrounded, IsRunning, IsInBattleMode, IsMovingForward,
                IsTurningRight, IsTurningLeft);
        }

        #endregion


        #region Methods

        private void GetInput()
        {
            _inputAxis.x = Input.GetAxis("Horizontal");
            _inputAxis.y = Input.GetAxis("Vertical");

            if (Input.GetButtonDown("Target"))
            {
                BattleModeSwitch();
            }

            if (Input.GetButton("Sprint"))
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        private void Move()
        {
            CountSpeed();

            if (_inputAxis.y != 0 || _inputAxis.x != 0)
            {              

                IsNotMoving = false;
                IsMovingForward = true;

                _currentHorizontalInput = _inputAxis.x > 0 ? 1 : _inputAxis.x < 0 ? -1 : 0;
                _currentVerticalInput = _inputAxis.y > 0 ? 1 : _inputAxis.y < 0 ? -1 : 0;

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

                _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform,
                    MovementSpeed * (1 - InBatleSpeedReduction));

            }
            else
            {
                IsNotMoving = true;
                IsMovingForward = false;
                IsRunning = false;
            }
        }

        private void CameraControl()
        {
            if (_inputAxis.y != 0 || _inputAxis.x != 0)
            {
                CurrentDirecton = _characterModel.CharacterTransform.localEulerAngles.y;
                TargetDirection = _characterModel.CharacterCamera.transform.localEulerAngles.y +
                    AdditionalDirection;
                CurrentAngle = Mathf.SmoothDampAngle(CurrentDirecton, TargetDirection, ref _currentAngleVelocity,
                    _characterModel.CharacterStruct.DirectionChangeLag);
                _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0, CurrentAngle, 0);
            }
        }

        private void Jump()
        {
            if (IsGrounded)
            {
                _characterModel.CharacterData.Jump(_characterModel.CharacterRigitbody,
                     _characterModel.CharacterStruct.JumpForce);
            }
        }

        private void BattleModeSwitch()
        {
            IsInBattleMode = !IsInBattleMode;

            if (IsInBattleMode)
            {
                InBatleSpeedReduction = _characterModel.CharacterStruct.InBatlleSpeedReduction;
            }
            else
            {
                InBatleSpeedReduction = 0;
            }
        }

        private void GroundCheck()
        {
            IsGrounded = _physics.CheckGround(_characterModel.CharacterCapsuleCollider.transform.position + Vector3.up,
                _characterModel.CharacterStruct.GroundCheckHeight, out _groundHit);

            if (_characterModel.CharacterRigitbody.velocity.y < 0)
            {
                _characterModel.CharacterRigitbody.velocity += Vector3.up * Physics.gravity.y *
                    _characterModel.CharacterStruct.FallingForce * Time.deltaTime;
            }
            else if (_characterModel.CharacterRigitbody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                _characterModel.CharacterRigitbody.velocity += Vector3.up * Physics.gravity.y *
                    _characterModel.CharacterStruct.LongJumpFallingForce * Time.deltaTime;
            }
        }

        private void CountSpeed()
        {
            if (_speedCountTime > 0)
            {
                _speedCountTime -= Time.fixedDeltaTime;
            }
            else
            {
                _speedCountTime = _speedCountFrame;
                _currentPosition = _characterModel.CharacterTransform.position;
                _currentSpeed = (_currentPosition - _lastPosition).magnitude / _speedCountFrame;
                _lastPosition = _currentPosition;
            }

            if (IsMovingForward)
            {
                if (IsRunning)
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

            if(_currentSpeed < _targetSpeed)
            {
                _speedChangeLag = _characterModel.CharacterStruct.AccelerationLag;
                _characterAnimationsController.SetAnimationsSpeed(Mathf.Sqrt(_currentSpeed / _targetSpeed));
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterStruct.DecelerationLag;
                _characterAnimationsController.SetAnimationsBaseSpeed();
            }

            MovementSpeed = Mathf.SmoothDamp(_currentSpeed, _targetSpeed, ref _currentVelocity, _speedChangeLag);
        }

        #endregion
    }
}

