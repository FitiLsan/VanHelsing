using UnityEngine;


namespace BeastHunter
{
    public sealed class RollingState : CharacterBaseState, IUpdate
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
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            if (_inputModel.IsInputMove)
            {
                RollTime = _characterModel.CharacterCommonSettings.RollTime;
                _characterModel.AnimationSpeed = _characterModel.CharacterCommonSettings.RollAnimationSpeed;
                _animationController.PlayRollForwardAnimation();
                _characterModel.IsDodging = true;
            }
            else
            {
                CheckNextState();
            }
        }

        public void Updating()
        {
            ExitCheck();
            Roll();
            StayInBattle();
        }

        public override void OnExit()
        {
            base.OnExit();
            _characterModel.AnimationSpeed = _characterModel.CharacterCommonSettings.AnimatorBaseSpeed;
            _characterModel.IsDodging = false;
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
            if (NextState == null)
            {
                if (_inputModel.IsInputMove)
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
