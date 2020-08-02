namespace BeastHunter
{
    public sealed class DancingState : CharacterBaseState
    {
        #region ClassLifeCycle

        public DancingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
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
            _animationController.PlayDancingAnimation();
        }

        #endregion
    }
}
