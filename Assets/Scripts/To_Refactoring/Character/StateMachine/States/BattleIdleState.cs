using UnityEngine;


namespace BeastHunter
{
    public class BattleIdleState : CharacterBaseState
    {
        #region Properties

        public Collider ClosestEnemy { get; private set; }

        #endregion


        #region ClassLifeCycle

        public BattleIdleState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            CanExit = true;
            CanBeOverriden = true;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _animationController.PlayBattleIdleAnimation(_characterModel.LeftHandWeapon, _characterModel.RightHandWeapon);
        }

        public override void Execute()
        {
            StayInBattle();
        }

        public override void OnExit()
        {

        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}


