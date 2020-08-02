using UnityEngine;


namespace BeastHunter
{
    public sealed class AttackingFromRightState : CharacterBaseState, IUpdate
    {
        #region Constants

        private const float TIME_PART_TO_ENABLE_WEAPON = 0f;

        #endregion


        #region Fields

        private float _currentAttackTime;
        private int _currentAttackIndex;

        #endregion


        #region ClassLifeCycle

        public AttackingFromRightState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = true;
            _currentAttackIndex = 0;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            _currentAttackIndex = Random.Range(0, _characterModel.RightHandWeapon.AttacksRight.Length);
            _characterModel.RightHandWeapon.CurrentAttack = _characterModel.RightHandWeapon.AttacksRight[_currentAttackIndex];
            _currentAttackTime = _characterModel.RightHandWeapon.CurrentAttack.Time;
            _animationController.PlayAttackAnimation(_characterModel.RightHandWeapon.SimpleAttackFromRightkAnimationHash, 
                _currentAttackIndex);
            TimeRemaining enableWeapon = new TimeRemaining(EnableWeapon, TIME_PART_TO_ENABLE_WEAPON * _currentAttackTime);
            enableWeapon.AddTimeRemaining(TIME_PART_TO_ENABLE_WEAPON * _currentAttackTime);
        }

        public void Updating()
        {
            ExitCheck();
            LookAtEnemy();
            StayInBattle();          
        }

        public override void OnExit()
        {
            base.OnExit();
            _characterModel.RightWeaponBehavior.IsInteractable = false;
        }

        private void ExitCheck()
        {
            if (_currentAttackTime > 0)
            {
                _currentAttackTime -= Time.deltaTime;
            }
            else
            {
                if (_stateMachine.PreviousState == _stateMachine.CharacterStates[CharacterStatesEnum.BattleTargetMovement])
                {
                    _stateMachine.ReturnState();
                }
            }
        }

        private void LookAtEnemy()
        {
            if (_characterModel.ClosestEnemy != null && _stateMachine.PreviousState.IsTargeting)
            {
                _characterModel.CharacterTransform.LookAt(_characterModel.ClosestEnemy.transform);
            }
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        private void EnableWeapon()
        {
            _characterModel.RightWeaponBehavior.IsInteractable = true;
        }

        #endregion
    }
}
