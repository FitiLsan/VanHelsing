namespace BeastHunter
{
    public class DefaultIdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public DefaultIdleState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            CanExit = true;
            CanBeOverriden = true;
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            _animationController.PlayDefaultIdleAnimation();
            _characterModel.CameraCinemachineBrain.m_DefaultBlend.m_Time = 0f;
            _characterModel.CharacterTargetCamera.Priority = 5;
        }

        public override void Execute()
        {

        }

        public override void OnExit()
        {

        }

        #endregion
    }
}

