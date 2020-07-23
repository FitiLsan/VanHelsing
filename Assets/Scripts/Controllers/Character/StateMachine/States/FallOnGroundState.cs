namespace BeastHunter
{
    public sealed class FallOnGroundState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public FallOnGroundState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.NotActive;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = false;
            CanBeOverriden = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {

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

