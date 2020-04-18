using UnityEngine;


namespace BeastHunter
{
    public class AttackingState : CharacterBaseState
    {
        #region Constants

        private const float EXIT_TIME = 0.5f;
        private readonly float[] ATTACKS_TIME = new float[3] { 1.5f, 2f, 1.5f };

        #endregion


        #region Fields

        private float _currentExitTime;
        private float _currentAttackTime;
        private int _currentAttackIndex;

        #endregion


        #region ClassLifeCycle

        public AttackingState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
        {
            CanExit = false;
            _inputModel.OnAttack += OnPressAttack;
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            _characterModel.IsInBattleMode = true;
            _currentExitTime = EXIT_TIME;
            _currentAttackTime = 0;
            _characterModel.IsAttacking = true;
        }

        public override void Execute()
        {
            CountTimeBetweenAttacks();
            ExitCheck();
            StayInBattle();
        }

        private void ExitCheck()
        {
            if(_currentExitTime > 0)
            {
                _currentExitTime -= Time.deltaTime;
            }
            else
            {
                _currentExitTime = 0;
            }

            if (_currentExitTime + _currentAttackTime <= 0)
            {
                _characterModel.IsAttacking = false;
                CanExit = true;
            }
        }

        private void CountTimeBetweenAttacks()
        {
            if(_currentAttackTime > 0)
            {
                _currentAttackTime -= Time.deltaTime;
            }
            else
            {
                _currentAttackTime = 0;
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

            _characterModel.AttackIndex = _currentAttackIndex;
        }

        private void OnPressAttack()
        {
            if(_currentAttackTime < EXIT_TIME)
            {
                _currentAttackTime = ATTACKS_TIME[_currentAttackIndex];
                _currentExitTime = EXIT_TIME;
                SetNextAttack();
            }
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}
