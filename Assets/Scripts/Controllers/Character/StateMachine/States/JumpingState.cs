using UnityEngine;


namespace BeastHunter
{
    public sealed class JumpingState : CharacterBaseState, IUpdate
    {
        #region Properties

        private float _jumpTime;

        #endregion


        #region ClassLifeCycle

        public JumpingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            IsTargeting = false;
            IsAttacking = false;            
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            ExitCheck();
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
            _jumpTime = _characterModel.CharacterData._characterCommonSettings.JumpTime;
            _animationController.SetRootMotion(true);
            _animationController.PlayJumpForwardAnimation();
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Disabled;
            _characterModel.IsDodging = true;
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            base.OnExit();
            _animationController.SetRootMotion(false);
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;
            _characterModel.IsDodging = false;
        }

        private void ExitCheck()
        {
            _jumpTime -= Time.deltaTime;

            if (_jumpTime <= 0)
            {
                CheckNextState();
            }
        }

        private void CheckNextState()
        {
            if (_inputModel.IsInputMove)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            }
            else
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Idle]);
            }
        }

        #endregion
    }
}
