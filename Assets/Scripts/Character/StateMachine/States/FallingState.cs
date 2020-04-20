

namespace BeastHunter
{
    public class FallingState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public FallingState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
        {
            CanExit = false;
            CanBeOverriden = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {

        }

        public override void Execute()
        {
            ExitCheck();
        }

        public override void OnExit()
        {

        }

        public void ExitCheck()
        {
            if (_characterModel.IsGrounded)
            {
                CanExit = true;
            }
        }

        #endregion
    }
}