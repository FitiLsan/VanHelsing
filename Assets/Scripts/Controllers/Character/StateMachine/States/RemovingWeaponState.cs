using UnityEngine;


namespace BeastHunter
{
    public sealed class RemovingWeaponState : CharacterBaseState, IUpdate
    {
        #region Fields

        private float _removingTime;
        private float _disappearingTime;

        #endregion


        #region ClassLifeCycle

        public RemovingWeaponState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Default;
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
        }

        public void Updating()
        {
            AppearanceCheck();
            ExitCheck();
            StayOutBattle();
        }

        private void ExitCheck()
        {
            if (_removingTime >= 0)
            {
                _removingTime -= Time.deltaTime;
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
            if (_disappearingTime >= 0)
            {
                _disappearingTime -= Time.deltaTime;

                if (_disappearingTime < 0)
                {
                    //Services.SharedInstance.InventoryService.HideWepons(_characterModel);
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
