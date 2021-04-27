namespace BeastHunter
{
    public class MidAirState : CharacterBaseState, IUpdate
    {
        #region ClassLifeCycle

        public MidAirState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.KnockedDown;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            if (_characterModel.IsGrounded) _stateMachine.
                    SetState(_stateMachine.CharacterStates[CharacterStatesEnum.KnockedDown]);
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            _stateMachine.BackState.OnEnemyHealthBar(false);
            return !_characterModel.CurrentStats.BaseStats.IsDead && !_characterModel.IsGrounded;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize(previousState);
            _characterModel.CharacterRigitbody.isKinematic = false;
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

