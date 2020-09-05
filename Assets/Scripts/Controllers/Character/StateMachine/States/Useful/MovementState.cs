using UnityEngine;


namespace BeastHunter
{
    public sealed class MovementState : CharacterBaseState, IUpdate
    {
        #region Constants

        private const float EXIT_TIME = 1f;

        #endregion


        #region Properties

        private bool IsStopping { get; set; }
        private float ExitTime { get; set; }

        #endregion


        #region ClassLifeCycle

        public MovementState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Default;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _stateMachine.BackState.CountSpeedDefault();
            ControlMovement();
            CountExitTime();
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            StopCountExitTime();

            _stateMachine.BackState.OnStop = StartCountExitTime;
            _stateMachine.BackState.OnMove = StopCountExitTime;
            _stateMachine.BackState.OnSneak = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Sneaking]);
            _stateMachine.BackState.OnAttack = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Attacking]);
            _animationController.PlayMovementAnimation();
        }

        public override void OnExit()
        {
            base.OnExit();
            _stateMachine.BackState.OnStop = null;
            _stateMachine.BackState.OnMove = null;
            _stateMachine.BackState.OnSneak = null;
            _stateMachine.BackState.OnAttack = null;
        }

        private void ControlMovement()
        {
            _stateMachine.BackState.RotateCharacter(!IsStopping);
            _stateMachine.BackState.MoveCharacter();
        }

        private void CountExitTime()
        {
            if (IsStopping)
            {
                ExitTime -= ExitTime > 0 ? Time.deltaTime : 0;

                if (ExitTime <= 0)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
                }
            }
        }           

        private void StartCountExitTime()
        {
            IsStopping = true;
        }

        private void StopCountExitTime()
        {
            IsStopping = false;
            ExitTime = EXIT_TIME;
        }

        #endregion
    }
}


