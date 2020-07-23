using UnityEngine;


namespace BeastHunter
{
    public class CrouchToDefaultState : CharacterBaseState
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
            CanExit = false;
            CanBeOverriden = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            ExitTime = EXIT_TIME;
            CanExit = false;
            _animationController.PlayCrouchToDefaultAnimation();
        }

        public override void Execute()
        {
            ExitCheck();
            SetCrouchLevel();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
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
                CanExit = true;
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
            }
        }

        #endregion
    }
}