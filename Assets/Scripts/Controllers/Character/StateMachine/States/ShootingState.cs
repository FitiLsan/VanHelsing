using UnityEngine;


namespace BeastHunter
{
    public sealed class ShootingState : CharacterBaseState, IUpdate
    {
        #region Fields

        private int _attackIndex;
        private float _exitTime;

        #endregion


        #region ClassLifeCycle

        public ShootingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.Shooting;
            IsTargeting = false;
            IsAttacking = true;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _stateMachine.BackState.CountSpeedStrafing();
            ControlMovement();
            ControlAimingTarget();
            CountExtiTime();
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return _characterModel.CurrentWeaponData.Value?.Type == WeaponType.Shooting;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();

            _characterModel.CurrentWeaponData.Value.MakeSimpleAttack(out _attackIndex, _characterModel.CharacterTransform);
            _exitTime = _characterModel.CurrentWeaponData.Value.CurrentAttack.AttackTime;
        }

        private void ControlMovement()
        {
            _stateMachine.BackState.RotateCharacterAimimng();
            _stateMachine.BackState.MoveCharacter(true);
        }

        private void ControlAimingTarget()
        {
            Services.SharedInstance.CameraService.
                SetCameraTargetPosition(new Vector3(_inputModel.MouseInputX, _inputModel.MouseInputY, 0), true);
        }

        private void CountExtiTime()
        {
            if(_exitTime > 0)
            {
                _exitTime -= Time.deltaTime;

                if(_exitTime <= 0)
                {
                    _stateMachine.ReturnState();
                }
            }
        }

        #endregion
    }
}

