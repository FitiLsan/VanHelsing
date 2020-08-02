namespace BeastHunter
{
    public sealed class DeadState : CharacterBaseState
    {
        #region ClassLifeCycle

        public DeadState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.NotActive;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            _characterModel.IsDead = true;
            _animationController.PlayDeadAnimation();
            _characterModel.CharacterTransform.tag = TagManager.NPC;
            GlobalEventsModel.OnPlayerDie?.Invoke();
        }

        #endregion
    }
}

