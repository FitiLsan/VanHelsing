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

        public abstract bool CanBeActivated();

        public virtual void Initialize(CharacterBaseState previousState = null)
        {         
            EnableActions();
        }

        public virtual void OnExit(CharacterBaseState nextState = null)
        {
            DisableActions();       
        }

        protected virtual void EnableActions() { IsActive = true; }
        protected virtual void DisableActions() { IsActive = false; }

        #endregion
    }
}
