using UnityEngine;


namespace BeastHunter
{
    public sealed class MovementState : CharacterBaseState, IUpdate
    {
        #region Properties

        private bool IsStopping { get; set; }
        private float ExitTime { get; set; }

        #endregion


        #region ClassLifeCycle

        public MovementState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
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

        public override bool CanBeActivated()
        {
            return !IsActive;
        }

        protected override void EnableActions()
        {
            base.EnableActions();
            _stateMachine.BackState.OnStop = StartCountExitTime;
            _stateMachine.BackState.OnMove = StopCountExitTime;
            _stateMachine.BackState.OnSneak = () => SneakOrSlide();
            _stateMachine.BackState.OnAttack = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Attacking]);
            _stateMachine.BackState.OnJump = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Jumping]);
            _stateMachine.BackState.OnAim = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Battle]);
            _animationController.PlayMovementAnimation();
        }

        protected override void DisableActions()
        {
            _stateMachine.BackState.OnStop = null;
            _stateMachine.BackState.OnMove = null;
            _stateMachine.BackState.OnSneak = null;
            _stateMachine.BackState.OnAttack = null;
            _stateMachine.BackState.OnJump = null;
            _stateMachine.BackState.OnAim = null;
            base.DisableActions();
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();

            if (_inputModel.IsInputMove)
            {
                StopCountExitTime();
            }
            else
            {
                StartCountExitTime();
            }
        }

        private void ControlMovement()
        {
            _stateMachine.BackState.RotateCharacter(!IsStopping);
            _stateMachine.BackState.MoveCharacter(false);
        }

        private void CountExitTime()
        {
            if (IsStopping)
            {
                ExitTime -= ExitTime > 0 ? Time.deltaTime : 0;

                if (ExitTime <= 0)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Idle]);
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
            ExitTime = _characterModel.CharacterData._characterCommonSettings.TImeToContinueMovingAfterStop;
        }

        private void SneakOrSlide()
        {
            if (_inputModel.IsInputRun)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Sliding]);
            }
            else
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Sneaking]);
            }
        }

        #endregion
    }
}


