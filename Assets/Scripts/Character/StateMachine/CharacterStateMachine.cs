

namespace BeastHunter
{
    public sealed class CharacterStateMachine
    {
        #region Properties

        public CharacterBaseState CurrentState { get; private set; }
        public CharacterBaseState PreviousState { get; private set; }

        #endregion


        #region ClassLifeCycle

        public CharacterStateMachine(CharacterBaseState startState)
        {
            PreviousState = null;
            CurrentState = startState;
            CurrentState.Initialize();
        }

        #endregion


        #region Methods

        public void SetState(CharacterBaseState newState)
        {
            if (CurrentState != newState)
            {
                if (CurrentState.CanExit)
                {
                    CurrentState.OnExit();
                    PreviousState = CurrentState;
                    CurrentState = newState;
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
                CurrentState.Initialize();
            }
        }

        public void ReturnStateAnyway()
        {
            CurrentState.OnExit();
            CharacterBaseState tempState = PreviousState;
            PreviousState = CurrentState;
            CurrentState = tempState;
            CurrentState.Initialize();
        }

        #endregion
    }
}

