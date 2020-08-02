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
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            _animationController.PlayCrouchIdleAnimation();
        }

        #endregion
    }
}