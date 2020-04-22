using UnityEngine;


namespace BeastHunter
{
    public class FallingState : CharacterBaseState
    {

        #region ClassLifeCycle

        public FallingState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
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