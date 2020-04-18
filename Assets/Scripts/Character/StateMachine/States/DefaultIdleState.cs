

namespace BeastHunter
{
    public class DefaultIdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public DefaultIdleState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
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
            StayOnOnePlace();
        }

        private void StayOnOnePlace()
        {
            _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, 0);
        }

        #endregion
    }
}

