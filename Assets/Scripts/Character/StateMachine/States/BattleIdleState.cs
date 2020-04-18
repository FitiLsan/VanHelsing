

namespace BeastHunter
{
    public class BattleIdleState : CharacterBaseState
    {
        #region ClassLifeCycle

        public BattleIdleState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
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
            StayInBattle();
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        private void StayOnOnePlace()
        {
            _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, 0);
        }

        #endregion
    }
}


