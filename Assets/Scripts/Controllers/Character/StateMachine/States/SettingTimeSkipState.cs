using UnityEngine;


namespace BeastHunter
{
    public sealed class SettingTimeSkipState : CharacterBaseState, ITearDown
    {
        #region ClassLifeCycle

        public SettingTimeSkipState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.NotActive;
            IsTargeting = false;
            IsAttacking = false;

            Services.SharedInstance.TimeSkipService.OnOpenCloseWindow += OnOpenCloseWindowHandler;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            _animationController.PlayIdleAnimation();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Services.SharedInstance.CameraService.SetActiveCamera(Services.SharedInstance.CameraService.CharacterTargetCamera);
        }

        public override void OnExit()
        {
            base.OnExit();
            Cursor.lockState = CursorLockMode.Locked;
            if (Services.SharedInstance.TimeSkipService.IsOpen)
            {
                Services.SharedInstance.TimeSkipService.CloseTimeSkipMenu();
            }
            Services.SharedInstance.CameraService.SetActiveCamera(Services.SharedInstance.CameraService.CharacterFreelookCamera);
        }

        public void TearDown()
        {
            Services.SharedInstance.TimeSkipService.OnOpenCloseWindow -= OnOpenCloseWindowHandler;
        }

        private void OnOpenCloseWindowHandler(bool isEnabled)
        {
            if (!isEnabled)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
            }         
        }

        #endregion
    }
}

