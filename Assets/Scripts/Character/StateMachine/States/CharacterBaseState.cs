

namespace BeastHunter
{
    public abstract class CharacterBaseState
    {
        #region Fields

        protected readonly InputModel _inputModel;
        protected readonly CharacterModel _characterModel;

        #endregion


        #region Properties

        public bool CanExit { get; protected set; }
        public bool CanBeOverriden { get; protected set; }

        #endregion


        #region ClassLifeCycle

        public CharacterBaseState(CharacterModel characterModel, InputModel inputModel)
        {
            _characterModel = characterModel;
            _inputModel = inputModel;
        }

        #endregion


        #region Methods

        public abstract void Initialize();

        public abstract void Execute();

        public abstract void OnExit();

        #endregion
    }
}
