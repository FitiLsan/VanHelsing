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
            BackState = new BackState(context, this);

            AnimationController = animationController;

            CharacterStates.Add(CharacterStatesEnum.AttackingFromLeft, new AttackingFromLeftState(context, this));
            CharacterStates.Add(CharacterStatesEnum.AttackingFromRight, new AttackingFromRightState(context, this));
            CharacterStates.Add(CharacterStatesEnum.BattleIdle, new BattleIdleState(context, this));
            CharacterStates.Add(CharacterStatesEnum.BattleMovement, new BattleMovementState(context, this));
            CharacterStates.Add(CharacterStatesEnum.BattleTargetMovement, new BattleTargetMovementState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Dancing, new DancingState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Dead, new DeadState(context, this));
            CharacterStates.Add(CharacterStatesEnum.DefaultIdle, new DefaultIdleState(context, this));
            CharacterStates.Add(CharacterStatesEnum.DefaultMovement, new DefaultMovementState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Falling, new DodgingState(context, this));
            CharacterStates.Add(CharacterStatesEnum.FallOnGround, new FallOnGroundState(context, this));
            CharacterStates.Add(CharacterStatesEnum.GettingWeapon, new GettingWeaponState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Jumping, new JumpingState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Kicking, new KickingState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Landing, new LandingState(context, this));
            CharacterStates.Add(CharacterStatesEnum.RemovingWeapon, new RemovingWeaponState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Rolling, new RollingState(context, this));
            CharacterStates.Add(CharacterStatesEnum.RollingTarget, new RollingTargetState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Stunned, new StunnedState(context, this));
            CharacterStates.Add(CharacterStatesEnum.Talking, new TalkingState(context, this));
            CharacterStates.Add(CharacterStatesEnum.SneakingIdle, new CrouchIdleState(context, this));
            CharacterStates.Add(CharacterStatesEnum.SneakingMovement, new CrouchMovementState(context, this));
            CharacterStates.Add(CharacterStatesEnum.DefaultToCrouch, new DefaultToCrouchState(context, this));
            CharacterStates.Add(CharacterStatesEnum.CrouchToDefault, new CrouchToDefaultState(context, this));
            CharacterStates.Add(CharacterStatesEnum.TrapPlace, new TrapPlaceState(context, this));
        }

        #endregion


        #region Methods

        public void SetStartState(CharacterBaseState startState)
        {
            PreviousState = startState;
            CurrentState = startState;
        }

        public void OnAwake()
        {
            // BackState.OnAwake();

            foreach (var state in CharacterStates)
            {
               // On Awake state
            }
        }

        public void Execute()
        {
            BackState.Execute();

            foreach (var state in CharacterStates)
            {
                state.Value.Execute();
            }
        }

        public void OnTearDown()
        {
            BackState.OnTearDown();

            foreach (var state in CharacterStates)
            {
                state.Value.OnTearDown();
            }
        }

        public void SetState(CharacterBaseState newState)
        {
            if (CurrentState != newState)
            {
                if (CurrentState.CanExit)
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
            }
        }

        public void SetState(CharacterBaseState newState, CharacterBaseState nextState)
        {
            if (CurrentState != newState)
            {
                if (CurrentState.CanExit)
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
            }
        }

        public void SetStateOverride(CharacterBaseState newState)
        {
            if (CurrentState != newState)
            {
                if (CurrentState.CanBeOverriden)
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
            }
        }

        public void SetStateOverride(CharacterBaseState newState, CharacterBaseState nextState)
        {
            if (CurrentState != newState)
            {
                if (CurrentState.CanBeOverriden)
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
            }
        }

        public void SetStateAnyway(CharacterBaseState newState)
        {
            if (CurrentState != newState)
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
        }

        public void SetStateAnyway(CharacterBaseState newState, CharacterBaseState nextState)
        {
            if (CurrentState != newState)
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
        }

        public void ReturnState()
        {
            if (CurrentState.CanExit)
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
        }

        public void ReturnState(CharacterBaseState nextState)
        {
            if (CurrentState.CanExit)
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
        }

        public void ReturnStateOverride()
        {
            if (CurrentState.CanBeOverriden)
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
        }

        public void ReturnStateOverride(CharacterBaseState nextState)
        {
            if (CurrentState.CanBeOverriden)
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
        }

        public void ReturnStateAnyway()
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

        public void ReturnStateAnyway(CharacterBaseState nextState)
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

