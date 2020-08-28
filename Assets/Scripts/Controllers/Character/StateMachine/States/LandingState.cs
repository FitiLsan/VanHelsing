namespace BeastHunter
{
    public sealed class LandingState : CharacterBaseState
    {
        #region ClassLifeCycle

        public LandingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.NotActive;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion
    }
}
