

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
            if(CurrentState != newState)
            {
                if (CurrentState.CanExit)
                {
                    PreviousState = CurrentState;
                    CurrentState = newState;
                    CurrentState.Initialize();
                }
            }          
        }

        public void ReturnState()
        {
            if (CurrentState.CanExit)
            {
                CharacterBaseState tempState = PreviousState;
                PreviousState = CurrentState;
                CurrentState = tempState;
                CurrentState.Initialize();
            }
        }

        #endregion
    }
}

