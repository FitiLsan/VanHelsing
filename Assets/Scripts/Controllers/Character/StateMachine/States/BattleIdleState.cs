namespace BeastHunter
{
    public sealed class BattleIdleState : CharacterBaseState, IUpdate
    {
        #region ClassLifeCycle

        public BattleIdleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            _animationController.PlayBattleIdleAnimation(_characterModel.LeftHandWeapon, _characterModel.RightHandWeapon);
        }

        public void Updating()
        {
            StayInBattle();
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}


