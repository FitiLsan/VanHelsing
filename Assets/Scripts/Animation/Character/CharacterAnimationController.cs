using UnityEngine;
using UniRx;


namespace BeastHunter
{
    public sealed class CharacterAnimationController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;
        private readonly CharacterModel _characterModel;
        private readonly CharacterAnimationModel _animationModel;
        private readonly Animator _characterAnimator;

        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycles

        public CharacterAnimationController(GameContext context)
        {
            _context = context;
            _characterModel = _context.CharacterModel;
            _animationModel = _characterModel.CharacterAnimationModel;
            _characterAnimator = _animationModel.CharacterAnimator;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _characterModel.CurrentCharacterState.Subscribe(ChangeStateAnimation);
            _characterModel.CurrentWeaponData.Subscribe(ChangeArmsAnimation);
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            UpdateAnimationParameters();
            UpdateCrouchLevel();
        }

        #endregion


        #region OnTearDown

        public void TearDown()
        {
            _characterModel.CurrentCharacterState.Dispose();
            _characterModel.CurrentWeaponData.Dispose();
        }

        #endregion


        #region Methods

        private void ChangeArmsAnimation(WeaponData currentWeapon)
        {
            switch (currentWeapon.Type)
            {
                case WeaponType.Melee:
                    break;
                case WeaponType.Shooting:
                    break;
                case WeaponType.Throwing:
                    break;
                default:
                    break;
            }
        }

        private void ChangeStateAnimation(CharacterBaseState currentState)
        {
            switch (currentState.StateName)
            {
                case CharacterStatesEnum.Aiming:
                    SetTopBodyAnimationWeigth(1f);
                    PlayStrafeAnimation();
                    PlayArmsAnimationAimingWeapon();
                    break;
                case CharacterStatesEnum.Attacking:
                    SetRootMotion(true);
                    break;
                case CharacterStatesEnum.Battle:
                    break;
                case CharacterStatesEnum.Dead:
                    break;
                case CharacterStatesEnum.Dodging:
                    break;
                case CharacterStatesEnum.Idle:
                    break;
                case CharacterStatesEnum.Jumping:
                    break;
                case CharacterStatesEnum.Movement:
                    break;
                case CharacterStatesEnum.Sliding:
                    break;
                case CharacterStatesEnum.Sneaking:
                    break;
                case CharacterStatesEnum.TrapPlacing:
                    break;
                default:
                break;
            }
        }

        private void UpdateAnimationParameters()
        {
            _characterAnimator.SetFloat(_animationModel.CharacterAnimationData.XAxisAnimatorParameterName,
                _context.InputModel.InputAxisX);
            _characterAnimator.SetFloat(_animationModel.CharacterAnimationData.YAxisAnimatorParameterName,
                _context.InputModel.InputAxisY);
            _characterAnimator.SetFloat(_animationModel.CharacterAnimationData.MovementSpeedAnimatorParameterName,
                _context.CharacterModel.CurrentSpeed);
            _characterAnimator.SetFloat(_animationModel.CharacterAnimationData.XMouseAxisAnimatorParameterName,
                _context.CharacterModel.CharacterRigitbody.velocity.y);
            _characterAnimator.speed = _context.CharacterModel.AnimationSpeed;
        }

        private void SetDodgeAxises()
        {
            _characterAnimator.SetFloat(_animationModel.CharacterAnimationData.XDodgeAxisAnimatorParameterName, 
                _context.InputModel.InputTotalAxisX);
            _characterAnimator.SetFloat(_animationModel.CharacterAnimationData.YDodgeAxisAnimatorParameterName, 
                _context.InputModel.InputTotalAxisY);
        }

        private void UpdateCrouchLevel()
        {
            if (_characterModel.CurrentCharacterState.Value.StateName == CharacterStatesEnum.Sneaking)
            {
                _characterAnimator.SetFloat(_animationModel.CharacterAnimationData.
                    CrouchLevelAnimatorParameterName, _animationModel.CrouchLevel);
            }
        }

        private void SetRootMotion(bool shouldBeOn)
        {
            _characterAnimator.applyRootMotion = shouldBeOn;
        }

        private void SetTopBodyAnimationWeigth(float value)
        {
            _characterAnimator.SetLayerWeight(1, value);
        }

        private void PlayMovementAnimation()
        {
            _characterAnimator.Play(_animationModel.MovementAnimationHash);
        }

        private void PlayStrafeAnimation()
        {
            _characterAnimator.Play(_animationModel.StrafingAnimationHash);
        }

        private void PlayShortDodgeAnimation()
        {
            _characterAnimator.Play(_animationModel.ShortDodgeAnimationHash);
        }

        private void PlayLongDodgeAnimation()
        {
            _characterAnimator.Play(_animationModel.LongDodgeAnimationHash);
        }

        private void PlaySlideForwardAnimation()
        {
            _characterAnimator.Play(_animationModel.SlidingForwardAnimationHash);
        }

        private void PlayTrapPlacingAnimation()
        {
            _characterAnimator.Play(_animationModel.TrapPlacingAnimationHash);
        }

        private void PlaySimpleAttackAnimation()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.WeaponName + 
                _context.CharacterModel.CurrentWeaponData.Value.SimpleAttackAnimationPrefix +
                    _context.CharacterModel.CurrentWeaponData.Value.CurrentAttack.AnimationName);
        }

        private void PlaySpecialAttackAnimation()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.WeaponName +
                _context.CharacterModel.CurrentWeaponData.Value.SpecialAttackAnimationPrefix +
                    _context.CharacterModel.CurrentWeaponData.Value.CurrentAttack.AnimationName);
        }

        private void PlayArmsSimpleAttackAnimation()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.WeaponName +
                _context.CharacterModel.CurrentWeaponData.Value.SimpleAttackAnimationPrefix +
                    _context.CharacterModel.CurrentWeaponData.Value.CurrentAttack.AnimationName, 1);
        }

        private void PlayArmsSpecialAttackAnimation()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.WeaponName +
                _context.CharacterModel.CurrentWeaponData.Value.SpecialAttackAnimationPrefix +
                    _context.CharacterModel.CurrentWeaponData.Value.CurrentAttack.AnimationName, 1);
        }

        private void PlayArmsAnimationGettingWeapon()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.WeaponName +
                _context.CharacterModel.CurrentWeaponData.Value.GettingAnimationPostfix, 1);
        }

        private void PlayArmsAnimationHoldingWeapon()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.WeaponName +
                _context.CharacterModel.CurrentWeaponData.Value.HoldingAnimationPostfix, 1);
        }

        private void PlayArmsAnimationAimingWeapon()
        {
            if(_context.CharacterModel.CurrentWeaponData.Value.Type == WeaponType.Shooting)
            {
                _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.WeaponName +
                    (_context.CharacterModel.CurrentWeaponData as IShoot).AimingAnimationPostfix, 1);
            }        
        }

        private void PlayArmsAnimationRemovingWeapon()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.WeaponName +
                _context.CharacterModel.CurrentWeaponData.Value.RemovingAnimationPostfix, 1);
        }

        #endregion
    }
}

