using UnityEngine;

namespace BeastHunter
{
    public class BattleState : CharacterBaseState, IUpdate
    {
        #region FIelds

        private float _baseAnimationSpeed;
        private float _animationSpeedWhileRun;
        private bool _hasCameraControl;

        #endregion


        #region ClassLifeCycle

        public BattleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.Battle;
            IsTargeting = true;
            IsAttacking = false;
            _baseAnimationSpeed = _characterModel.CharacterCommonSettings.AnimatorBaseSpeed;
            _animationSpeedWhileRun = _characterModel.CharacterCommonSettings.
                InBattleRunSpeed / _characterModel.CharacterCommonSettings.InBattleWalkSpeed;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _stateMachine.BackState.CountSpeed();
            ControlMovement();
            ClosestEnemyCheck();        
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            if(_characterModel.CurrentWeaponData.Value?.Type == WeaponType.Shooting ||
                _characterModel.CurrentWeaponData.Value?.Type == WeaponType.Throwing)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Aiming]);
            }
            else
            {
                
                ClosestEnemyCheck();
            }
            
            return _characterModel.ClosestEnemy.Value != null && _characterModel.IsWeaponInHands && _characterModel.
                CurrentWeaponData.Value.Type != WeaponType.Shooting && _characterModel.
                    CurrentWeaponData.Value.Type != WeaponType.Throwing;
        }

        protected override void EnableActions()
        {
            base.EnableActions();
            _inputModel.OnAim += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            _inputModel.OnAttack += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Attacking]);
            _inputModel.OnRunStart = () => _stateMachine.BackState.SetAnimatorSpeed(_animationSpeedWhileRun);
            _inputModel.OnRunStop = () => _stateMachine.BackState.SetAnimatorSpeed(_baseAnimationSpeed);
            _inputModel.OnJump += Dodge;
            _inputModel.OnWeaponWheel += CheckCameraControl;
        }

        protected override void DisableActions()
        {
            _inputModel.OnAim = null;
            _inputModel.OnAttack = null;
            _inputModel.OnRunStart = null;
            _inputModel.OnRunStop = null;
            _inputModel.OnJump = null;
            _inputModel.OnWeaponWheel = null;
            base.DisableActions();
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();
            _hasCameraControl = true;
            _stateMachine.BackState.OnEnemyHealthBar(true);

            if (_inputModel.IsInputRun)
            {
                _stateMachine.BackState.SetAnimatorSpeed(_animationSpeedWhileRun);
            }
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            _stateMachine.BackState.SetAnimatorSpeed(_baseAnimationSpeed);
            _stateMachine.BackState.OnEnemyHealthBar(false);

            base.OnExit();
        }

        private void ControlMovement()
        {
            _stateMachine.BackState.RotateCharacter(false, _hasCameraControl);
            _stateMachine.BackState.MoveCharacter(true);
        }

        private void Dodge()
        {
            if (_inputModel.IsInputMove)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Dodging]);
            }
        }
           
        private void CheckCameraControl(bool isControlLocked)
        {
            _hasCameraControl = isControlLocked;
        }

        private void ClosestEnemyCheck()
        {         
            if (_characterModel.EnemiesInTrigger.Count > 0)
            {
                float currentDistanceToEnemy = float.PositiveInfinity;
                float smallestDistanceToEnemy = currentDistanceToEnemy;
                int closestEnemyIndex = 0;

                foreach (var enemy in _characterModel.EnemiesInTrigger)
                {
                    currentDistanceToEnemy = (enemy.transform.position - _characterModel.CharacterTransform.position).
                        sqrMagnitude;

                    if (currentDistanceToEnemy < smallestDistanceToEnemy)
                    {
                        closestEnemyIndex = _characterModel.EnemiesInTrigger.IndexOf(enemy);
                        smallestDistanceToEnemy = currentDistanceToEnemy;
                    }
                }
               
                _characterModel.ClosestEnemy.Value = _characterModel.EnemiesInTrigger[closestEnemyIndex];
                
            }
            else
            {            
                _characterModel.ClosestEnemy.Value = null;
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            }       
        }

        #endregion
    }
}

