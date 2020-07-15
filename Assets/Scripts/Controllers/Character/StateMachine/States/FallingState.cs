namespace BeastHunter
{
    public sealed class FallingState : CharacterBaseState
    {

        #region ClassLifeCycle

        public FallingState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            Type = StateType.NotActive;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = false;
            CanBeOverriden = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _animationController.PlayFallAnimation();
        }

        public override void Execute()
        {
            ExitCheck();
        }

        public override void OnExit()
        {

        }

        public override void OnTearDown()
        {
        }

        private void ExitCheck()
        {
            if (_characterModel.IsGrounded)
            {
                CanExit = true;
            }
        }

        #endregion
    }
}