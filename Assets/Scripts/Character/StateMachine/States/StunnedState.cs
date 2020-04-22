using UnityEngine;


namespace BeastHunter
{
    public class StunnedState : CharacterBaseState
    {
        #region Constants

        private const float EXIT_TIME = 1f;

        #endregion


        #region Properties

        private float ExitTime { get; set; }

        #endregion


        #region ClassLifeCycle

        public StunnedState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            CanExit = false;
            CanBeOverriden = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            ExitTime = EXIT_TIME;
            CanExit = false;
            _animationController.PlayStunnedAnimation();
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
            if(ExitTime > 0)
            {
                ExitTime -= Time.deltaTime;
            }
            else
            {
                CanExit = true;

                if (_stateMachine.PreviousState == _stateMachine._battleTargetMovementState)
                {
                    _stateMachine.ReturnState();
                }
            }
        }

        #endregion
    }
}