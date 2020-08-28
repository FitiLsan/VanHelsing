namespace BeastHunter
{
    public sealed class DodgingState : CharacterBaseState
    {
        #region ClassLifeCycle

        public DodgingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion
    }
}

