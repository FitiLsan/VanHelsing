using UnityEngine;


namespace BeastHunter
{
    public sealed class RemovingWeaponState : CharacterBaseState
    {
        #region Fields

        private float _removingTime;
        private float _disappearingTime;

        #endregion


        #region ClassLifeCycle

        public RemovingWeaponState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            Type = StateType.Default;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = false;
            CanBeOverriden = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            if (_characterModel.LeftHandWeapon.WeaponHandType == WeaponHandType.TwoHanded)
            {
                _removingTime = _characterModel.LeftHandWeapon.TimeToRemove;
                _disappearingTime = _characterModel.LeftHandWeapon.TimeToDisappear;
                _animationController.PlayGettingWeaponAnimation(_characterModel.LeftHandWeapon.RemovingAnimationHash);
            }
            else
            {
                //torefactor
                _removingTime = 0;
                _disappearingTime = 0;
            }

            CanExit = false;
        }

        public override void Execute()
        {
            AppearanceCheck();
            ExitCheck();
            StayOutBattle();
        }

        public override void OnExit()
        {

        }

        public override void OnTearDown()
        {
        }

        private void ExitCheck()
        {
            if (_removingTime >= 0)
            {
                _removingTime -= Time.deltaTime;
            }
            else
            {
                CanExit = true;

                if(NextState == null)
                {
                    _stateMachine.SetState(_stateMachine._defaultIdleState);
                }
                else
                {
                    _stateMachine.SetState(NextState);
                }
            }
        }

        private void AppearanceCheck()
        {
            if (_disappearingTime >= 0)
            {
                _disappearingTime -= Time.deltaTime;

                if (_disappearingTime < 0)
                {
                    Services.SharedInstance.InventoryService.HideWepons(_characterModel);
                }
            }
        }

        private void StayOutBattle()
        {
            _characterModel.IsInBattleMode = false;
        }

        #endregion
    }
}
