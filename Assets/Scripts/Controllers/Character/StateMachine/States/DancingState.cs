namespace BeastHunter
{
    public sealed class DancingState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public DancingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Default;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = false;
            CanBeOverriden = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _animationController.PlayDancingAnimation();
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
