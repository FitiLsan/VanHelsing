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
            CanExit = true;
            CanBeOverriden = true;
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            _animationController.PlayDefaultIdleAnimation();
        }

        public override void Execute()
        {

        }

        public override void OnExit()
        {

        }

        public override void OnTearDown()
        {
        }

        #endregion
    }
}

