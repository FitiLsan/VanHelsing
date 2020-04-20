
namespace BeastHunter
{
    public class FallOnGroundState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public FallOnGroundState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
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

        }

        public override void OnExit()
        {

        }

        #endregion
    }
}

