using UnityEngine;


namespace BeastHunter
{
    public sealed class RollingState : CharacterBaseState
    {
        #region Properties

        private float RollTime { get; set; }

        #endregion


        #region ClassLifeCycle

        public RollingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = false;
            CanBeOverriden = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            if (_characterModel.IsMoving)
            {
                RollTime = _characterModel.CharacterCommonSettings.RollTime;
                _characterModel.AnimationSpeed = _characterModel.CharacterCommonSettings.RollAnimationSpeed;
                CanExit = false;
                CanBeOverriden = false;
                _animationController.PlayRollForwardAnimation();
                _characterModel.IsDodging = true;
            }
            else
            {
                CheckNextState();
            }
        }

        public override void Execute()
        {
            ExitCheck();
            Roll();
            StayInBattle();
        }

        public override void OnExit()
        {
            _characterModel.AnimationSpeed = _characterModel.CharacterCommonSettings.AnimatorBaseSpeed;
            _characterModel.IsDodging = false;
        }

        public override void OnTearDown()
        {
        }

        private void ExitCheck()
        {
            RollTime -= Time.deltaTime;

            if (RollTime <= 0)
            {
                CheckNextState();
            }
        }

        private void CheckNextState()
        {
            CanExit = true;
            CanBeOverriden = true;

            if (NextState == null)
            {
                if (_characterModel.IsMoving)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.BattleMovement]);
                }
                else
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.BattleIdle]);
                }             
            }
            else
            {
                _stateMachine.SetState(NextState);
            }
        }

        private void Roll()
        {
            if (RollTime > 0 && _characterModel.IsGrounded)
            {
                _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, _characterModel.CharacterData.
                    _characterCommonSettings.RollFrameDistance);
            }
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}
