

namespace BeastHunter
{
    public abstract class CharacterBaseState
    {
        #region Fields

        protected readonly CharacterInputController _inputController;
        protected readonly CharacterController _characterController;
        protected readonly CharacterModel _characterModel;

        #endregion


        #region Properties

        public bool CanExit { get; protected set; }

        #endregion


        #region ClassLifeCycle

        public CharacterBaseState(CharacterInputController inputController, CharacterController characterController,
            CharacterModel characterModel)
        {
            _inputController = inputController;
            _characterController = characterController;
            _characterModel = characterModel;
        }

        #endregion


        #region Methods

        public abstract void Initialize();

        public abstract void Execute();

        #endregion
    }

}
