using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class BossStateMachine
    {
        #region Fields

        public readonly GameContext _context;
        public BossModel _model;

        #endregion


        #region Properties

        public Dictionary<BossStatesEnum, BossBaseState> States { get; }
        public BossBaseState CurrentState { get; private set; }
        public BossBaseState LastState { get; private set; }
        public BossStatesEnum LastStateType { get; private set; }
        public BossStatesEnum CurrentStateType { get; private set; }

        public Action OnStateChange { get; private set; }
        public Action OnAfterStateChange { get; private set; }

        public BossMainState _mainState { get; private set; }
        public BossSkills BossSkills { get; private set; }

        #endregion


        #region ClassLifeCycle

        public BossStateMachine(GameContext context, BossModel model)
        {
            _context = context;
            _model = model;

            _mainState = new BossMainState(this);
            BossSkills = new BossSkills(this);

            States = new Dictionary<BossStatesEnum, BossBaseState>();
            States.Add(BossStatesEnum.Idle, new BossIdleState(this));
            States.Add(BossStatesEnum.Moving, new BossMovingState(this));
            States.Add(BossStatesEnum.Patroling, new BossPatrolingState(this));
            States.Add(BossStatesEnum.Eating, new BossEatingState(this));
            States.Add(BossStatesEnum.Resting, new BossRestingState(this));

            States.Add(BossStatesEnum.Attacking, new BossAttackingState(this));
            States.Add(BossStatesEnum.Defencing, new BossDefencingState(this));
            States.Add(BossStatesEnum.Chasing, new BossChasingState(this));
            States.Add(BossStatesEnum.Retreating, new BossRetreatingState(this));
            States.Add(BossStatesEnum.Searching, new BossSearchingState(this));
            States.Add(BossStatesEnum.Dead, new BossDeadState(this));
            States.Add(BossStatesEnum.Resurrecting, new BossResurrectingState(this));
            States.Add(BossStatesEnum.Standstill, new BossStandstillState(this));
            
         //   States.Add(BossStatesEnum.Targeting, new BossTargetingState(this));

          //  States.Add(BossStatesEnum.Stunned, new BossStunnedState(this));
          //  States.Add(BossStatesEnum.Hitted, new BossHittedState(this));
        }

        #endregion


        #region Methods

        public void OnAwake()
        {
            _mainState.OnAwake();

            foreach (var state in States)
            {
                state.Value.OnAwake();
            }

            OnStateChange += OnStateChangeHandler;
            OnAfterStateChange += OnAfterStateChangeHandler;


            //SetFirstState(_model.MovementPoints.Length > 0 ? BossStatesEnum.Moving : BossStatesEnum.Idle);
            SetFirstState(BossStatesEnum.Idle);
        }

        public void Execute()
        {
            Debug.Log($"current State {CurrentState}");
            CurrentState.Execute();
            _mainState.Execute();
        }

        public void OnTearDown()
        {
            _mainState.OnTearDown();

            foreach (var state in States)
            {
                state.Value.OnTearDown();
            }

            OnStateChange -= OnStateChangeHandler;
            OnAfterStateChange -= OnAfterStateChangeHandler;
        }

        private void SetFirstState(BossStatesEnum name)
        {
            if (States.ContainsKey(name))
            {
                CurrentState = States[name];
                CurrentStateType = name;
                CurrentState.Initialise();
                OnAfterStateChange?.Invoke();
            }
        }

        public void SetCurrentState(BossStatesEnum name)
        {
            if (CurrentState.CanExit && States.ContainsKey(name))
            {
                OnStateChange?.Invoke();
                CurrentState.OnExit();
                LastState = CurrentState;
                LastStateType = CurrentStateType;
                CurrentStateType = name;
                CurrentState = States[name];
                CurrentState.Initialise();
                OnAfterStateChange?.Invoke();
            }
        }

        public void SetCurrentStateOverride(BossStatesEnum name)
        {
            if (CurrentState.CanBeOverriden && States.ContainsKey(name))
            {
                OnStateChange?.Invoke();
                CurrentState.OnExit();
                LastState = CurrentState;
                LastStateType = CurrentStateType;
                CurrentStateType = name;
                CurrentState = States[name];
                CurrentState.Initialise();
                OnAfterStateChange?.Invoke();
            }
        }

        public void SetCurrentStateAnyway(BossStatesEnum name, EffectType type = EffectType.None)
        {
            if (States.ContainsKey(name))
            {
                OnStateChange?.Invoke();
                CurrentState.OnExit();
                LastState = CurrentState;
                LastStateType = CurrentStateType;
                CurrentStateType = name;
                CurrentState = States[name];
                CurrentState.CurrentEffectType = type;
                CurrentState.Initialise();
                
                OnAfterStateChange?.Invoke();
            }
        }

        public void OnStateChangeHandler()
        {
            
        }

        public void OnAfterStateChangeHandler()
        {
            _mainState.CheckCurrentFieldOfView();
        }

        #endregion
    }
}

