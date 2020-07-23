using UnityEngine;


namespace BeastHunter
{
    public class TrapPlaceState : CharacterBaseState
    {
        #region Constants

        private const float EXIT_TIME = 1f;

        #endregion


        #region Properties

        private float ExitTime { get; set; }

        #endregion


        #region ClassLifeCycle

        public TrapPlaceState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Default;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = false;
            CanBeOverriden = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            ExitTime = EXIT_TIME;
            CanExit = false;
            _animationController.PlayTrapPlaceAnimation();
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
            if (ExitTime > 0)
            {
                ExitTime -= Time.deltaTime;
            }
            else
            {
                CanExit = true;

                if (_stateMachine.PreviousState == _stateMachine.CharacterStates[CharacterStatesEnum.BattleTargetMovement])
                {
                    _stateMachine.ReturnState();
                }
            }
        }

        #endregion
    }
}

