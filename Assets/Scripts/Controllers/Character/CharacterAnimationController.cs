using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterAnimationController
    {
        #region Constants

        private const string NONE_ANIMATON_NAME = "None";
        private const string MOVEMENT_ANIMATION_NAME = "Movement";
        private const string STRAFE_ANIMATION_NAME = "Strafe";
        private const string SLIDE_FORWARD_ANIMATION_NAME = "SlideForward";
        private const string TURN_AROUND_ANIMATION_NAME = "TurningAround";
        private const string JUMP_FORWARD_ANIMATION_NAME = "JumpForward";
        private const string DODGE_ANIMATION_NAME = "Dodge";
        private const string TRAP_PLACING_ANIMATION_NAME = "TrapPlacing";

        private const string AXIS_X_ANIMATOR_PARAMETER_NAME = "AxisX";
        private const string AXIS_Y_ANIMATOR_PARAMETER_NAME = "AxisY";
        private const string MOVE_SPEED_ANIMATOR_PARAMETER_NAME = "MoveSpeed";
        private const string MOUSE_AXIS_X_ANIMATOR_PARAMETER_NAME = "MouseAxisX";
        private const string CROUCH_LEVEL_ANIMATOR_PARAMETER_NAME = "CrouchLevel";
        private const string DODGE_AXIS_X_ANIMATOR_PARAMETER_NAME = "DodgeAxisX";
        private const string DODGE_AXIS_Y_ANIMATOR_PARAMETER_NAME = "DodgeAxisY";

        private const string NOT_ARMED_ATTACK_ANIMATION_NAME_PREFIX = "NotArmedAttack_";
        private const string ARMED_ATTACK_ANIMATION_NAME_PREFIX = "Attack_";

        #endregion


        #region Fields

        private int _movementHash;
        private int _strafeHash;
        private int _jumpForwardHash;
        private int _dodgeHash;
        private int _slideForwardHash;
        private int _turningAroundHash;
        private int _trapPlacingHash;

        #endregion


        #region Properties

        private Animator CharacterAnimator { get; set; }

        #endregion


        #region ClassLifeCycles

        public CharacterAnimationController(Animator characterAnimator)
        {
            CharacterAnimator = characterAnimator;
            Initialize();
        }

        #endregion


        #region Methods

        public void Initialize()
        {
            _movementHash = Animator.StringToHash(MOVEMENT_ANIMATION_NAME);
            _strafeHash = Animator.StringToHash(STRAFE_ANIMATION_NAME);
            _slideForwardHash = Animator.StringToHash(SLIDE_FORWARD_ANIMATION_NAME);
            _turningAroundHash = Animator.StringToHash(TURN_AROUND_ANIMATION_NAME);
            _jumpForwardHash = Animator.StringToHash(JUMP_FORWARD_ANIMATION_NAME);
            _dodgeHash = Animator.StringToHash(DODGE_ANIMATION_NAME);
            _trapPlacingHash = Animator.StringToHash(TRAP_PLACING_ANIMATION_NAME);
        }

        public void UpdateAnimationParameters(float axisX, float axisY, float rotationAxisX, float moveSpeed, 
            float animationSpeed)
        {
            if (CharacterAnimator != null)
            {
                CharacterAnimator.SetFloat(AXIS_X_ANIMATOR_PARAMETER_NAME, axisX);
                CharacterAnimator.SetFloat(AXIS_Y_ANIMATOR_PARAMETER_NAME, axisY);
                CharacterAnimator.SetFloat(MOVE_SPEED_ANIMATOR_PARAMETER_NAME, moveSpeed);
                CharacterAnimator.SetFloat(MOUSE_AXIS_X_ANIMATOR_PARAMETER_NAME, rotationAxisX);
                CharacterAnimator.speed = animationSpeed;
            }
        }

        public void SetRootMotion(bool shouldBeOn)
        {
            CharacterAnimator.applyRootMotion = shouldBeOn;
        }

        public void SetDodgeAxises(float dodgeAxisX, float dodgeAxisY)
        {
            CharacterAnimator.SetFloat(DODGE_AXIS_X_ANIMATOR_PARAMETER_NAME, dodgeAxisX);
            CharacterAnimator.SetFloat(DODGE_AXIS_Y_ANIMATOR_PARAMETER_NAME, dodgeAxisY);
        }

        public void SetTopBodyAnimationWeigth(float value)
        {
            CharacterAnimator.SetLayerWeight(1, value);
        }

        public void PlayMovementAnimation()
        {
            CharacterAnimator.Play(_movementHash);
        }

        public void PlayStrafeAnimation(string weaponPostfix = null)
        {
            if (weaponPostfix == null)
            {
                CharacterAnimator.Play(_strafeHash);
            }
            else
            {
                CharacterAnimator.Play(STRAFE_ANIMATION_NAME + weaponPostfix);
            }
        }

        public void PlayDodgeAnimation(string weaponPostfix = null)
        {
            if (weaponPostfix == null)
            {
                CharacterAnimator.Play(_dodgeHash);
            }
            else
            {
                CharacterAnimator.Play(DODGE_ANIMATION_NAME + weaponPostfix);
            }
        }

        public void PlayJumpForwardAnimation()
        {
            CharacterAnimator.Play(_jumpForwardHash);
        }

        public void PlaySlideForwardAnimation()
        {
            CharacterAnimator.Play(_slideForwardHash);
        }

        public void PlayTrapPlacingAnimation()
        {
            CharacterAnimator.Play(_trapPlacingHash);
        }

        public void PlayNotArmedAttackAnimation(int attackIndex)
        {
            CharacterAnimator.Play(NOT_ARMED_ATTACK_ANIMATION_NAME_PREFIX + attackIndex.ToString());
        }

        public void PlayArmedAttackAnimation(string weaponName, int attackIndex)
        {
            CharacterAnimator.Play(weaponName.ToString() + ARMED_ATTACK_ANIMATION_NAME_PREFIX + attackIndex.ToString());
        }

        public void SetCrouchLevel(float value)
        {
            CharacterAnimator.SetFloat(CROUCH_LEVEL_ANIMATOR_PARAMETER_NAME, value);  
        }

        public float GetCurrentAnimationTime()
        {
            return CharacterAnimator.GetCurrentAnimatorStateInfo(0).length;
        }

        #endregion
    }
}

