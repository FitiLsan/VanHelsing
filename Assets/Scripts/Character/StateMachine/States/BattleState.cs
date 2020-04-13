using UnityEngine;


namespace BeastHunter
{
    public class BattleState : CharacterBaseState
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

        public BattleState(CharacterInputController inputController, CharacterController characterController, 
            CharacterModel characterModel) : base(inputController, characterController, characterModel)
        {
            CanExit = true;
            SpeedIncreace = _characterModel.CharacterStruct.InBattleRunSpeed / 
                _characterModel.CharacterStruct.InBattleWalkSpeed;
            AnimationSpeed = 1;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _characterController._characterAnimationsController.
                ChangeRuntimeAnimator(_characterModel.CharacterStruct.CharacterBattleAnimator);
            _characterModel.CharacterSphereCollider.radius *= _characterModel.CharacterStruct.SphereColliderRadiusIncrese;
            GetClosestEnemy();
        }

        public override void Execute()
        {
            GetClosestEnemy();
            CountSpeed();
            CheckMovingForward();      
            AnimationControl();
            MovementControl();
        }

        private void CheckMovingForward()
        {
            if (_inputController.InputAxisX != 0 || _inputController.InputAxisY != 0)
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
            if (IsMoving && _characterController.IsGrounded)
            {
                _currentHorizontalInput = _inputController.InputAxisX > 0 ? 1 : _inputController.InputAxisX < 0 ? -1 : 0;
                _currentVerticalInput = _inputController.InputAxisY > 0 ? 1 : _inputController.InputAxisY < 0 ? -1 : 0;

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

        private void AnimationControl()
        {
            _characterController._characterAnimationsController.
                SetBattleAnimation(_currentVerticalInput, _currentHorizontalInput, IsMoving);
            _characterController._characterAnimationsController.SetAnimationsSpeed(AnimationSpeed);
        }

        private void CountSpeed()
        {
            if (IsMoving)
            {
                if (_inputController.InputRun)
                {
                    _targetSpeed = _characterModel.CharacterStruct.InBattleRunSpeed;
                    AnimationSpeed = SpeedIncreace;
                }
                else
                {
                    _targetSpeed = _characterModel.CharacterStruct.InBattleWalkSpeed;
                    AnimationSpeed = 1;
                }
            }
            else
            {
                _targetSpeed = 0;
            }

            if (_characterController.CurrentSpeed < _targetSpeed)
            {
                _speedChangeLag = _characterModel.CharacterStruct.InBattleAccelerationLag;
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterStruct.InBattleDecelerationLag;
            }

            MovementSpeed = Mathf.SmoothDamp(_characterController.CurrentSpeed, _targetSpeed, ref _currentVelocity,
                _speedChangeLag);
        }

        private void GetClosestEnemy()
        {
            Collider enemy = null;

            float minimalDistance = _characterModel.CharacterStruct.SphereColliderRadius;
            float countDistance = minimalDistance;

            foreach (var collider in _characterModel._collidersInTrigger)
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
