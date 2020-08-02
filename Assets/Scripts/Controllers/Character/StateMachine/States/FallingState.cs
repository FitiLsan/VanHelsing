namespace BeastHunter
{
    public sealed class FallingState : CharacterBaseState, IUpdate
    {

        #region ClassLifeCycle

        public FallingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.NotActive;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _animationController.PlayFallAnimation();
        }

        public void Updating()
        {
            ExitCheck();
        }

        private void ExitCheck()
        {
            if (_characterModel.IsGrounded)
            {
            }
        }

        #endregion
    }
}