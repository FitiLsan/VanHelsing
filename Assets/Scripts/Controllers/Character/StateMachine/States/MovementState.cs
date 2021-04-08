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
            StateName = CharacterStatesEnum.Movement;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _stateMachine.BackState.CountSpeed();
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
            _inputModel.OnStop += StartCountExitTime;
            _inputModel.OnMove += StopCountExitTime;
            _inputModel.OnSneakSlide += () => SneakOrSlide();
            _inputModel.OnAttack += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Attacking]);
            _inputModel.OnJump += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Jumping]);
            _inputModel.OnAim += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Battle]);
        }

        protected override void DisableActions()
        {
            _inputModel.OnStop = null;
            _inputModel.OnMove = null;
            _inputModel.OnSneakSlide = null;
            _inputModel.OnAttack = null;
            _inputModel.OnJump = null;
            _inputModel.OnAim = null;
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
            ExitTime = _characterModel.CharacterData.CharacterCommonSettings.TImeToContinueMovingAfterStop;
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


