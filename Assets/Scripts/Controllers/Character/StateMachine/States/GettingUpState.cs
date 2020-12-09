namespace BeastHunter
{
    public sealed class GettingUpState : CharacterBaseState
    {
        #region ClassLifeCycle

        public GettingUpState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.GettingUp;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return true;
        }

        #endregion
    }
}

