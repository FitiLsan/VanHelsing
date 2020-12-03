using UnityEngine;
using UniRx;


namespace BeastHunter
{
    public sealed class CharacterAnimationController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;
        private CharacterModel _characterModel;
        private CharacterAnimationModel _animationModel;
        private Animator _characterAnimator;

        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycles

        public CharacterAnimationController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _characterModel = _context.CharacterModel;
            _animationModel = _characterModel.CharacterAnimationModel;
            _characterAnimator = _animationModel.CharacterAnimator;
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
            if(currentWeapon != null)
            {
                switch (currentWeapon.Type)
                {
                    case WeaponType.Melee:
                        SetTopBodyAnimationWeigth(1f, 0f);
                        PlayArmsAnimationHoldingWeapon();
                        break;
                    case WeaponType.Shooting:
                        SetTopBodyAnimationWeigth(1f, 0f);
                        PlayArmsAnimationHoldingWeapon();
                        break;
                    case WeaponType.Throwing:
                        SetTopBodyAnimationWeigth(1f, 0f);
                        PlayArmsAnimationHoldingWeapon();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                PlayArmsNoneAnimation();
            }
        }

        private void ChangeStateAnimation(CharacterBaseState currentState)
        {
            switch (currentState?.StateName)
            {
                case CharacterStatesEnum.Aiming:
                    SetTopBodyAnimationWeigth(1f, 0f);
                    SetRootMotion(false);
                    PlayStrafeAnimation();
                    PlayArmsAnimationAimingWeapon();
                    break;
                case CharacterStatesEnum.Attacking:
                    SetTopBodyAnimationWeigth(0f, 0f);
                    SetRootMotion(true);
                    PlaySimpleAttackAnimationMelee();
                    // TO REFACTOR - DONT KNOW HOW TO UNDERSTAND IF IT IS SPECIAL ATTACK
                    break;
                case CharacterStatesEnum.Shooting:
                    SetTopBodyAnimationWeigth(1f, 1f);
                    SetRootMotion(false);
                    PlayArmsSimpleAttackAnimation();
                    break;
                case CharacterStatesEnum.Battle:
                    SetTopBodyAnimationWeigth(1f, 0f);
                    SetRootMotion(false);
                    PlayStrafeAnimation();
                    PlayArmsAnimationHoldingWeapon();
                    break;
                case CharacterStatesEnum.Dead:
                    SetTopBodyAnimationWeigth(0f, 0f);
                    break;
                case CharacterStatesEnum.Dodging:
                    SetTopBodyAnimationWeigth(0f, 0f);
                    SetRootMotion(true);
                    SetDodgeAxises();
                    PlayShortDodgeAnimation();
                    break;
                case CharacterStatesEnum.Idle:
                    SetTopBodyAnimationWeigth(1f, 0f);
                    SetRootMotion(false);
                    PlayMovementAnimation();
                    PlayArmsAnimationHoldingWeapon();
                    break;
                case CharacterStatesEnum.Jumping:
                    SetTopBodyAnimationWeigth(0f, 0f);
                    SetRootMotion(true);
                    PlayLongDodgeAnimation();
                    break;
                case CharacterStatesEnum.Movement:
                    SetRootMotion(false);
                    SetTopBodyAnimationWeigth(1f, 0f);
                    if(_characterModel.PreviousCharacterState.Value.StateName != CharacterStatesEnum.GettingUp)
                    {
                        PlayMovementAnimation();
                    }
                    PlayArmsAnimationHoldingWeapon();
                    break;
                case CharacterStatesEnum.Sliding:
                    SetTopBodyAnimationWeigth(0f, 0f);
                    SetRootMotion(true);
                    PlaySlideForwardAnimation();
                    break;
                case CharacterStatesEnum.Sneaking:
                    SetTopBodyAnimationWeigth(1f, 0f);
                    SetRootMotion(false);
                    PlayMovementAnimation();
                    PlayArmsAnimationHoldingWeapon();
                    break;
                case CharacterStatesEnum.TrapPlacing:
                    SetTopBodyAnimationWeigth(0f, 0f);
                    SetRootMotion(false);
                    PlayTrapPlacingAnimation();
                    break;
                case CharacterStatesEnum.KnockedDown:
                    SetTopBodyAnimationWeigth(0f, 0f);
                    PlayArmsNoneAnimation();
                    break;
                case CharacterStatesEnum.GettingUp:
                    SetRootMotion(true);
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

        private void SetTopBodyAnimationWeigth(float armsValue, float torsoValue)
        {
            _characterAnimator.SetLayerWeight(1, armsValue);
            _characterAnimator.SetLayerWeight(2, torsoValue);
        }

        private void PlayMovementAnimation()
        {
            _characterAnimator.Play(_animationModel.MovementAnimationHash);
        }

        private void PlayStrafeAnimation()
        {
            if (_characterModel.IsWeaponInHands)
            {
                _characterAnimator.Play(_characterModel.CurrentWeaponData.Value.StrafeAnimationPostfix);
            }
            else
            {
                _characterAnimator.Play(_animationModel.StrafingAnimationHash);
            }         
        }

        private void PlayShortDodgeAnimation()
        {
            if (_characterModel.IsWeaponInHands)
            {
                _characterAnimator.Play(_characterModel.CurrentWeaponData.Value.DodgeAnimationPostfix);
            }
            else
            {
                _characterAnimator.Play(_animationModel.ShortDodgeAnimationHash);
            }           
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

        private void PlaySimpleAttackAnimationMelee()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.
                SimpleAttackAnimationPrefix + _context.CharacterModel.CurrentWeaponData.Value.
                    CurrentAttack.AnimationName);
        }

        private void PlaySpecialAttackAnimationMelee()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.
                SpecialAttackAnimationPrefix + _context.CharacterModel.CurrentWeaponData.Value.
                    CurrentAttack.AnimationName);
        }

        private void PlayArmsSimpleAttackAnimation()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.
                SimpleAttackAnimationPrefix + _context.CharacterModel.CurrentWeaponData.Value.
                    CurrentAttack.AnimationName, 1);
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.
                SimpleAttackAnimationPrefix + _context.CharacterModel.CurrentWeaponData.Value.
                    CurrentAttack.AnimationName, 2);
        }

        private void PlayArmsSpecialAttackAnimation()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.
                SpecialAttackAnimationPrefix + _context.CharacterModel.CurrentWeaponData.Value.
                    CurrentAttack.AnimationName, 1);
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.
                SpecialAttackAnimationPrefix + _context.CharacterModel.CurrentWeaponData.Value.
                    CurrentAttack.AnimationName, 2);
        }

        private void PlayArmsAnimationGettingWeapon()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.GettingAnimationPostfix, 1);
        }

        private void PlayArmsAnimationHoldingWeapon()
        {
            if (_characterModel.CurrentWeaponData.Value != null)
            {
                _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.HoldingAnimationPostfix, 1);
                PlayTorsoNoneAnimation();
            }         
        }

        private void PlayArmsAnimationAimingWeapon()
        {
            if(_context.CharacterModel.CurrentWeaponData.Value.Type == WeaponType.Shooting)
            {
                _characterAnimator.Play((_context.CharacterModel.CurrentWeaponData.Value as IShoot).
                    AimingAnimationPostfix, 1);
                PlayTorsoNoneAnimation();
            }        
        }

        private void PlayArmsAnimationRemovingWeapon()
        {
            _characterAnimator.Play(_context.CharacterModel.CurrentWeaponData.Value.RemovingAnimationPostfix, 1);
        }

        private void PlayArmsNoneAnimation()
        {
            _characterAnimator.Play("None", 1);
        }

        private void PlayTorsoNoneAnimation()
        {
            _characterAnimator.Play("None", 2);
        }

        #endregion
    }
}

