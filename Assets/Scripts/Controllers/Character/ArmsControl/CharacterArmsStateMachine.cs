using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class CharacterArmsStateMachine
    {
        #region Properties

        public Action<ArmsBaseState, ArmsBaseState> OnBeforeStateChangeHangler { get; set; }
        public Action<ArmsBaseState, ArmsBaseState> OnStateChangeHandler { get; set; }
        public Action<ArmsBaseState> OnAfterStateChangeHandler { get; set; }

        public Dictionary<CharacterArmsStates, ArmsBaseState> ArmsStates { get; private set; }

        public ArmsBaseState PreviousState { get; private set; }
        public ArmsBaseState CurrentState { get; private set; }

        public CharacterAnimationController AnimationController { get; private set; }

        #endregion


        #region ClassLifeCycle

        public CharacterArmsStateMachine(GameContext context, CharacterAnimationController animationController)
        {
            ArmsStates = new Dictionary<CharacterArmsStates, ArmsBaseState>();
            PreviousState = null;
            CurrentState = null;
            AnimationController = animationController;
        }

        #endregion


        #region Methods

        public void SetStartState(ArmsBaseState startState)
        {
            PreviousState = startState;
            CurrentState = startState;
            CurrentState.Initialize();
        }

        public void OnAwake()
        {
            foreach (var state in ArmsStates)
            {
                if (state.Value is IAwake) (state.Value as IAwake).OnAwake();
            }
        }

        public void Execute()
        {
            if (CurrentState is IUpdate) (CurrentState as IUpdate).Updating();
        }

        public void OnTearDown()
        {
            foreach (var state in ArmsStates)
            {
                if (state.Value is ITearDown) (state.Value as ITearDown).TearDown();
            }
        }

        public void SetState(ArmsBaseState newState)
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

        public void SetState(ArmsBaseState newState, ArmsBaseState nextState)
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
            ArmsBaseState tempState = PreviousState;
            PreviousState = CurrentState;
            CurrentState = tempState;
            CurrentState.NextState = null;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize(PreviousState);
            OnAfterStateChange(CurrentState);
        }

        public void ReturnState(ArmsBaseState nextState)
        {
            OnBeforeStateChange(CurrentState, PreviousState);
            CurrentState.OnExit(PreviousState);
            ArmsBaseState tempState = PreviousState;
            PreviousState = CurrentState;
            CurrentState = tempState;
            CurrentState.NextState = nextState;
            OnStateChange(PreviousState, CurrentState);
            CurrentState.Initialize(PreviousState);
            OnAfterStateChange(CurrentState);
        }

        private void OnBeforeStateChange(ArmsBaseState previousState, ArmsBaseState newState)
        {
            OnBeforeStateChangeHangler?.Invoke(previousState, newState);
        }

        private void OnStateChange(ArmsBaseState previousState, ArmsBaseState newState)
        {
            OnStateChangeHandler?.Invoke(previousState, newState);
        }

        private void OnAfterStateChange(ArmsBaseState currentState)
        {
            OnAfterStateChangeHandler?.Invoke(currentState);
        }

        #endregion
    }
}

