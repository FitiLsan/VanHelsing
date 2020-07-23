namespace BeastHunter
{
    public sealed class CrouchIdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public CrouchIdleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Sneaking;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = true;
            CanBeOverriden = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _animationController.PlayCrouchIdleAnimation();
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