using UnityEngine;


namespace BeastHunter
{
    public class BattleMovementState : CharacterBaseState
    {
        #region Fields

        private float _currentHorizontalInput;
        private float _currentVerticalInput;
        private float _targetSpeed;
        private float _speedChangeLag;
        private float _currentVelocity;

        #endregion


        #region Properties

        public Vector3 MoveDirection { get; private set; }
        public Collider ClosestEnemy { get; private set; }

        public bool IsMoving { get; private set; }
        public float MovementSpeed { get; private set; }
        public float SpeedIncreace { get; private set; }
        public float AnimationSpeed { get; private set; }

        #endregion


        #region ClassLifeCycle

        public BattleMovementState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
        {
            CanExit = true;
            SpeedIncreace = _characterModel.CharacterCommonSettings.InBattleRunSpeed / 
                _characterModel.CharacterCommonSettings.InBattleWalkSpeed;
            AnimationSpeed = 1;
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
            if (IsMoving && _characterModel.IsGrounded)
            {
                _currentHorizontalInput = _inputModel.inputStruct._inputAxisX > 0 ? 1 : _inputModel.inputStruct._inputAxisX < 0 ? -1 : 0;
                _currentVerticalInput = _inputModel.inputStruct._inputAxisY > 0 ? 1 : _inputModel.inputStruct._inputAxisY < 0 ? -1 : 0;

                MoveDirection = (Vector3.forward * _currentVerticalInput + Vector3.right * _currentHorizontalInput)/
                    (Mathf.Abs(_currentVerticalInput) + Mathf.Abs(_currentHorizontalInput));

                if(ClosestEnemy != null)
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
                    AnimationSpeed = SpeedIncreace;
                }
                else
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.InBattleWalkSpeed;
                    AnimationSpeed = 1;
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
