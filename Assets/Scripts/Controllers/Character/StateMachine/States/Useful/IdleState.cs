namespace BeastHunter
{
    public sealed class IdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public IdleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Default;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            _stateMachine.BackState.OnMove = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultMovement]);
            _stateMachine.BackState.OnSneak = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Sneaking]);
            _stateMachine.BackState.OnAttack = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Attacking]);

            _stateMachine.BackState.StopCharacter();
            _animationController.PlayIdleAnimation(); 
        }

        public override void OnExit()
        {
            base.OnExit();
            _stateMachine.BackState.OnMove = null;
            _stateMachine.BackState.OnSneak = null;
            _stateMachine.BackState.OnAttack = null;
        }

        #endregion
    }
}

