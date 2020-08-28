namespace BeastHunter
{
    public abstract class CharacterBaseState
    {
        #region Fields

        protected readonly InputModel _inputModel;
        protected readonly CharacterModel _characterModel;
        protected readonly CharacterAnimationController _animationController;
        protected readonly CharacterStateMachine _stateMachine;
        protected readonly EventManager _eventManager;

        #endregion


        #region Properties

        public CharacterBaseState NextState { get; set; }
        public StateType Type { get; protected set; }

        public bool IsActive { get; private set; }
        public bool IsTargeting { get; protected set; }
        public bool IsAttacking { get; protected set; }

        #endregion


        #region ClassLifeCycle

        public CharacterBaseState(GameContext context, CharacterStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _characterModel = context.CharacterModel;
            _inputModel = context.InputModel;
            _animationController = _stateMachine.AnimationController;
            _eventManager = Services.SharedInstance.EventManager;
        }

        #endregion


        #region Methods

        public virtual void Initialize()
        {
            IsActive = true;
        }

        public virtual void OnExit()
        {
            IsActive = false;
        }

        #endregion
    }
}
