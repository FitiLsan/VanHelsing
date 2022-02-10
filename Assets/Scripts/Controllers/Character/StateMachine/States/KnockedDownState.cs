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
            _stateMachine.BackState.OnEnemyHealthBar(false);
            if (!_characterModel.IsGrounded)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.MidAir]);
            }
            return !_characterModel.CurrentStats.BaseStats.IsDead && _characterModel.IsGrounded;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize(previousState);
            _stateMachine.BackState.StopCharacter();
            _characterModel.CharacterRigitbody.constraints = UnityEngine.RigidbodyConstraints.FreezePositionX | 
                UnityEngine.RigidbodyConstraints.FreezePositionZ | UnityEngine.RigidbodyConstraints.FreezeRotation;
            _characterModel.CharacterRigitbody.isKinematic = true;
            Damage damage = new Damage();
            damage.ElementDamageType = ElementDamageType.None;
            damage.ElementDamageValue = 0;
            damage.PhysicalDamageType = PhysicalDamageType.Crushing;
            damage.PhysicalDamageValue = 10f;
            _stateMachine.BackState.TakeDamage(damage);
        }

        #endregion
    }
}
