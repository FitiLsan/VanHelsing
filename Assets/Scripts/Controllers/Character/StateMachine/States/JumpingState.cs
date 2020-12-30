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
            StateName = CharacterStatesEnum.Jumping;
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
            _jumpTime = _characterModel.CharacterData.CharacterCommonSettings.JumpTime;
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Disabled;
            _characterModel.IsDodging = true;
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            base.OnExit();
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
