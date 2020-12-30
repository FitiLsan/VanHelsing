﻿using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class CharacterStateMachine
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region Properties

        public Action<CharacterBaseState, CharacterBaseState> OnBeforeStateChangeHangler { get; set; }
        public Action<CharacterBaseState, CharacterBaseState> OnStateChangeHandler { get; set; }
        public Action<CharacterBaseState> OnAfterStateChangeHandler { get; set; }

        public Dictionary<CharacterStatesEnum, CharacterBaseState> CharacterStates { get; private set; }

        public CharacterBaseState PreviousState { get; private set; }
        public CharacterBaseState CurrentState { get; private set; }
        public BackState BackState { get; private set; }       

        #endregion


        #region ClassLifeCycle

        public CharacterStateMachine(GameContext context)
        {
            _context = context;
            CharacterStates = new Dictionary<CharacterStatesEnum, CharacterBaseState>();
            PreviousState = null;
            CurrentState = null;
            BackState = new BackState(_context, this);

            CharacterStates.Add(CharacterStatesEnum.Idle, new IdleState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Movement, new MovementState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Sneaking, new SneakingState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Attacking, new MeleeAttackingState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Jumping, new JumpingState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Sliding, new SlidingState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Battle, new BattleState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Dodging, new DodgingState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Dead, new DeadState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.TimeSkip, new TimeSkipState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.TrapPlacing, new TrapPlacingState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Aiming, new AimingState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.Shooting, new ShootingState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.KnockedDown, new KnockedDownState(_context, this));
            CharacterStates.Add(CharacterStatesEnum.GettingUp, new GettingUpState(_context, this));
        }

        #endregion


        #region Methods

        public void SetStartState(CharacterBaseState startState)
        {
            PreviousState = startState;
            CurrentState = startState;
            BackState.Initialize();
            CurrentState.Initialize();
            OnAfterStateChange(CurrentState);
        }

        public void OnAwake()
        {
            BackState.OnAwake();

            foreach (var state in CharacterStates)
            {
                if(state.Value is IAwake) (state.Value as IAwake).OnAwake();
            }
        }

        public void Execute()
        {
            BackState.Updating();

            if (CurrentState is IUpdate) (CurrentState as IUpdate).Updating();
        }

        public void OnTearDown()
        {
            BackState.TearDown();

            foreach (var state in CharacterStates)
            {
                if (state.Value is ITearDown) (state.Value as ITearDown).TearDown();
            }
        }

        public void SetState(CharacterBaseState newState)
        {
            if (newState.CanBeActivated())
            {
                OnBeforeStateChange(CurrentState, newState);
                CurrentState.OnExit(newState);
                PreviousState = CurrentState;
                CurrentState = newState;
                CurrentState.NextState = null;
                OnStateChange(PreviousState, CurrentState);
                CurrentState.Initialize(PreviousState);
                OnAfterStateChange(CurrentState);
            }
        }

        public void SetState(CharacterBaseState newState, CharacterBaseState nextState)
        {
            if (newState.CanBeActivated())
            {
                OnBeforeStateChange(CurrentState, newState);
                CurrentState.OnExit(newState);
                PreviousState = CurrentState;
                CurrentState = newState;
                CurrentState.NextState = nextState;
                OnStateChange(PreviousState, CurrentState);
                CurrentState.Initialize(PreviousState);
                OnAfterStateChange(CurrentState);
            }
        }

        public void ReturnState()
        {
            OnBeforeStateChange(CurrentState, PreviousState);
            CurrentState.OnExit(PreviousState);
            CharacterBaseState tempState = PreviousState;
            PreviousState = CurrentState;
            CurrentState = tempState;
            CurrentState.NextState = null;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize(PreviousState);
            OnAfterStateChange(CurrentState);
        }

        public void ReturnState(CharacterBaseState nextState)
        {
            OnBeforeStateChange(CurrentState, PreviousState);
            CurrentState.OnExit(PreviousState);
            CharacterBaseState tempState = PreviousState;
            PreviousState = CurrentState;
            CurrentState = tempState;
            CurrentState.NextState = nextState;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize(PreviousState);
            OnAfterStateChange(CurrentState);
        }

        private void OnBeforeStateChange(CharacterBaseState previousState, CharacterBaseState newState)
        {
            OnBeforeStateChangeHangler?.Invoke(previousState, newState);
        }

        private void OnStateChange(CharacterBaseState previousState, CharacterBaseState newState)
        {
            OnStateChangeHandler?.Invoke(previousState, newState);
        }

        private void OnAfterStateChange(CharacterBaseState currentState)
        {
            _context.CharacterModel.PreviousCharacterState.Value = PreviousState;
            _context.CharacterModel.CurrentCharacterState.Value = currentState;          
            OnAfterStateChangeHandler?.Invoke(currentState);
        }

        #endregion
    }
}

