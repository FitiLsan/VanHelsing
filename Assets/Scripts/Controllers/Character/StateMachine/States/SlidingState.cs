using UnityEngine;


namespace BeastHunter
{
    public class SlidingState : CharacterBaseState, IUpdate
    {
        #region Properties

        private float _slideTime;

        #endregion


        #region ClassLifeCycle

        public SlidingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.Sliding;
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
            _slideTime = _characterModel.CharacterData.CharacterCommonSettings.SlideTime;
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Disabled;
            _characterModel.IsDodging = true;
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;
            _characterModel.IsDodging = false;
            base.OnExit();
        }

        private void ExitCheck()
        {
            _slideTime -= Time.deltaTime;

            if (_slideTime <= 0)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            }
        }

        #endregion
    }
}

