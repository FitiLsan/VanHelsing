using UnityEngine;


namespace BeastHunter
{
    public class AimingState : CharacterBaseState, IUpdate
    {
        #region FIelds

        private float _baseAnimationSpeed;
        private float _animationSpeedWhileRun;
        private bool _isThrowing;

        #endregion


        #region ClassLifeCycle

        public AimingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.Aiming;
            IsTargeting = false;
            IsAttacking = false;
            _baseAnimationSpeed = _characterModel.CharacterCommonSettings.AnimatorBaseSpeed;
            _animationSpeedWhileRun = _characterModel.CharacterCommonSettings.
                AimRunSpeed / _characterModel.CharacterCommonSettings.AimWalkSpeed;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _stateMachine.BackState.CountSpeed();
            ControlMovement();
            ControlAimingTarget();
            if (_isThrowing) _stateMachine.BackState.UpdateAimingDotsForProjectile();
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return _characterModel.CurrentWeaponData.Value.Type == WeaponType.Shooting ||
                _characterModel.CurrentWeaponData.Value.Type == WeaponType.Throwing;
        }

        protected override void EnableActions()
        {
            base.EnableActions();

            _inputModel.OnAim += () => _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            _inputModel.OnAttack += () => _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Shooting]);
            _inputModel.OnRunStart += () => _stateMachine.BackState.SetAnimatorSpeed(_animationSpeedWhileRun);
            _inputModel.OnRunStop += () => _stateMachine.BackState.SetAnimatorSpeed(_baseAnimationSpeed);
            _inputModel.OnJump += () => Dodge();
        }

        protected override void DisableActions()
        {
            _inputModel.OnAim = null;
            _inputModel.OnAttack = null;
            _inputModel.OnRunStart = null;
            _inputModel.OnRunStop = null;
            _inputModel.OnJump = null;
            base.DisableActions();
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();

            if (_inputModel.IsInputRun)
            {
                _stateMachine.BackState.SetAnimatorSpeed(_animationSpeedWhileRun);
            }
            _isThrowing = _characterModel.CurrentWeaponData.Value.Type == WeaponType.Throwing;
            if(_isThrowing) Services.SharedInstance.CameraService.StartDrawAimLine();
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            _stateMachine.BackState.SetAnimatorSpeed(_baseAnimationSpeed);
            if (_isThrowing)  Services.SharedInstance.CameraService.StopDrawAimLine();

            base.OnExit();
        }

        private void ControlMovement()
        {
            _stateMachine.BackState.RotateCharacterAimimng();
            _stateMachine.BackState.MoveCharacter(true);
        }

        private void Dodge()
        {
            if (_inputModel.IsInputMove)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Dodging]);
            }
        }

        private void ControlAimingTarget()
        {
            Services.SharedInstance.CameraService.
                SetCameraTargetPosition(new Vector3(_inputModel.MouseInputX, -_inputModel.MouseInputY, 0), true);
        }

        #endregion
    }
}

