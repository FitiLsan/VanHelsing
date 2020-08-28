using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterAnimationController
    {
        #region Fields

        private int _defaultMovementHash;
        private int _defaultIdleHash;
        private int _jumpHash;
        private int _fallHash;
        private int _battleIdleHash;
        private int _battleIdleTwoHandedSliceHash;
        private int _battleMovementHash;
        private int _battleMovementTwoHandedSliceHash;
        private int _battleTargetMovementHash;
        private int _battleTargetMovementTwoHandedSliceHash;
        private int _rollHash;
        private int _rollTwoHandedSliceHash;
        private int _rollForwardHash;
        private int _dancingHash;
        private int _stunnedHash;
        private int _deadHash;

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
            _defaultMovementHash = Animator.StringToHash("DefaultMovement");
            _jumpHash = Animator.StringToHash("Jump");
            _defaultIdleHash = Animator.StringToHash("DefaultIdle");
            _dancingHash = Animator.StringToHash("Dancing");
            _fallHash = Animator.StringToHash("Fall");
            _rollHash = Animator.StringToHash("Roll");
            _rollTwoHandedSliceHash = Animator.StringToHash("RollTwoHandedSlice");
            _rollForwardHash = Animator.StringToHash("RollForward");
            _battleIdleHash = Animator.StringToHash("BattleIdle");
            _battleIdleTwoHandedSliceHash = Animator.StringToHash("BattleIdleTwoHandedSlice");
            _battleMovementHash = Animator.StringToHash("BattleMovement");
            _battleMovementTwoHandedSliceHash = Animator.StringToHash("BattleMovementTwoHandedSlice");
            _battleTargetMovementHash = Animator.StringToHash("BattleTargetMovement");
            _battleTargetMovementTwoHandedSliceHash = Animator.StringToHash("BattleTargetMovementTwoHandedSlice");
            _stunnedHash = Animator.StringToHash("Stunned");
            _deadHash = Animator.StringToHash("Dead");
        }

        public void UpdateAnimationParameters(float axisX, float axisY, float moveSpeed, float animationSpeed)
        {
            if (CharacterAnimator != null)
            {
                CharacterAnimator.SetFloat("AxisX", axisX);
                CharacterAnimator.SetFloat("AxisY", axisY);
                CharacterAnimator.SetFloat("MoveSpeed", moveSpeed);
                CharacterAnimator.speed = animationSpeed;
            }
        }

        public void PlayDefaultIdleAnimation()
        {
            CharacterAnimator.Play(_defaultIdleHash);
        }

        public void PlayDefaultMovementAnimation()
        {
            CharacterAnimator.Play(_defaultMovementHash);
        }

        public void PlayBattleIdleAnimation(WeaponItem leftWeapon, WeaponItem rightWeapon)
        {
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!! to refactor
            if (leftWeapon.WeaponHandType == WeaponHandType.TwoHanded && leftWeapon.WeaponType == WeaponType.MeleeCutting)
            {
                CharacterAnimator.Play(_battleIdleTwoHandedSliceHash);
            }
            else
            {
                CharacterAnimator.Play(_battleIdleHash);
            }
        }

        public void PlayBattleMovementAnimation(WeaponItem leftWeapon, WeaponItem rightWeapon)
        {
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!! to refactor
            if (leftWeapon.WeaponHandType == WeaponHandType.TwoHanded && leftWeapon.WeaponType == WeaponType.MeleeCutting)
            {
                CharacterAnimator.Play(_battleMovementTwoHandedSliceHash);
            }
            else
            {
                CharacterAnimator.Play(_battleMovementHash);
            }
        }

        public void PlayBattleTargetMovementAnimation(WeaponItem leftWeapon, WeaponItem rightWeapon)
        {
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!! to refactor
            if (leftWeapon.WeaponHandType == WeaponHandType.TwoHanded && leftWeapon.WeaponType == WeaponType.MeleeCutting)
            {
                CharacterAnimator.Play(_battleTargetMovementTwoHandedSliceHash);
            }
            else
            {
                CharacterAnimator.Play(_battleTargetMovementHash);
            }
        }

        public void PlayJumpAnimation()
        {
            CharacterAnimator.Play(_jumpHash);
        }

        public void PlayRollAnimation(float rollingX, float rollingY, WeaponItem leftWeapon, WeaponItem rightWeapon)
        {   
            CharacterAnimator.SetFloat("RollingX", rollingX);
            CharacterAnimator.SetFloat("RollingY", rollingY);

            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!! to refactor
            if (leftWeapon.WeaponHandType == WeaponHandType.TwoHanded && leftWeapon.WeaponType == WeaponType.MeleeCutting)
            {
                CharacterAnimator.Play(_rollTwoHandedSliceHash);
            }
            else
            {
                CharacterAnimator.Play(_rollHash);
            }
        }

        public void PlayRollForwardAnimation()
        {
            CharacterAnimator.Play(_rollForwardHash);
        }

        public void PlayDancingAnimation()
        {
            CharacterAnimator.Play(_dancingHash);
        }

        public void PlayAttackAnimation(int attackHash, float attackNumber)
        {
            CharacterAnimator.SetFloat("AttackNumber", attackNumber);
            CharacterAnimator.Play(attackHash);
        }

        public void PlayStunnedAnimation()
        {
            CharacterAnimator.Play(_stunnedHash);
        }

        public void PlayDeadAnimation()
        {
            CharacterAnimator.Play(_deadHash);
        }

        public void PlayFallAnimation()
        {
            CharacterAnimator.Play(_fallHash);
        }

        public void PlayGettingWeaponAnimation(int gettingHash)
        {
            CharacterAnimator.Play(gettingHash);
        }

        public void PlayRemovingWeaponAnimation(int removingHash)
        {
            CharacterAnimator.Play(removingHash);
        }

        #endregion
    }
}

