namespace BeastHunter
{
    public abstract class BossBaseState
    {
        #region Fields

        protected BossStateMachine _stateMachine;
        protected BossData _bossData;
        protected BossModel _bossModel;
        protected BossMainState _mainState;
        #endregion


        #region Properties

        public bool CanExit { get; protected set; }
        public bool CanBeOverriden { get; protected set; }
        public bool CurrentStateType { get; protected set; }
        public bool IsBattleState { get; protected set; }

        #endregion


        #region ClassLifeCycle

        public BossBaseState(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _bossData = _stateMachine._model.BossData;
            _bossModel = _stateMachine._model;
            _mainState = _stateMachine._mainState;
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

