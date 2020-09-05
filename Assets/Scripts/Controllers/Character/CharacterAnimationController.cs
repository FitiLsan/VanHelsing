using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterAnimationController
    {
        #region Fields

        private int _movementHash;
        private int _idleHash;
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
        private int _trapPlaceHash;

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
            _movementHash = Animator.StringToHash("Movement");
            _jumpHash = Animator.StringToHash("Jump");
            _idleHash = Animator.StringToHash("Idle");
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
            _trapPlaceHash = Animator.StringToHash("PlaceTrap");
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

        public void PlayIdleAnimation()
        {
            CharacterAnimator.Play(_idleHash);
        }

        public void PlayMovementAnimation()
        {
            CharacterAnimator.Play(_movementHash);
        }

        public void PlayNotArmedAttackAnimation(int attackIndex)
        {
            CharacterAnimator.Play("NotArmedAttack_"+ attackIndex.ToString());
        }

        public void PlayArmedAttackAnimation(string weaponName, int attackIndex)
        {
            CharacterAnimator.Play(weaponName.ToString() + "Attack_" + attackIndex.ToString());
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

        public void SetLookAt(Vector3 target)
        {
            CharacterAnimator.SetLookAtPosition(target);
        }

        public void StopLookAt()
        {
            CharacterAnimator.SetLookAtWeight(0f);
        }

        public void SetLookAtWeight(float weight, float bodyWeight, float headWeight, float eyesWeight, float clampWeight)
        {
            CharacterAnimator.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
        }

        public void SetCrouchLevel(float level)
        {
            CharacterAnimator.SetFloat("CrouchLevel", level);
        }

        public void PlayTrapPlaceAnimation()
        {
            CharacterAnimator.Play(_trapPlaceHash);
        }

        #endregion
    }
}

