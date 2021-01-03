using UnityEngine;


namespace BeastHunter
{
    public sealed class MeleeAttackingState : CharacterBaseState, IUpdate
    {
        #region Fields

        private int _attackIndex;
        private float _exitTIme;

        #endregion


        #region ClassLifeCycle

        public MeleeAttackingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.Attacking;
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
            return _characterModel.CurrentWeaponData.Value?.Type == WeaponType.Melee;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();
            _stateMachine.BackState.OnEnemyHealthBar(true);

            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Kinematic;
            switch (_characterModel.CurrentWeaponData.Value.Type)
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

            _characterModel.CurrentWeaponData.Value.MakeSimpleAttack(out _attackIndex, _characterModel.CharacterTransform);
            _exitTIme = _characterModel.CurrentWeaponData.Value.CurrentAttack.AttackTime;

            _stateMachine.BackState.StopCharacter();
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            _characterModel.PuppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;

            switch (_characterModel.CurrentWeaponData.Value.Type)
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

            base.OnExit(nextState);
        }

        private void CheckExitTime()
        {
            _exitTIme -= Time.deltaTime;

            if (_exitTIme <= 0)
            {
                if (_inputModel.IsInputMove && !_stateMachine.PreviousState.IsTargeting)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
                }
                else
                {
                    _stateMachine.ReturnState();
                }            
            }
        }

        #endregion
    }
}

