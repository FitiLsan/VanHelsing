using UnityEngine;


namespace BeastHunter
{
    public class DodgingState : CharacterBaseState, IUpdate
    {
        #region Properties

        private float _rollTime;

        #endregion


        #region ClassLifeCycle

        public DodgingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.Dodging;
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

            _rollTime = _characterModel.CharacterData.CharacterCommonSettings.RollingTime;
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
            _rollTime -= Time.deltaTime;

            if (_rollTime <= 0)
            {
                _stateMachine.ReturnState();
            }
        }

        #endregion
    }
}

