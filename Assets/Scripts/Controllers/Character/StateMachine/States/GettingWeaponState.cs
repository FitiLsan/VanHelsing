using UnityEngine;


namespace BeastHunter
{
    public sealed class GettingWeaponState : CharacterBaseState, IUpdate
    {
        #region Fields

        private float _gettingTime;
        private float _appearingTime;

        #endregion


        #region ClassLifeCycle

        public GettingWeaponState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();

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
        }

        public void Updating()
        {
            AppearanceCheck();
            ExitCheck();
            StayInBattle();
        }

        private void ExitCheck()
        {
            if (_gettingTime >= 0)
            {
                _gettingTime -= Time.deltaTime;
            }
            else
            {
                if(NextState == null)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
                }
                else
                {
                    _stateMachine.SetState(NextState);
                }
            }
        }

        private void AppearanceCheck()
        {
            if (_appearingTime >= 0)
            {
                _appearingTime -= Time.deltaTime;

                if(_appearingTime < 0)
                {
                    //Services.SharedInstance.InventoryService.ShowWeapons(_characterModel);
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

