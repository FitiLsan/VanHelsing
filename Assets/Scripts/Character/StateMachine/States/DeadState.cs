

namespace BeastHunter
{
    public class DeadState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public DeadState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
        {
            CanExit = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {

        }

        public override void Execute()
        {
            StayDead();
        }

        private void StayDead()
        {
            _characterModel.IsDead = true;
        }

        #endregion
    }
}

