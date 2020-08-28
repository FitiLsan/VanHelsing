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

        [Tooltip("Character instance layer.")]
        [SerializeField] private int _instanceLayer;

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

        [Tooltip("Walk speed while sneaking value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _sneakWalkSpeed;

        [Tooltip("Run speed while sneaking value between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _sneakRunSpeed;

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

        [Tooltip("Character direction change lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _directionChangeLag;

        [Tooltip("Base animator speed between 0 and 2.")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _animatorBaseSpeed;

        [Header("Character battle settings")]

        [Tooltip("Target mark prefab.")]
        [SerializeField] private GameObject _targetPrefab;

        [Tooltip("Target mark height above.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _targetMarkHeight;

        [Tooltip("Target mark name")]
        [SerializeField] private string _targetMarkName;

        [Tooltip("Battle ignore time between 0 and 1-0.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _battleIgnoreTime;

        [Tooltip("Left foot hit box radius between 0 and 50.")]
        [Range(0.0f, 50.0f)]
        [SerializeField] private float _leftFootHitBoxRadius;

        [Tooltip("Right foot hit box radius between 0 and 50.")]
        [Range(0.0f, 50.0f)]
        [SerializeField] private float _rightFootHitBoxRadius;

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
        public GameObject TargetPrefab => _targetPrefab;

        public RuntimeAnimatorController CharacterAnimator => _characterAnimator;

        public Vector3 InstantiatePosition => _instantiatePosition;
        public Vector3 CapsuleColliderCenter => _capsuleColliderCenter;
        public Vector3 SphereColliderCenter => _sphereColliderCenter;

        public string InstanceName => _instanceName;
        public string InstanceTag => _instanceTag;
        public int InstanceLayer => _instanceLayer;

        public float InstantiateDirection => _instantiateDirection;
        public float RigitbodyMass => _rigitbodyMass;
        public float RigitbodyDrag => _rigitbodyDrag;
        public float RigitbodyAngularDrag => _rigitbodyAngularDrag;

        public float CapsuleColliderRadius => _capsuleColliderRadius;
        public float CapsuleColliderHeight => _capsuleColliderHeight;
        public float SphereColliderRadius => _sphereColliderRadius;
        public float SphereColliderRadiusIncrease => _sphereColliderRadiusIncrease;

        public float HealthPoints => _healthPoints;
        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float InBattleWalkSpeed => _inBattleWalkSpeed;
        public float InBattleRunSpeed => _inBattleRunSpeed;
        public float SneakWalkSpeed => _sneakWalkSpeed;
        public float SneakRunSpeed => _sneakRunSpeed;
        public float JumpHorizontalForce => _jumpHorizontalForce;
        public float JumpVerticalForce => _jumpVerticalForce;
        public float GroundCheckHeight => _groundCheckHeight;
        public float GroundCHeckHeightReduction => _groundCheckHeightReduction;
        public float AccelerationLag => _accelerationLag;
        public float DecelerationLag => _decelerationLag;
        public float InBattleAccelerationLag => _inBattleAccelerationLag;
        public float InBattleDecelerationLag => _inBattleDecelerationLag;
        public float SneakAccelerationLag => _sneakAccelerationLag;
        public float SneakDecelerationLag => _sneakDecelerationLag;
        public float DirectionChangeLag => _directionChangeLag;
        public float AnimatorBaseSpeed => _animatorBaseSpeed;

        public float TargetMarkHeight => _targetMarkHeight;
        public float BattleIgnoreTime => _battleIgnoreTime;
        public string TargetMarkName => _targetMarkName;

        public float LeftFootHitBoxRadius => _leftFootHitBoxRadius;
        public float RightFootHitBoxRadius => _rightFootHitBoxRadius;

        public float RollTime => _rollTime;
        public float RollFrameDistance => _rollFrameDistance;
        public float RollAnimationSpeed => _rollAnimationSpeed;

        #endregion


        #region Methods

        public GameObject CreateTargetMark(Transform characterTransform, Vector3 basePosition)
        {
            GameObject targetMark = GameObject.Instantiate(TargetPrefab);
            targetMark.name = TargetMarkName;

            targetMark.transform.SetParent(characterTransform);
            targetMark.transform.localPosition = Vector3.zero;
            targetMark.transform.localRotation = Quaternion.Euler(0, 0, 0);
            targetMark.transform.localPosition = Vector3.zero;
            targetMark.transform.localEulerAngles = new Vector3(90, 0, 0);
            targetMark.SetActive(false);

            return targetMark;
        }

        public void SetTargetMarkBasePosition(Transform characterTransform, Transform targetMarkTransform)
        {
            targetMarkTransform.localPosition = Vector3.zero;
            targetMarkTransform.gameObject.SetActive(false);
        }

        #endregion
    }
}
