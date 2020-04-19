using UnityEngine;


namespace BeastHunter
{
    public class AttackingState : CharacterBaseState
    {
        #region Constants

        private const float EXIT_TIME = 0.1f;
        private readonly float[] ATTACKS_TIME = new float[3] { 0.8f, 1f, 0.8f };

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
            CanBeOverriden = true;
            _inputModel.OnAttack += OnPressAttack;
            _currentAttackIndex = ATTACKS_TIME.Length-1;
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            StayInBattle();
            _currentExitTime = EXIT_TIME;
            _characterModel.IsAttacking = true;
        }

        public override void Execute()
        {
            CountTimeBetweenAttacks();         
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
            if (_currentAttackTime <= 0)
            {
                if (_currentExitTime > 0)
                {
                    _currentExitTime -= Time.deltaTime;
                }
                else
                {
                    _currentExitTime = 0;
                    CanExit = true;
                    _characterModel.IsAttacking = false;
                }
            }
            else
            {
                _currentExitTime = EXIT_TIME;
                CanExit = false;
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
            if(_currentAttackTime == 0)
            {
                SetNextAttack();
                _currentAttackTime = ATTACKS_TIME[_currentAttackIndex];
                _currentExitTime = EXIT_TIME;
                _characterModel.PlayerHitBoxes[_currentAttackIndex].IsInteractable = true;
                Debug.Log(_currentAttackIndex);
            }
        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        #endregion
    }
}
