using UnityEngine;


namespace BeastHunter
{
    public class BattleMovementState : CharacterBaseState
    {
        #region Constants

        private const float CAMERA_LOW_SIDE_ANGLE = 45f;
        private const float CAMERA_HALF_SIDE_ANGLE = 90f;
        private const float CAMERA_BACK_SIDE_ANGLE = 225f;
        private const float CAMERA_BACK_ANGLE = 180f;
        private const float ANGLE_SPEED_INCREACE = 1.225f;

        #endregion


        #region Fields

        private float _currentHorizontalInput;
        private float _currentVerticalInput;
        private float _currentAngleVelocity;
        private float _targetSpeed;
        private float _speedChangeLag;
        private float _currentVelocity;
        private float _angleSpeedIncrease;

        #endregion


        #region Properties

        public Vector3 MoveDirection { get; private set; }
        public Collider ClosestEnemy { get; private set; }

        public bool IsMoving { get; private set; }
        private float TargetDirection { get; set; }
        private float CurrentDirecton { get; set; }
        private float AdditionalDirection { get; set; }
        private float CurrentAngle { get; set; }
        public float MovementSpeed { get; private set; }
        public float SpeedIncreace { get; private set; }

        #endregion


        #region ClassLifeCycle

        public BattleMovementState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
        {
            CanExit = true;
            CanBeOverriden = true;
            SpeedIncreace = _characterModel.CharacterCommonSettings.InBattleRunSpeed / 
                _characterModel.CharacterCommonSettings.InBattleWalkSpeed;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _characterModel.CharacterSphereCollider.radius *= _characterModel.CharacterCommonSettings.SphereColliderRadiusIncrese;
            GetClosestEnemy();
        }

        public override void Execute()
        {
            GetClosestEnemy();
            CountSpeed();
            CheckMovingForward();      
            MovementControl();
            StayInBattle();
        }

        public override void OnExit()
        {
            _characterModel.AnimationSpeed = 1f;
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        private void CheckMovingForward()
        {
            if (_inputModel.inputStruct._inputAxisX != 0 || _inputModel.inputStruct._inputAxisY != 0)
            {
                IsMoving = true;
            }
            else
            {
                IsMoving = false;
            }
        }

        private void MovementControl()
        {
            if (_characterModel.IsGrounded)
            {
                _currentHorizontalInput = _inputModel.inputStruct._inputAxisX > 0 ? 1 : _inputModel.inputStruct._inputAxisX < 0 ? -1 : 0;
                _currentVerticalInput = _inputModel.inputStruct._inputAxisY > 0 ? 1 : _inputModel.inputStruct._inputAxisY < 0 ? -1 : 0;
                _angleSpeedIncrease = (_currentHorizontalInput != 0 && _currentVerticalInput != 0) ? ANGLE_SPEED_INCREACE : 1;

                if (_characterModel.IsTargeting)
                {
                    MoveDirection = (Vector3.forward * _currentVerticalInput + Vector3.right * _currentHorizontalInput) /
                        (Mathf.Abs(_currentVerticalInput) + Mathf.Abs(_currentHorizontalInput)) * _angleSpeedIncrease;

                    if (ClosestEnemy != null)
                    {
                        Vector3 lookPos = ClosestEnemy.transform.position - _characterModel.CharacterTransform.position;
                        lookPos.y = 0;
                        Quaternion rotation = Quaternion.LookRotation(lookPos);
                        _characterModel.CharacterTransform.rotation = rotation;
                    }

                    _characterModel.CharacterData.Move(_characterModel.CharacterTransform, MovementSpeed, MoveDirection);
                }
                else
                {
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
            else
            {
                _currentHorizontalInput = 0;
                _currentVerticalInput = 0;
            }
        }

        private void CountSpeed()
        {
            if (IsMoving)
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

        #endregion
    }
}
