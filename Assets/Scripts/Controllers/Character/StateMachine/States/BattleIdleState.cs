namespace BeastHunter
{
    public sealed class BattleIdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public BattleIdleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = true;
            CanBeOverriden = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _animationController.PlayBattleIdleAnimation(_characterModel.LeftHandWeapon, _characterModel.RightHandWeapon);
        }

        public override void Execute()
        {
            StayInBattle();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}


