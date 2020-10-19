namespace BeastHunter
{
    public sealed class IdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public IdleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return !IsActive;
        }

        protected override void EnableActions()
        {
            base.EnableActions();
            _stateMachine.BackState.OnMove = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            _stateMachine.BackState.OnSneak = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Sneaking]);
            _stateMachine.BackState.OnAttack = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Attacking]);
            _stateMachine.BackState.OnJump = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Jumping]);
            _stateMachine.BackState.OnAim = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Battle]);
            _stateMachine.BackState.OnTimeSkipOpenClose = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.TimeSkip]);
        }

        protected override void DisableActions()
        {
            _stateMachine.BackState.OnMove = null;
            _stateMachine.BackState.OnSneak = null;
            _stateMachine.BackState.OnAttack = null;
            _stateMachine.BackState.OnJump = null;
            _stateMachine.BackState.OnAim = null;
            _stateMachine.BackState.OnTimeSkipOpenClose = null;
            base.DisableActions();
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();

            _stateMachine.BackState.StopCharacter();
            _animationController.PlayMovementAnimation(); 
        }

        #endregion
    }
}

