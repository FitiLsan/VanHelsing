using UnityEngine;


namespace BeastHunter
{
    public class GettingWeaponState : CharacterBaseState
    {
        #region Fields

        private float _gettingTime;
        private float _appearingTime;

        #endregion


        #region ClassLifeCycle

        public GettingWeaponState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            CanExit = false;
            CanBeOverriden = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            if (_characterModel.LeftHandWeapon.WeaponHandType == WeaponHandType.TwoHanded)
            {
                _gettingTime = _characterModel.LeftHandWeapon.TimeToGet;
                _appearingTime = _characterModel.LeftHandWeapon.TimeToAppear;
                _animationController.PlayGettingWeaponAnimation(_characterModel.LeftHandWeapon.GettingAnimationHash);
            }
            else
            {
                //to refactor
                _gettingTime = 0;
                _appearingTime = 0;
            }

            CanExit = false;
        }

        public override void Execute()
        {
            AppearanceCheck();
            ExitCheck();
            StayInBattle();
        }

        public override void OnExit()
        {

        }

        private void ExitCheck()
        {
            if (_gettingTime >= 0)
            {
                _gettingTime -= Time.deltaTime;
            }
            else
            {
                CanExit = true;
                _stateMachine.SetState(_stateMachine._battleIdleState);
            }
        }

        private void AppearanceCheck()
        {
            if (_appearingTime >= 0)
            {
                _appearingTime -= Time.deltaTime;

                if(_appearingTime < 0)
                {
                    Services.SharedInstance.InventoryService.ShowWeapons(_characterModel);
                }
            }
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}

