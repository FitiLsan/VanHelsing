using UnityEngine;


namespace BeastHunter
{
    public sealed class AttackingFromLeftState : CharacterBaseState
    {
        #region Fields

        private float _currentAttackTime;
        private int _currentAttackIndex;

        #endregion


        #region ClassLifeCycle

        public AttackingFromLeftState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
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
            _currentAttackIndex = Random.Range(0, _characterModel.LeftHandWeapon.AttacksLeft.Length);
            _characterModel.LeftHandWeapon.CurrentAttack = _characterModel.LeftHandWeapon.AttacksLeft[_currentAttackIndex];
            _currentAttackTime = _characterModel.LeftHandWeapon.CurrentAttack.Time;
            _animationController.PlayAttackAnimation(_characterModel.LeftHandWeapon.SimpleAttackFromLeftkAnimationHash, _currentAttackIndex);
            _characterModel.LeftWeaponBehavior.IsInteractable = true;
            CanExit = false;
        }

        public override void Execute()
        {       
            ExitCheck();
            StayInBattle();
        }

        public override void OnExit()
        {
            _characterModel.LeftWeaponBehavior.IsInteractable = false;
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

                if(_stateMachine.PreviousState == _stateMachine._battleTargetMovementState)
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
