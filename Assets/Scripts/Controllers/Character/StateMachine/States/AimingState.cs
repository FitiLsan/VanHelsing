using UnityEngine;


namespace BeastHunter
{
    public class AimingState : CharacterBaseState, IUpdate
    {
        #region FIelds

        private float _baseAnimationSpeed;
        private float _animationSpeedWhileRun;

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
            _stateMachine.BackState.UpdateAimingDotsForProjectile();
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return _characterModel.CurrentWeaponData.Value.Type == WeaponType.Shooting;
        }

        protected override void EnableActions()
        {
            base.EnableActions();
            _stateMachine.BackState.OnAim = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            _stateMachine.BackState.OnAttack = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Shooting]);
            _stateMachine.BackState.OnStartRun = () => _stateMachine.BackState.SetAnimatorSpeed(_animationSpeedWhileRun);
            _stateMachine.BackState.OnStopRun = () => _stateMachine.BackState.SetAnimatorSpeed(_baseAnimationSpeed);
            _stateMachine.BackState.OnJump = Dodge;
        }

        protected override void DisableActions()
        {
            _stateMachine.BackState.OnAim = null;
            _stateMachine.BackState.OnAttack = null;
            _stateMachine.BackState.OnStartRun = null;
            _stateMachine.BackState.OnStopRun = null;
            _stateMachine.BackState.OnJump = null;
            base.DisableActions();
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();

            if (_inputModel.IsInputRun)
            {
                _stateMachine.BackState.SetAnimatorSpeed(_animationSpeedWhileRun);
            }

            Services.SharedInstance.CameraService.StartDrawAimLine();
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            _stateMachine.BackState.SetAnimatorSpeed(_baseAnimationSpeed);
            Services.SharedInstance.CameraService.StopDrawAimLine();

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
                SetCameraTargetPosition(new Vector3(_inputModel.MouseInputX, _inputModel.MouseInputY, 0), true);
        }

        #endregion
    }
}

