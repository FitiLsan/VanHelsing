namespace BeastHunter
{
    public sealed class FallOnGroundState : CharacterBaseState
    {
        #region ClassLifeCycle

        public FallOnGroundState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.NotActive;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion
    }
}

