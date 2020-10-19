namespace BeastHunter
{
    public class BattleState : CharacterBaseState, IAwake, IUpdate, ITearDown
    {
        #region FIelds

        private float _baseAnimationSpeed;
        private float _animationSpeedWhileRun;
        private bool _hasCameraControl;

        #endregion


        #region ClassLifeCycle

        public BattleState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            IsTargeting = false;
            IsAttacking = false;
            _baseAnimationSpeed = _characterModel.CharacterCommonSettings.AnimatorBaseSpeed;
            _animationSpeedWhileRun = _characterModel.CharacterCommonSettings.
                InBattleRunSpeed / _characterModel.CharacterCommonSettings.InBattleWalkSpeed;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _stateMachine.BackState.OnWeaponChange += SetAnimation;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _stateMachine.BackState.CountSpeedStrafing();
            ControlMovement();
            ClosestEnemyCheck();
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            _stateMachine.BackState.OnWeaponChange -= SetAnimation;
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            ClosestEnemyCheck();
            return _characterModel.ClosestEnemy != null;
        }

        protected override void EnableActions()
        {
            base.EnableActions();
            _stateMachine.BackState.OnAim = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            _stateMachine.BackState.OnAttack = () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Attacking]);
            _stateMachine.BackState.OnStartRun = () => _stateMachine.BackState.SetAnimatorSpeed(_animationSpeedWhileRun);
            _stateMachine.BackState.OnStopRun = () => _stateMachine.BackState.SetAnimatorSpeed(_baseAnimationSpeed);
            _stateMachine.BackState.OnJump = Dodge;
            _stateMachine.BackState.OnWeaponWheelOpen += () => _hasCameraControl = false;
            _stateMachine.BackState.OnWeaponWheelClose += () => _hasCameraControl = true;
        }

        protected override void DisableActions()
        {
            _stateMachine.BackState.OnAim = null;
            _stateMachine.BackState.OnAttack = null;
            _stateMachine.BackState.OnStartRun = null;
            _stateMachine.BackState.OnStopRun = null;
            _stateMachine.BackState.OnJump = null;
            _stateMachine.BackState.OnWeaponWheelOpen -= () => _hasCameraControl = false;
            _stateMachine.BackState.OnWeaponWheelClose -= () => _hasCameraControl = true;
            base.DisableActions();
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();
            _hasCameraControl = true;

            if (Services.SharedInstance.CameraService.CurrentActiveCamera != Services.
                SharedInstance.CameraService.CharacterTargetCamera)
            {
                Services.SharedInstance.CameraService.SetActiveCamera(Services.SharedInstance.
                    CameraService.CharacterTargetCamera);
            }

            if (_inputModel.IsInputRun)
            {
                _stateMachine.BackState.SetAnimatorSpeed(_animationSpeedWhileRun);
            }

            SetAnimation();
        }

        public override void OnExit(CharacterBaseState nextState = null)
        {
            if(nextState != _stateMachine.CharacterStates[CharacterStatesEnum.Attacking] && nextState != _stateMachine.
                CharacterStates[CharacterStatesEnum.Dodging])
            {
                Services.SharedInstance.CameraService.SetActiveCamera(Services.SharedInstance.
                    CameraService.CharacterFreelookCamera);
            }

            _stateMachine.BackState.SetAnimatorSpeed(_baseAnimationSpeed);
            _animationController.SetTopBodyAnimationWeigth(0f);

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

        private void SetAnimation()
        {
            if(_stateMachine.CurrentState == this)
            {
                _animationController.SetTopBodyAnimationWeigth(1f);
                _animationController.PlayStrafeAnimation(_characterModel.CurrentWeaponData?.StrafeAndDodgePostfix);
            }            
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

                _characterModel.ClosestEnemy = _characterModel.EnemiesInTrigger[closestEnemyIndex];
            }
            else
            {
                _characterModel.ClosestEnemy = null;
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            }
        }

        #endregion
    }
}

