using UniRx;


namespace BeastHunter
{
    public sealed class DeadState : CharacterBaseState
    {
        #region ClassLifeCycle

        public DeadState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.Dead;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return !IsActive;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();
            _characterModel.CurrentStats.BaseStats.IsDead = true;

            MessageBroker.Default.Publish(new OnPlayerDieEventCLass());

            LoseWeapon();
            DisablePhysics();
        }

        #endregion


        #region Methods

        private void LoseWeapon()
        {
            if (_characterModel.CurrentWeaponData != null)
            {
                _characterModel.PuppetMaster.propMuscles[0].currentProp = null;
                _characterModel.PuppetMaster.propMuscles[1].currentProp = null;
            }
        }

        private void DisablePhysics()
        {
            _characterModel.PuppetMaster.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
            _characterModel.CharacterRigitbody.isKinematic = true;
        }

        #endregion
    }
}

