using UnityEngine;
using System.Collections.Generic;
using RootMotion.Dynamics;
using Extensions;
using UniRx;


namespace BeastHunter
{
    public sealed class CharacterModel
    {
        #region Fields

        private Collider _closestEnemy;

        #endregion


        #region Properties

        public int InstanceID { get; }
        public BaseStatsClass CharacterStats { get; set; }
        public ReactiveProperty<WeaponData> CurrentWeaponData { get; set; }
        public GameObject CurrentWeaponLeft { get; set; }
        public GameObject CurrentWeaponRight { get; set; }
        public WeaponHitBoxBehavior WeaponBehaviorLeft { get; set; }
        public WeaponHitBoxBehavior WeaponBehaviorRight { get; set; }
        public ReactiveProperty<TrapModel> CurrentPlacingTrapModel { get; set; }
        public ReactiveProperty<CharacterBaseState> CurrentCharacterState { get; set; }
        public ReactiveProperty<CharacterBaseState> PreviousCharacterState { get; set; }

        public AudioSource SpeechAudioSource { get; }
        public AudioSource MovementAudioSource { get; }
        public CharacterAnimationModel CharacterAnimationModel { get; }
        public Transform CharacterTransform { get; }
        public CapsuleCollider CharacterCapsuleCollider { get; }
        public SphereCollider CharacterSphereCollider { get; }
        public Rigidbody CharacterRigitbody { get; }
        public PlayerBehavior PlayerBehavior { get; }
        public CharacterData CharacterData { get; }
        public CharacterCommonSettingsStruct CharacterCommonSettings { get; }
        public BaseStatsClass CharacterStatsSettings { get; }
        public PuppetMaster PuppetMaster { get; }
        public BehaviourPuppet BehaviorPuppet { get; }
        public BehaviourFall BehaviorFall { get; }
        public LineRenderer ProjectileTrajectoryPredict { get; }

        public ReactiveCollection<Collider> EnemiesInTrigger { get; set; }
        public ReactiveProperty<Collider> ClosestEnemy { get; set; }

        public float CurrentSpeed { get; set; }
        public float AnimationSpeed { get; set; }

        public bool IsDodging { get; set; }
        public bool IsSneaking { get; set; }
        public bool IsGrounded { get; set; }

        public bool IsEnemyNear { get; set; }
        public bool IsInHidingPlace { get; set; }
        public bool IsDead { get; set; }
        public bool IsWeaponInHands
        {
            get
            {
                return CurrentWeaponLeft != null || CurrentWeaponRight != null;
            }
        }

        #endregion


        #region ClassLifeCycle

        public CharacterModel(GameObject prefab, CharacterData characterData, Vector3 groundPosition)
        {
            InstanceID = prefab.GetInstanceID();
            CharacterData = characterData;
            CharacterCommonSettings = CharacterData.CharacterCommonSettings;
            CharacterStatsSettings = CharacterData.CharacterStatsSettings;
            CharacterTransform = prefab.transform.GetChild(2).transform;
            CharacterTransform.position = groundPosition;
            CharacterTransform.rotation = Quaternion.Euler(0, CharacterCommonSettings.InstantiateDirection, 0);
            CharacterTransform.name = CharacterCommonSettings.InstanceName;
            CharacterTransform.tag = CharacterCommonSettings.InstanceTag;

            CharacterStats = CharacterStatsSettings;

            AudioSource[] characterAudioSources = CharacterTransform.gameObject.GetComponentsInChildren<AudioSource>();
            SpeechAudioSource = characterAudioSources[0];
            MovementAudioSource = characterAudioSources[1];

            if (CharacterTransform.gameObject.TryGetComponent(out Rigidbody _characterRigitbody))
            {
                CharacterRigitbody = _characterRigitbody;
            }
            else
            {
                throw new System.Exception("There is no rigidbody on character prefab");
            }

            CharacterRigitbody.mass = CharacterCommonSettings.RigitbodyMass;
            CharacterRigitbody.drag = CharacterCommonSettings.RigitbodyDrag;
            CharacterRigitbody.angularDrag = CharacterCommonSettings.RigitbodyAngularDrag;
            CharacterRigitbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            if (CharacterTransform.gameObject.TryGetComponent(out CapsuleCollider _characterCapsuleCollider))
            {
                CharacterCapsuleCollider = _characterCapsuleCollider;
            }
            else
            {
                throw new System.Exception("There is no capsule collider on character prefab");
            }
            CharacterCapsuleCollider.center = CharacterCommonSettings.CapsuleColliderCenter;
            CharacterCapsuleCollider.radius = CharacterCommonSettings.CapsuleColliderRadius;
            CharacterCapsuleCollider.height = CharacterCommonSettings.CapsuleColliderHeight;
            CharacterCapsuleCollider.material = CharacterCommonSettings.CapsuleColliderPhysicMaterial;

            if (CharacterTransform.gameObject.TryGetComponent(out SphereCollider _characterSphereCollider))
            {
                CharacterSphereCollider = _characterSphereCollider;
            }
            else
            {
                throw new System.Exception("There is no sphere collider on character prefab");
            }
            CharacterSphereCollider.center = CharacterCommonSettings.SphereColliderCenter;
            CharacterSphereCollider.radius = CharacterCommonSettings.SphereColliderRadius;
            CharacterSphereCollider.isTrigger = true;

            if (CharacterTransform.gameObject.TryGetComponent(out PlayerBehavior playerBehavior))
            {
                PlayerBehavior = playerBehavior;
            }
            else
            {
                throw new System.Exception("There is no player behavior script on character prefab");
            }

            if (CharacterTransform.gameObject.TryGetComponent(out LineRenderer projectileProjection))
            {
                ProjectileTrajectoryPredict = projectileProjection;
            }
            else
            {
                throw new System.Exception("There is no line renderer on character prefab");
            }

            PuppetMaster = prefab.transform.GetChild(1).gameObject.GetComponent<PuppetMaster>();
            BehaviorPuppet = prefab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<BehaviourPuppet>();
            BehaviorFall = prefab.transform.GetChild(0).GetChild(1).gameObject.GetComponent<BehaviourFall>();

            EnemiesInTrigger = new ReactiveCollection<Collider>();
            
            ClosestEnemy = new ReactiveProperty<Collider>();
            IsGrounded = false;
            IsSneaking = false;
            IsEnemyNear = false;
            IsInHidingPlace = false;
            IsDead = false;

            CurrentSpeed = 0;
            AnimationSpeed = CharacterData.CharacterCommonSettings.AnimatorBaseSpeed;

            CurrentWeaponData = new ReactiveProperty<WeaponData>();
            CurrentWeaponData.Value = null;
            CurrentWeaponLeft = null;
            CurrentWeaponRight = null;
            CurrentPlacingTrapModel = new ReactiveProperty<TrapModel>();
            CurrentPlacingTrapModel.Value = null;
            CharacterAnimationModel = new CharacterAnimationModel(CharacterTransform.GetComponent<Animator>(), 
                CharacterCommonSettings.CharacterAnimator, CharacterCommonSettings.BeginningApplyRootMotion);
            CurrentCharacterState = new ReactiveProperty<CharacterBaseState>();
            PreviousCharacterState = new ReactiveProperty<CharacterBaseState>();

            Services.SharedInstance.CameraService.Initialize(this);
            Services.SharedInstance.AudioService.Initialize(this);
        }

        #endregion
    }
}
