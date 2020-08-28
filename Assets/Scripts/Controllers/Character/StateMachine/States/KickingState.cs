namespace BeastHunter
{
    public sealed class KickingState : CharacterBaseState
    {
        #region ClassLifeCycle

        public KickingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion
    }
}

