using UnityEngine;


namespace BeastHunter
{
    public class FallingState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public FallingState(CharacterInputController inputController, CharacterController characterController,
            CharacterModel characterModel) : base(inputController, characterController, characterModel)
        {
            CanExit = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {

        }

        public override void Execute()
        {

        }

        #endregion
    }
}