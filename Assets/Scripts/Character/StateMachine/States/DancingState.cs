
namespace BeastHunter
{
    public class DancingState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public DancingState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
        {
            CanExit = false;
            CanBeOverriden = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _characterModel.IsDansing = true;
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
