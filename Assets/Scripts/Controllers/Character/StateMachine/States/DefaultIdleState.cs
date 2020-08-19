namespace BeastHunter
{
    public sealed class DefaultIdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public DefaultIdleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Default;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            //_eventManager.StartListening(InputEventTypes.MoveStart, _stateMachine.BackState.SetDefaultMovementState);
            //_eventManager.StartListening(InputEventTypes.AttackLeft, _stateMachine.BackState.SetAttackingStateLeft);
            //_eventManager.StartListening(InputEventTypes.AttackRight, _stateMachine.BackState.SetAttackingStateRight);
            //_eventManager.StartListening(InputEventTypes.NumberOne, _stateMachine.BackState.GetWeapon);
            //_eventManager.StartListening(InputEventTypes.BattleExit, _stateMachine.BackState.RemoveWeapon);

            _animationController.PlayDefaultIdleAnimation();
        }

        public override void OnExit()
        {
            base.OnExit();
            //_eventManager.StopListening(InputEventTypes.MoveStart, _stateMachine.BackState.SetDefaultMovementState);
            //_eventManager.StopListening(InputEventTypes.AttackLeft, _stateMachine.BackState.SetAttackingStateLeft);
            //_eventManager.StopListening(InputEventTypes.AttackRight, _stateMachine.BackState.SetAttackingStateRight);
            //_eventManager.StopListening(InputEventTypes.NumberOne, _stateMachine.BackState.GetWeapon);
            //_eventManager.StopListening(InputEventTypes.BattleExit, _stateMachine.BackState.RemoveWeapon);
        }

        #endregion
    }
}

