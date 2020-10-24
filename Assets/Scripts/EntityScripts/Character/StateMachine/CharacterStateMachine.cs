﻿using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class CharacterStateMachine
    {
        #region Fields

        private List<CharacterBaseState> _allStates;

        #endregion


        #region Properties

        public DefaultIdleState _defaultIdleState { get; }
        public DefaultMovementState _defaultMovementState { get; }
        public JumpingState _jumpingState { get; }
        public FallingState _fallingState { get; }
        public LandingState _landingState { get; }
        public FallOnGroundState _fallOnGroundState { get; }
        public BattleIdleState _battleIdleState { get; }
        public BattleMovementState _battleMovementState { get; }
        public BattleTargetMovementState _battleTargetMovementState { get; }
        public AttackingFromLeftState _attackingLeftState { get; }
        public AttackingFromRightState _attackingRightState { get; }
        public DodgingState _dodgingState { get; }
        public StunnedState _stunnedState { get; }
        public DeadState _deadState { get; }
        public RollingState _rollingState { get; }
        public RollingTargetState _rollingTargetState { get; }
        public TalkingState _talkingState { get; }
        public DancingState _dancingState { get; }
        public GettingWeaponState _gettingWeaponState { get; }
        public RemovingWeaponState _removingWeaponState { get; }

        public CharacterBaseState PreviousState { get; private set; }
        public CharacterBaseState CurrentState { get; private set; }
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
            _allStates = new List<CharacterBaseState>();
            PreviousState = null;
            CurrentState = null;

            _defaultIdleState = (DefaultIdleState)CreateState(new DefaultIdleState(_characterModel, _inputModel, _animationController, this));
            _battleIdleState = (BattleIdleState)CreateState(new BattleIdleState(_characterModel, _inputModel, _animationController, this));
            _defaultMovementState = (DefaultMovementState)CreateState(new DefaultMovementState(_characterModel, _inputModel, _animationController, this));
            _battleMovementState = (BattleMovementState)CreateState(new BattleMovementState(_characterModel, _inputModel, _animationController, this));
            _battleTargetMovementState = (BattleTargetMovementState)CreateState(new BattleTargetMovementState(_characterModel, _inputModel, _animationController, this));
            _attackingLeftState = (AttackingFromLeftState)CreateState(new AttackingFromLeftState(_characterModel, _inputModel, _animationController, this));
            _attackingRightState = (AttackingFromRightState)CreateState(new AttackingFromRightState(_characterModel, _inputModel, _animationController, this));
            _jumpingState = (JumpingState)CreateState(new JumpingState(_characterModel, _inputModel, _animationController, this));
            _dodgingState = (DodgingState)CreateState(new DodgingState(_characterModel, _inputModel, _animationController, this));
            _fallingState = (FallingState)CreateState(new FallingState(_characterModel, _inputModel, _animationController, this));
            _dancingState = (DancingState)CreateState(new DancingState(_characterModel, _inputModel, _animationController, this));
            _rollingState = (RollingState)CreateState(new RollingState(_characterModel, _inputModel, _animationController, this));
            _rollingTargetState = (RollingTargetState)CreateState(new RollingTargetState(_characterModel, _inputModel, _animationController, this));
            _landingState = (LandingState)CreateState(new LandingState(_characterModel, _inputModel, _animationController, this));
            _fallOnGroundState = (FallOnGroundState)CreateState(new FallOnGroundState(_characterModel, _inputModel, _animationController, this));
            _stunnedState = (StunnedState)CreateState(new StunnedState(_characterModel, _inputModel, _animationController, this));
            _talkingState = (TalkingState)CreateState(new TalkingState(_characterModel, _inputModel, _animationController, this));
            _deadState = (DeadState)CreateState(new DeadState(_characterModel, _inputModel, _animationController, this));
            _gettingWeaponState = (GettingWeaponState)CreateState(new GettingWeaponState(_characterModel, _inputModel, _animationController, this));
            _removingWeaponState = (RemovingWeaponState)CreateState(new RemovingWeaponState(_characterModel, _inputModel, _animationController, this));
        }

        #endregion


        #region Methods

        private CharacterBaseState CreateState(CharacterBaseState newState)
        {
            _allStates.Add(newState);
            return newState;
        }

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
                    CurrentState.NextState = null;
                    OnStateChange(PreviousState, CurrentState);
                    CurrentState.Initialize();
                }
            }
        }

        public void SetState(CharacterBaseState newState, CharacterBaseState nextState)
        {
            if (CurrentState != newState)
            {
                if (CurrentState.CanExit)
                {
                    CurrentState.OnExit();
                    PreviousState = CurrentState;
                    CurrentState = newState;
                    CurrentState.NextState = nextState;
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
                    CurrentState.NextState = null;
                    OnStateChange(PreviousState, CurrentState);
                    CurrentState.Initialize();
                }
            }
        }

        public void SetStateOverride(CharacterBaseState newState, CharacterBaseState nextState)
        {
            if (CurrentState != newState)
            {
                if (CurrentState.CanBeOverriden)
                {
                    CurrentState.OnExit();
                    PreviousState = CurrentState;
                    CurrentState = newState;
                    CurrentState.NextState = nextState;
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
                CurrentState.NextState = null;
                OnStateChange(PreviousState, CurrentState);
                CurrentState.Initialize();
            }
        }

        public void SetStateAnyway(CharacterBaseState newState, CharacterBaseState nextState)
        {
            if (CurrentState != newState)
            {
                CurrentState.OnExit();
                PreviousState = CurrentState;
                CurrentState = newState;
                CurrentState.NextState = nextState;
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
                CurrentState.NextState = null;
                OnStateChange(PreviousState, CurrentState);
                CurrentState.Initialize();
            }
        }

        public void ReturnState(CharacterBaseState nextState)
        {
            if (CurrentState.CanExit)
            {
                CurrentState.OnExit();
                CharacterBaseState tempState = PreviousState;
                PreviousState = CurrentState;
                CurrentState = tempState;
                CurrentState.NextState = nextState;
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
                CurrentState.NextState = null;
                OnStateChange(PreviousState, CurrentState);
                CurrentState.Initialize();
            }
        }

        public void ReturnStateOverride(CharacterBaseState nextState)
        {
            if (CurrentState.CanBeOverriden)
            {
                CurrentState.OnExit();
                CharacterBaseState tempState = PreviousState;
                PreviousState = CurrentState;
                CurrentState = tempState;
                CurrentState.NextState = nextState;
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
            CurrentState.NextState = null;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize();
        }

        public void ReturnStateAnyway(CharacterBaseState nextState)
        {
            CurrentState.OnExit();
            CharacterBaseState tempState = PreviousState;
            PreviousState = CurrentState;
            CurrentState = tempState;
            CurrentState.NextState = nextState;
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

        public void TearDownStates()
        {
            foreach (var state in _allStates)
            {
                state.OnTearDown();
            }
        }

        #endregion
    }
}

