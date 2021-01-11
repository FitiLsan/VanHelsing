namespace BeastHunter
{
    public sealed class KnockedDownState : CharacterBaseState, IAwake, ITearDown
    {
        #region ClassLifeCycle

        public KnockedDownState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.KnockedDown;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _characterModel.BehaviorPuppet.OnPostActivate += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingUp]);
        }


        #endregion


        #region ITearDown

        public void TearDown()
        {
            _characterModel.BehaviorPuppet.OnPostActivate -= () => _stateMachine.
               SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingUp]);
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return true;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize(previousState);
            _stateMachine.BackState.StopCharacter();
            _characterModel.CharacterRigitbody.constraints = UnityEngine.RigidbodyConstraints.FreezePositionX | 
                UnityEngine.RigidbodyConstraints.FreezePositionZ | UnityEngine.RigidbodyConstraints.FreezeRotation;
        }

        #endregion
    }
}
