namespace BeastHunter
{
    public sealed class IdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public IdleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.Idle;
            IsTargeting = false;
            IsAttacking = false;
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
            _inputModel.OnMove += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            _inputModel.OnSneakSlide += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Sneaking]);
            _inputModel.OnAttack += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Attacking]);
            _inputModel.OnJump += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Jumping]);
            _inputModel.OnAim += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Battle]);
        }

        protected override void DisableActions()
        {
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

            if(_stateMachine.PreviousState.StateName != CharacterStatesEnum.GettingUp)
            {
                _stateMachine.BackState.StopCharacter();
            }          
        }

        #endregion
    }
}

