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

        [Tooltip("Character default movement runtime animator controller.")]
        [SerializeField] private RuntimeAnimatorController _characterAnimator;

        [Tooltip("Vector 3 prefab position on the scene")]
        [SerializeField] private Vector3 _instantiatePosition;

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

        [Tooltip("Health points between 0 and 100.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _healthPoints;

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

        [Tooltip("Jump horizontal force between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _jumpHorizontalForce;

        [Tooltip("Jump vertical force between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _jumpVerticalForce;

        [Tooltip("Ground check height under character between 0 and 2.")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _groundCheckHeight;

        [Tooltip("Ground check height reduction while jumping between 0 and 2.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _groundCheckHeightReduction;

        [Tooltip("Character speed measuring time frame between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _speedMeasureFrame;

        [Tooltip("Character acceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _accelerationLag;

        [Tooltip("Character deceleraton lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _decelerationLag;

        [Tooltip("Character acceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _inBattleAccelerationLag;

        [Tooltip("Character deceleraton lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _inBattleDecelerationLag;

        [Tooltip("Character direction change lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _directionChangeLag;

        [Tooltip("Base animator speed between 0 and 2.")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _animatorBaseSpeed;

        [Header("Character battle settings")]

        [Tooltip("Battle ignore time between 0 and 1-0.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _battleIgnoreTime;

        [Tooltip("Path of the first hit box object of prefab")]
        [SerializeField] private string _firstHitBoxObjectPath;

        [Tooltip("First hit box radius between 0 and 50.")]
        [Range(0.0f, 50.0f)]
        [SerializeField] private float _firstHitBoxRadius;

        [Tooltip("Path of the second hit box object of prefab")]
        [SerializeField] private string _secondHitBoxObjectPath;

        [Tooltip("Second hit box radius between 0 and 50.")]
        [Range(0.0f, 50.0f)]
        [SerializeField] private float _secondHitBoxRadius;

        [Tooltip("Path of the third hit box object of prefab")]
        [SerializeField] private string _thirdHitBoxObjectPath;

        [Tooltip("Third hit box radius between 0 and 50.")]
        [Range(0.0f, 50.0f)]
        [SerializeField] private float _thirdHitBoxRadius;

        [Tooltip("Damage structure.")]
        [SerializeField] private DamageStruct _damageStruct;

        [Tooltip("Actual rolling time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rollTime;

        [Tooltip("Actual roll distance between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rollFrameDistance;

        [Tooltip("Actual roll speed between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rollAnimationSpeed;

        #endregion


        #region Properties

        public GameObject Prefab => _prefab;

        public RuntimeAnimatorController CharacterAnimator => _characterAnimator;

        public Vector3 InstantiatePosition => _instantiatePosition;
        public Vector3 CapsuleColliderCenter => _capsuleColliderCenter;
        public Vector3 SphereColliderCenter => _sphereColliderCenter;

        public string InstanceName => _instanceName;
        public string InstanceTag => _instanceTag;

        public float InstantiateDirection => _instantiateDirection;
        public float RigitbodyMass => _rigitbodyMass;
        public float RigitbodyDrag => _rigitbodyDrag;
        public float RigitbodyAngularDrag => _rigitbodyAngularDrag;

        public float CapsuleColliderRadius => _capsuleColliderRadius;
        public float CapsuleColliderHeight => _capsuleColliderHeight;
        public float SphereColliderRadius => _sphereColliderRadius;
        public float SphereColliderRadiusIncrese => _sphereColliderRadiusIncrease;

        public float HealthPoints => _healthPoints;
        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float InBattleWalkSpeed => _inBattleWalkSpeed;
        public float InBattleRunSpeed => _inBattleRunSpeed;
        public float JumpHorizontalForce => _jumpHorizontalForce;
        public float JumpVerticalForce => _jumpVerticalForce;
        public float GroundCheckHeight => _groundCheckHeight;
        public float GroundCHeckHeightReduction => _groundCheckHeightReduction;
        public float SpeedMeasureFrame => _speedMeasureFrame;
        public float AccelerationLag => _accelerationLag;
        public float DecelerationLag => _decelerationLag;
        public float InBattleAccelerationLag => _inBattleAccelerationLag;
        public float InBattleDecelerationLag => _inBattleDecelerationLag;
        public float DirectionChangeLag => _directionChangeLag;
        public float AnimatorBaseSpeed => _animatorBaseSpeed;

        public float BattleIgnoreTime => _battleIgnoreTime;

        public string FirstHitBoxObjectPath => _firstHitBoxObjectPath;
        public string SecondHitBoxObjectPath => _secondHitBoxObjectPath;
        public string ThirdHitBoxObjectPath => _thirdHitBoxObjectPath;

        public float FirstHitBoxRadius => _firstHitBoxRadius;
        public float SecondHitBoxRadius => _secondHitBoxRadius;
        public float ThirdHitBoxRadius => _thirdHitBoxRadius;

        public float RollTime => _rollTime;
        public float RollFrameDistance => _rollFrameDistance;
        public float RollAnimationSpeed => _rollAnimationSpeed;

        public DamageStruct CharacterDamage => _damageStruct;

        #endregion
    }
}
