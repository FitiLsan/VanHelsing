using UnityEngine;


namespace BeastHunter
{
    public class AttackingState : CharacterBaseState, IUpdate
    {
        #region Fields

        private int _attackIndex;
        private float _exitTIme;

        #endregion


        #region ClassLifeCycle

        public AttackingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Battle;
            IsTargeting = false;
            IsAttacking = true;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            CheckExitTime();
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();

            if(_characterModel.CurrentWeapon == null)
            {
                _attackIndex = Random.Range(0, 2);
                _exitTIme = 0.8f;
                _animationController.PlayNotArmedAttackAnimation(_attackIndex);
            }
            else
            {
                _attackIndex = Random.Range(0, _characterModel.CurrentWeaponItem.AttacksRight.Length);
                _exitTIme = _characterModel.CurrentWeaponItem.AttacksRight[_attackIndex].Time;
                _animationController.PlayArmedAttackAnimation(_characterModel.CurrentWeaponItem.name, _attackIndex);
            }

            _stateMachine.BackState.StopCharacter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void CheckExitTime()
        {
            _exitTIme -= Time.deltaTime;

            if(_exitTIme <= 0)
            {
                _stateMachine.SetState(_stateMachine.PreviousState);
            }
        }

        #endregion
    }
}

