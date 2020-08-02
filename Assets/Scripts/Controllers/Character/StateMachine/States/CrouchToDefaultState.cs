using UnityEngine;


namespace BeastHunter
{
    public sealed class CrouchToDefaultState : CharacterBaseState, IUpdate
    {
        #region Constants

        private const float EXIT_TIME = 0.5f;

        #endregion


        #region Properties

        private float ExitTime { get; set; }

        #endregion


        #region ClassLifeCycle

        public CrouchToDefaultState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Sneaking;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            ExitTime = EXIT_TIME;
            _animationController.PlayCrouchToDefaultAnimation();
        }

        public void Updating()
        {
            ExitCheck();
            SetCrouchLevel();
        }

        private void SetCrouchLevel()
        {
            _animationController.SetCrouchLevel((EXIT_TIME - ExitTime) / EXIT_TIME);
        }

        private void ExitCheck()
        {
            if (ExitTime > 0)
            {
                ExitTime -= Time.deltaTime;
            }
            else
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
            }
        }

        #endregion
    }
}