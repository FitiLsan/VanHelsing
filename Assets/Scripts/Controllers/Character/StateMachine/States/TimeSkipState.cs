using UnityEngine;


namespace BeastHunter
{
    public class TimeSkipState : CharacterBaseState
    {
        #region ClassLifeCycle

        public TimeSkipState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.TimeSkip;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return !IsActive;
        }

        protected override void EnableActions()
        {
            base.EnableActions();
            _stateMachine.BackState.OnTimeSkipOpenClose = CloseTimeSkipWithButton;
            Services.SharedInstance.TimeSkipService.OnOpenCloseWindow += CloseTimeSkipWithUI;
        }

        protected override void DisableActions()
        {
            _stateMachine.BackState.OnTimeSkipOpenClose = null;
            Services.SharedInstance.TimeSkipService.OnOpenCloseWindow -= CloseTimeSkipWithUI;
            base.DisableActions();
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Services.SharedInstance.CameraService.LockFreeLookCamera();
            Services.SharedInstance.TimeSkipService.OpenTimeSkipMenu();

        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Services.SharedInstance.CameraService.UnlockFreeLookCamera();

            base.OnExit();
        }

        private void CloseTimeSkipWithUI(bool isWindowOpen)
        {
            if (!isWindowOpen)
            {
                _stateMachine.ReturnState();
            }         
        }

        private void CloseTimeSkipWithButton()
        {
            Services.SharedInstance.TimeSkipService.CloseTimeSkipMenu();
            _stateMachine.ReturnState();
        }

        #endregion
    }
}

