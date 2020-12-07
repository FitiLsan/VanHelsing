namespace BeastHunter
{
    public sealed class KnockedDownState : CharacterBaseState
    {
        #region ClassLifeCycle

        public KnockedDownState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.KnockedDown;
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
