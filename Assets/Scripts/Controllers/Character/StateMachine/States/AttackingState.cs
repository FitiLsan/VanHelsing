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

        public override bool CanBeActivated()
        {
            if(!_characterModel.IsWeaponInHands && _characterModel.CurrentPlacingTrapModel.Value != null)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.TrapPlacing]);
            }
            return _characterModel.IsWeaponInHands;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();

            _animationController.SetRootMotion(true);
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Kinematic;
            _animationController.SetTopBodyAnimationWeigth(0f);

            switch (_characterModel.CurrentWeaponData.Type)
            {
                case WeaponType.Melee:
                    if (_characterModel.WeaponBehaviorLeft != null) _characterModel.WeaponBehaviorLeft.IsInteractable = true;
                    if (_characterModel.WeaponBehaviorRight != null) _characterModel.WeaponBehaviorRight.IsInteractable = true;
                    break;
                case WeaponType.Shooting:
                    break;
                case WeaponType.Throwing:
                    break;
                default:
                    break;
            }

            _characterModel.CurrentWeaponData.MakeSimpleAttack(out _attackIndex, _characterModel.CharacterTransform);
            _exitTIme = _characterModel.CurrentWeaponData.CurrentAttack.AttackTime;
            _animationController.PlayArmedAttackAnimation(_characterModel.CurrentWeaponData.SimpleAttackAnimationPrefix, _attackIndex);

            _stateMachine.BackState.StopCharacter();
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;
            _animationController.SetTopBodyAnimationWeigth(1f);

            switch (_characterModel.CurrentWeaponData.Type)
            {
                case WeaponType.Melee:
                    if (_characterModel.WeaponBehaviorLeft != null) _characterModel.WeaponBehaviorLeft.IsInteractable = false;
                    if (_characterModel.WeaponBehaviorRight != null) _characterModel.WeaponBehaviorRight.IsInteractable = false;
                    break;
                case WeaponType.Shooting:
                    break;
                case WeaponType.Throwing:
                    break;
                default:
                    break;
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

