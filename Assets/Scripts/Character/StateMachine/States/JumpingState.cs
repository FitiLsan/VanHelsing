using UnityEngine;


namespace BeastHunter
{
    public class JumpingState : CharacterBaseState
    {
        #region Fields

        #endregion


        #region Properties

        public float JumpVerticalForce { get; private set; }
        public float JumpHorizontalForce { get; private set; }
        public float JumpTime { get; private set; }

        #endregion


        #region ClassLifeCycle

        public JumpingState(CharacterInputController inputController, CharacterController characterController,
                CharacterModel characterModel) : base(inputController, characterController, characterModel)
        {
            CanExit = false;
            JumpVerticalForce = _characterModel.CharacterStruct.JumpVerticalForce;
            JumpHorizontalForce = _characterModel.CharacterStruct.JumpHorizontalForce;
            JumpTime = 2;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _characterController._characterAnimationsController.
                ChangeRuntimeAnimator(_characterModel.CharacterStruct.CharacterJumpingAnimator);
        }

        public override void Execute()
        {
            Jumping();
        }

        public void Jumping()
        {

        }

        #endregion
    }
}

