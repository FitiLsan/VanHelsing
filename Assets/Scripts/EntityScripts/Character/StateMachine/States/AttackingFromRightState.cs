using UnityEngine;


namespace BeastHunter
{
    public sealed class AttackingFromRightState : CharacterBaseState
    {
        #region Fields

        private float _currentAttackTime;
        private int _currentAttackIndex;

        #endregion


        #region ClassLifeCycle

        public AttackingFromRightState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = true;
            CanExit = false;
            CanBeOverriden = false;
            _currentAttackIndex = 0;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _currentAttackIndex = Random.Range(0, _characterModel.RightHandWeapon.AttacksRight.Length);
            _characterModel.RightHandWeapon.CurrentAttack = _characterModel.RightHandWeapon.AttacksRight[_currentAttackIndex];
            _currentAttackTime = _characterModel.RightHandWeapon.CurrentAttack.Time;
            _animationController.PlayAttackAnimation(_characterModel.RightHandWeapon.SimpleAttackFromRightkAnimationHash, _currentAttackIndex);
            _characterModel.RightWeaponBehavior.IsInteractable = true;
            CanExit = false;
        }

        public override void Execute()
        {
            ExitCheck();
            StayInBattle();
        }

        public override void OnExit()
        {
            _characterModel.RightWeaponBehavior.IsInteractable = false;
        }

        public override void OnTearDown()
        {
        }

        private void ExitCheck()
        {
            if (_currentAttackTime > 0)
            {
                _currentAttackTime -= Time.deltaTime;
            }
            else
            {
                CanExit = true;

                if (_stateMachine.PreviousState == _stateMachine._battleTargetMovementState)
                {
                    _stateMachine.ReturnState();
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
