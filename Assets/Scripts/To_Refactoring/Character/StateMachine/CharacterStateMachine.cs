using System;


namespace BeastHunter
{
    public sealed class CharacterStateMachine
    {
        #region Fields

        public DefaultIdleState _defaultIdleState;
        public DefaultMovementState _defaultMovementState;
        public JumpingState _jumpingState;
        public FallingState _fallingState;
        public LandingState _landingState;
        public FallOnGroundState _fallOnGroundState;
        public BattleIdleState _battleIdleState;
        public BattleMovementState _battleMovementState;
        public BattleTargetMovementState _battleTargetMovementState;
        public AttackingFromLeftState _attackingLeftState;
        public AttackingFromRightState _attackingRightState;
        public DodgingState _dodgingState;
        public StunnedState _stunnedState;
        public DeadState _deadState;
        public RollingState _rollingState;
        public RollingTargetState _rollingTargetState;
        public TalkingState _talkingState;
        public DancingState _dancingState;
        public GettingWeaponState _gettingWeaponState;
        public RemovingWeaponState _removingWeaponState;

        #endregion


        #region Properties

        public CharacterBaseState CurrentState { get; private set; }
        public CharacterBaseState PreviousState { get; private set; }
        public Action<CharacterBaseState, CharacterBaseState> OnStateChangeHandler { get; set; }
        private InputModel _inputModel { get; set; }
        private CharacterModel _characterModel { get; set; }
        private CharacterAnimationController _animationController { get; set; }

        #endregion


        #region ClassLifeCycle

        public CharacterStateMachine(InputModel inputModel, CharacterModel characterModel, CharacterAnimationController animationController)
        {
            _inputModel = inputModel;
            _characterModel = characterModel;
            _animationController = animationController;
            PreviousState = null;
            CurrentState = null;

            _defaultIdleState = new DefaultIdleState(_characterModel, _inputModel, _animationController, this);
            _battleIdleState = new BattleIdleState(_characterModel, _inputModel, _animationController, this);
            _defaultMovementState = new DefaultMovementState(_characterModel, _inputModel, _animationController, this);
            _battleMovementState = new BattleMovementState(_characterModel, _inputModel, _animationController, this);
            _battleTargetMovementState = new BattleTargetMovementState(_characterModel, _inputModel, _animationController, this);
            _attackingLeftState = new AttackingFromLeftState(_characterModel, _inputModel, _animationController, this);
            _attackingRightState = new AttackingFromRightState(_characterModel, _inputModel, _animationController, this);
            _jumpingState = new JumpingState(_characterModel, _inputModel, _animationController, this);
            _dodgingState = new DodgingState(_characterModel, _inputModel, _animationController, this);
            _fallingState = new FallingState(_characterModel, _inputModel, _animationController, this);
            _dancingState = new DancingState(_characterModel, _inputModel, _animationController, this);
            _rollingState = new RollingState(_characterModel, _inputModel, _animationController, this);
            _rollingTargetState = new RollingTargetState(_characterModel, _inputModel, _animationController, this);
            _landingState = new LandingState(_characterModel, _inputModel, _animationController, this);
            _fallOnGroundState = new FallOnGroundState(_characterModel, _inputModel, _animationController, this);
            _stunnedState = new StunnedState(_characterModel, _inputModel, _animationController, this);
            _talkingState = new TalkingState(_characterModel, _inputModel, _animationController, this);
            _deadState = new DeadState(_characterModel, _inputModel, _animationController, this);
            _gettingWeaponState = new GettingWeaponState(_characterModel, _inputModel, _animationController, this);
            _removingWeaponState = new RemovingWeaponState(_characterModel, _inputModel, _animationController, this);
        }

        #endregion


        #region Methods

        public void SetStartState(CharacterBaseState startState)
        {
            PreviousState = startState;
            CurrentState = startState;
        }

        public void SetState(CharacterBaseState newState)
        {
            if (CurrentState != newState)
            {
                if (CurrentState.CanExit)
                {
                    CurrentState.OnExit();
                    PreviousState = CurrentState;
                    CurrentState = newState;
                    OnStateChange(PreviousState, CurrentState);
                    CurrentState.Initialize();
                }
            }
        }

        public void SetStateOverride(CharacterBaseState newState)
        {
            if (CurrentState != newState)
            {
                if (CurrentState.CanBeOverriden)
                {
                    CurrentState.OnExit();
                    PreviousState = CurrentState;
                    CurrentState = newState;
                    OnStateChange(PreviousState, CurrentState);
                    CurrentState.Initialize();
                }
            }
        }

        public void SetStateAnyway(CharacterBaseState newState)
        {
            if (CurrentState != newState)
            {
                CurrentState.OnExit();
                PreviousState = CurrentState;
                CurrentState = newState;
                OnStateChange(PreviousState, CurrentState);
                CurrentState.Initialize();
            }
        }

        public void ReturnState()
        {
            if (CurrentState.CanExit)
            {
                CurrentState.OnExit();
                CharacterBaseState tempState = PreviousState;
                PreviousState = CurrentState;
                CurrentState = tempState;
                OnStateChange(PreviousState, CurrentState);
                CurrentState.Initialize();
            }
        }

        public void ReturnStateOverride()
        {
            if (CurrentState.CanBeOverriden)
            {
                CurrentState.OnExit();
                CharacterBaseState tempState = PreviousState;
                PreviousState = CurrentState;
                CurrentState = tempState;
                OnStateChange(PreviousState, CurrentState);
                CurrentState.Initialize();
            }
        }

        public void ReturnStateAnyway()
        {
            CurrentState.OnExit();
            CharacterBaseState tempState = PreviousState;
            PreviousState = CurrentState;
            CurrentState = tempState;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize();
        }

        private void OnStateChange(CharacterBaseState previousState, CharacterBaseState newState)
        {
            if(OnStateChangeHandler != null)
            {
                OnStateChangeHandler.Invoke(previousState, newState);
            }
        }

        #endregion
    }
}

