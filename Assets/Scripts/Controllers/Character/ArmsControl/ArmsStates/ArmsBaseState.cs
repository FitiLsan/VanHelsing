namespace BeastHunter
{
    public abstract class ArmsBaseState
    {
        #region Fields

        protected readonly InputModel _inputModel;
        protected readonly CharacterModel _characterModel;
        protected readonly CharacterAnimationController _animationController;
        protected readonly CharacterArmsStateMachine _stateMachine;
        protected readonly EventManager _eventManager;

        #endregion


        #region Properties

        public ArmsBaseState NextState { get; set; }

        public bool IsActive { get; private set; }
        public bool IsTargeting { get; protected set; }
        public bool IsAttacking { get; protected set; }

        #endregion


        #region ClassLifeCycle

        public ArmsBaseState(GameContext context, CharacterArmsStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _characterModel = context.CharacterModel;
            _inputModel = context.InputModel;
            _animationController = _stateMachine.AnimationController;
            _eventManager = Services.SharedInstance.EventManager;
        }

        #endregion


        #region Methods

        public abstract bool CanBeActivated();

        public virtual void Initialize(ArmsBaseState previousState = null)
        {
            EnableActions();
        }

        public virtual void OnExit(ArmsBaseState nextState = null)
        {
            DisableActions();
        }

        protected virtual void EnableActions() { IsActive = true; }
        protected virtual void DisableActions() { IsActive = false; }

        #endregion
    }
}

