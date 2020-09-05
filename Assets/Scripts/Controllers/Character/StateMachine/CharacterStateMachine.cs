﻿using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class CharacterStateMachine
    {
        #region Properties

        public Action<CharacterBaseState, CharacterBaseState> OnBeforeStateChangeHangler { get; set; }
        public Action<CharacterBaseState, CharacterBaseState> OnStateChangeHandler { get; set; }
        public Action<CharacterBaseState> OnAfterStateChangeHandler { get; set; }

        public Dictionary<CharacterStatesEnum, CharacterBaseState> CharacterStates { get; private set; }

        public CharacterBaseState PreviousState { get; private set; }
        public CharacterBaseState CurrentState { get; private set; }
        public BackState BackState { get; private set; }

        public CharacterAnimationController AnimationController { get; private set; }

        #endregion


        #region ClassLifeCycle

        public CharacterStateMachine(GameContext context, CharacterAnimationController animationController)
        {
            CharacterStates = new Dictionary<CharacterStatesEnum, CharacterBaseState>();
            PreviousState = null;
            CurrentState = null;

            AnimationController = animationController;
            BackState = new BackState(context, this);

            CharacterStates.Add(CharacterStatesEnum.DefaultIdle, new IdleState(context, this));
            CharacterStates.Add(CharacterStatesEnum.DefaultMovement, new MovementState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Sneaking, new SneakingState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Attacking, new AttackingState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Jumping, new JumpingState(context, this));
        }

        #endregion


        #region Methods

        public void SetStartState(CharacterBaseState startState)
        {
            PreviousState = startState;
            CurrentState = startState;
            BackState.Initialize();
            CurrentState.Initialize();
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
            CustomDebug.Log(CurrentState);
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
            OnBeforeStateChange(CurrentState, newState);
            CurrentState.OnExit();
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.NextState = null;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize();
            OnAfterStateChange(CurrentState);
        }

        public void SetState(CharacterBaseState newState, CharacterBaseState nextState)
        {
            OnBeforeStateChange(CurrentState, newState);
            CurrentState.OnExit();
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.NextState = nextState;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize();
            OnAfterStateChange(CurrentState);
        }

        public void ReturnState()
        {
            OnBeforeStateChange(CurrentState, PreviousState);
            CurrentState.OnExit();
            CharacterBaseState tempState = PreviousState;
            PreviousState = CurrentState;
            CurrentState = tempState;
            CurrentState.NextState = null;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize();
            OnAfterStateChange(CurrentState);
        }

        public void ReturnState(CharacterBaseState nextState)
        {
            OnBeforeStateChange(CurrentState, PreviousState);
            CurrentState.OnExit();
            CharacterBaseState tempState = PreviousState;
            PreviousState = CurrentState;
            CurrentState = tempState;
            CurrentState.NextState = nextState;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize();
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
            OnAfterStateChangeHandler?.Invoke(currentState);
        }

        #endregion
    }
}

