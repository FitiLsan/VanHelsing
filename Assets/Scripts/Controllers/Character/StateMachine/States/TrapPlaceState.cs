using UnityEngine;


namespace BeastHunter
{
    public sealed class TrapPlaceState : CharacterBaseState, IUpdate
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
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            ExitTime = EXIT_TIME;
            _animationController.PlayTrapPlaceAnimation();
        }

        public void Updating()
        {
            ExitCheck();
        }

        private void ExitCheck()
        {
            if (ExitTime > 0)
            {
                ExitTime -= Time.deltaTime;
            }
            else
            {
                if (_stateMachine.PreviousState == _stateMachine.CharacterStates[CharacterStatesEnum.BattleTargetMovement])
                {
                    _stateMachine.ReturnState();
                }
            }
        }

        #endregion
    }
}

