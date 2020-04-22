using UnityEngine;


namespace BeastHunter
{
    public class AttackingState : CharacterBaseState
    {
        #region Constants

        private readonly float[] ATTACKS_TIME = new float[3] { 0.9f, 0.8f, 0.9f };

        #endregion


        #region Fields

        private float _currentAttackTime;
        private int _currentAttackIndex;

        #endregion


        #region ClassLifeCycle

        public AttackingState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            CanExit = false;
            CanBeOverriden = false;
            _currentAttackIndex = ATTACKS_TIME.Length-1;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            SetNextAttack();
            _currentAttackTime = ATTACKS_TIME[_currentAttackIndex];
            _characterModel.PlayerHitBoxes[_currentAttackIndex].IsInteractable = true;
            _animationController.PlayAttackAnimation(_currentAttackIndex);
            CanExit = false;
        }

        public override void Execute()
        {       
            ExitCheck();
            StayInBattle();
        }

        public override void OnExit()
        {
            foreach (var hitBox in _characterModel.PlayerHitBoxes)
            {
                hitBox.IsInteractable = false;
            }
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

        private void SetNextAttack()
        {
            if(_currentAttackIndex == ATTACKS_TIME.Length - 1)
            {
                _currentAttackIndex = 0;
            }
            else
            {
                _currentAttackIndex++;
            }
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}
