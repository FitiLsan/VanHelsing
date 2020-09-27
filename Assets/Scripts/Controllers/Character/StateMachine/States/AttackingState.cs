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

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();
            _animationController.SetRootMotion(true);
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Kinematic;

            if(_characterModel.CurrentWeapon == null)
            {
                _attackIndex = Random.Range(0, 2);
                _exitTIme = 0.8f;
                _animationController.PlayNotArmedAttackAnimation(_attackIndex);
            }
            else
            {
                if (_characterModel.FirstWeaponBehavior != null) _characterModel.FirstWeaponBehavior.IsInteractable = true;
                if (_characterModel.SecondWeaponBehavior != null) _characterModel.SecondWeaponBehavior.IsInteractable = true;

                _characterModel.CurrentWeaponData.MakeSimpleAttack(out _attackIndex);
                _exitTIme = _characterModel.CurrentWeaponData.CurrentAttack.AttackTime;
                _animationController.PlayArmedAttackAnimation(_characterModel.CurrentWeaponData.SimpleAttackAnimationPrefix, _attackIndex);
            }

            _stateMachine.BackState.StopCharacter();
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;

            if (_characterModel.CurrentWeapon != null)
            {
                if (_characterModel.FirstWeaponBehavior != null) _characterModel.FirstWeaponBehavior.IsInteractable = false;
                if (_characterModel.SecondWeaponBehavior != null) _characterModel.SecondWeaponBehavior.IsInteractable = false;
            }

            _animationController.SetRootMotion(false);
            base.OnExit(nextState);
        }

        private void CheckExitTime()
        {
            _exitTIme -= Time.deltaTime;

            if(_exitTIme <= 0)
            {
                _stateMachine.ReturnState();
            }
        }

        #endregion
    }
}

