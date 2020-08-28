namespace BeastHunter
{
    public sealed class DefaultIdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public DefaultIdleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
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

            _stateMachine.BackState.StopCharacter();
            _animationController.PlayDefaultMovementAnimation(); 
        }

        public override void OnExit()
        {
            base.OnExit();
            _stateMachine.BackState.OnMove = null;
            _stateMachine.BackState.OnSneak = null;
        }

        #endregion
    }
}

