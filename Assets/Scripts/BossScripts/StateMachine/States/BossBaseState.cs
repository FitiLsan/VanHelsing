namespace BeastHunter
{
    public abstract class BossBaseState
    {
        #region Fields

        protected BossStateMachine _stateMachine;

        #endregion


        #region Properties

        public bool CanExit { get; protected set; }
        public bool CanBeOverriden { get; protected set; }

        #endregion


        #region ClassLifeCycle

        public BossBaseState(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        #endregion


        #region Methods

        public abstract void OnAwake();

        public abstract void Initialise();

        public abstract void Execute();

        public abstract void OnExit();

        public abstract void OnTearDown();

        #endregion
    }
}

