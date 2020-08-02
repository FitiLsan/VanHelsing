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
            _animationController.PlayDefaultIdleAnimation();
        }

        #endregion
    }
}

