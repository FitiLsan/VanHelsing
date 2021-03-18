using DG.Tweening;


namespace BeastHunter
{
    public sealed class HittedState : CharacterBaseState
    {
        #region Properties

        private bool IsStopping { get; set; }
        private float ExitTime { get; set; }

        #endregion


        #region ClassLifeCycle

        public HittedState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.Hitted;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return !IsActive && _stateMachine.CurrentState != _stateMachine.CharacterStates[CharacterStatesEnum.KnockedDown] &&
                _stateMachine.CurrentState != _stateMachine.CharacterStates[CharacterStatesEnum.MidAir] &&
                _stateMachine.CurrentState != _stateMachine.CharacterStates[CharacterStatesEnum.GettingUp] && 
                _characterModel.BehaviorPuppet.state != RootMotion.Dynamics.BehaviourPuppet.State.Unpinned;
        }


        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();
            DOVirtual.DelayedCall(2f, () => _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]));
        }


        #endregion
    }
}

