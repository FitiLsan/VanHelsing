using UnityEngine;
using System;


namespace BeastHunter
{
    [Serializable]
    public struct CharacterCommonSettingsStruct
    {
        #region Fields

        [Header("Scene information fields")]

        [Tooltip("Character prefab.")]
        [SerializeField] private GameObject _prefab;

        [Tooltip("Character instance name.")]
        [SerializeField] private string _instanceName;

        [Tooltip("Character instance tag.")]
        [SerializeField] private string _instanceTag;

        [Tooltip("Character head object path")]
        [SerializeField] private string _headObjectPath;

        [Tooltip("Character instance layer.")]
        [SerializeField] private int _instanceLayer;

        [Tooltip("Character default movement runtime animator controller.")]
        [SerializeField] private RuntimeAnimatorController _characterAnimator;

        [Tooltip("Character animator beginning apply root motion.")]
        [SerializeField] private bool _beginningApplyRootMotion;

        [Tooltip("Prefab direction on the scene")]
        [SerializeField] private float _instantiateDirection;

        [Header("Prefab rigitbody information fields")]

        [Tooltip("Character rigitbody mass between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rigitbodyMass;

        [Tooltip("Character rigitbody drag between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rigitbodyDrag;

        [Tooltip("Character rigitbody angular drag between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rigitbodyAngularDrag;

        [Header("Prefab capsule collider information fields")]

        [Tooltip("Capsule collider center position")]
        [SerializeField] private Vector3 _capsuleColliderCenter;

        [Tooltip("Capsule collider radius between 0 and 2.")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _capsuleColliderRadius;

        [Tooltip("Capsule collider height between 0 and 5.")]
        [Range(0.0f, 5.0f)]
        [SerializeField] private float _capsuleColliderHeight;

        [Tooltip("Capsule collider physic material")]
        [SerializeField] private PhysicMaterial _capsuleColliderPhysicMaterial;

        [Header("Prefab sphere trigger information fields")]

        [Tooltip("Sphere trigger center position")]
        [SerializeField] private Vector3 _sphereColliderCenter;

        [Tooltip("Sphere trigger radius between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _sphereColliderRadius;

        [Tooltip("Sphere trigger radius increace in battle between 1 and 2.")]
        [Range(1f, 2f)]
        [SerializeField] private float _sphereColliderRadiusIncrease;

        [Header("Movement settings")]

        [Tooltip("Minimal speed value between 0 and 0.1.")]
        [Range(0.0f, 0.1f)]
        [SerializeField] private float _minimalSpeed;

        [Tooltip("Walk speed value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _walkSpeed;

        [Tooltip("Run speed value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _runSpeed;

        [Tooltip("Walk speed in battle value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _inBattleWalkSpeed;

        [Tooltip("Run speed in battle value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _inBattleRunSpeed;

        [Tooltip("Walk speed while sneaking value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _sneakWalkSpeed;

        [Tooltip("Run speed while sneaking value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _sneakRunSpeed;

        [Tooltip("Walk speed while aiming value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimWalkSpeed;

        [Tooltip("Run speed while aiming value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimRunSpeed;

        [Tooltip("Ground check height under character between 0 and 2.")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _groundCheckHeight;

        [Tooltip("Character acceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _accelerationLag;

        [Tooltip("Character deceleraton lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _decelerationLag;

        [Tooltip("Character acceleration lag in battle between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _inBattleAccelerationLag;

        [Tooltip("Character deceleraton lag in battle between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _inBattleDecelerationLag;

        [Tooltip("Character acceleration lag while sneaking between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _sneakAccelerationLag;

        [Tooltip("Character deceleraton lag while sneaking between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _sneakDecelerationLag;

        [Tooltip("Character acceleration lag while aiming between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _aimAccelerationLag;

        [Tooltip("Character deceleraton lag while aiming between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _aimDecelerationLag;

        [Tooltip("Character direction change lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _directionChangeLag;

        [Tooltip("Base animator speed between 0 and 2.")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _animatorBaseSpeed;

        [Tooltip("Time to continue moving when all keys up")]
        [Range(0.0f, 5.0f)]
        [SerializeField] private float _timeToContinueMovingAfterStop;

        [Header("Character battle settings")]

        [Tooltip("Character aiming direction change speed between 0 and 50.")]
        [Range(0.0f, 50.0f)]
        [SerializeField] private float _aimingDirectionChangeSpeed;

        [Tooltip("Actual rolling time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _jumpTime;

        [Tooltip("Actual sliding time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _slideTime;

        [Tooltip("Actual rolling time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rollTime;

        [Tooltip("Actual dodging time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _dodgingTime;

        [Header("Audio settings")]

        [SerializeField] private Sound[] _stepSounds;
        [SerializeField] private Sound _slideSound;

        [Tooltip("Volume of standart character movement sounds between 0 and 1.")]
        [Range(-80.0f, 20.0f)]
        [SerializeField] private float _standartCharacterMovementMixerVolume;

        [Tooltip("Volume of sneaking character movement sounds between 0 and 1.")]
        [Range(-80.0f, 20.0f)]
        [SerializeField] private float _sneakingCharacterMovementMixerVolume;

        #endregion


        #region Properties

        public GameObject Prefab => _prefab;
        public RuntimeAnimatorController CharacterAnimator => _characterAnimator;
        public PhysicMaterial CapsuleColliderPhysicMaterial => _capsuleColliderPhysicMaterial;
        public Sound[] StepSounds => _stepSounds;
        public Sound SlideSound => _slideSound;

        public Vector3 CapsuleColliderCenter => _capsuleColliderCenter;
        public Vector3 SphereColliderCenter => _sphereColliderCenter;

        public string InstanceName => _instanceName;
        public string InstanceTag => _instanceTag;
        public string HeadObjectPath => _headObjectPath;
        public int InstanceLayer => _instanceLayer;

        public float InstantiateDirection => _instantiateDirection;
        public float RigitbodyMass => _rigitbodyMass;
        public float RigitbodyDrag => _rigitbodyDrag;
        public float RigitbodyAngularDrag => _rigitbodyAngularDrag;

        public float CapsuleColliderRadius => _capsuleColliderRadius;
        public float CapsuleColliderHeight => _capsuleColliderHeight;
        public float SphereColliderRadius => _sphereColliderRadius;
        public float SphereColliderRadiusIncrease => _sphereColliderRadiusIncrease;

        public float MinimalSpeed => _minimalSpeed;
        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float InBattleWalkSpeed => _inBattleWalkSpeed;
        public float InBattleRunSpeed => _inBattleRunSpeed;
        public float SneakWalkSpeed => _sneakWalkSpeed;
        public float SneakRunSpeed => _sneakRunSpeed;
        public float AimWalkSpeed => _aimWalkSpeed;
        public float AimRunSpeed => _aimRunSpeed;
        public float GroundCheckHeight => _groundCheckHeight;
        public float AccelerationLag => _accelerationLag;
        public float DecelerationLag => _decelerationLag;
        public float InBattleAccelerationLag => _inBattleAccelerationLag;
        public float InBattleDecelerationLag => _inBattleDecelerationLag;
        public float SneakAccelerationLag => _sneakAccelerationLag;
        public float SneakDecelerationLag => _sneakDecelerationLag;
        public float AimAccelerationLag => _aimAccelerationLag;
        public float AimDecelerationLag => _aimDecelerationLag;
        public float DirectionChangeLag => _directionChangeLag;
        public float AnimatorBaseSpeed => _animatorBaseSpeed;
        public float TImeToContinueMovingAfterStop => _timeToContinueMovingAfterStop;

        public float AimingDirectionChangeSpeed => _aimingDirectionChangeSpeed;
        public float JumpTime => _jumpTime;
        public float SlideTime => _slideTime;
        public float RollingTime => _rollTime;
        public float DodgingTime => _dodgingTime;

        public float StandartCharacterMovementMixerVolume => _standartCharacterMovementMixerVolume;
        public float SneakingCharacterMovementMixerVolume => _sneakingCharacterMovementMixerVolume;

        public bool BeginningApplyRootMotion => _beginningApplyRootMotion;

        #endregion
    }
}
