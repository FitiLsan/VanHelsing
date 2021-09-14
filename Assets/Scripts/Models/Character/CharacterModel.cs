using UnityEngine;
using RootMotion.Dynamics;
using Extensions;
using UniRx;
using System;

namespace BeastHunter
{
    public sealed class CharacterModel: BaseModel
    {
        #region Fields

        private Collider _closestEnemy;
        public event Action StartControlEvent;
        public event Action StopControlEvent;

        #endregion


        #region Properties

        //  public Stats CurrentStats { get; set; }
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
        public Transform ObjectOnSceneTransform { get; }
        public Transform CharacterTransform { get; }
        public CapsuleCollider CharacterCapsuleCollider { get; }
        public SphereCollider CharacterSphereCollider { get; }
        public Rigidbody CharacterRigitbody { get; }
        public PlayerBehavior PlayerBehavior { get; }
        public CharacterData CharacterData { get; }
        public CharacterCommonSettings CharacterCommonSettings { get; }
        public Stats CharacterStartStats { get; }
        public PuppetMaster PuppetMaster { get; }
        public BehaviourPuppet BehaviorPuppet { get; }
        public BehaviourFall BehaviorFall { get; }

        public ReactiveCollection<Collider> EnemiesInTrigger { get; set; }
        public ReactiveProperty<Collider> ClosestEnemy { get; set; }

        public float CurrentSpeed { get; set; }
        public float AnimationSpeed { get; set; }

        public bool IsDodging { get; set; }
        public bool IsSneaking { get; set; }
        public bool IsGrounded { get; set; }

        public bool IsEnemyNear { get; set; }
        public bool IsInHidingPlace { get; set; }
        public bool IsWeaponInHands
        {
            get
            {
                return CurrentWeaponLeft != null || CurrentWeaponRight != null;
            }
        }

        public int InstanceID { get; }
        public GameObject BuffEffectPrefab { get; private set; }

        #endregion


        #region ClassLifeCycle

        public CharacterModel(GameObject objectOnScene, CharacterData characterData, LocationPosition groundedPosition)
        {
            CharacterData = characterData;
            CharacterCommonSettings = CharacterData.CharacterCommonSettings;
            CharacterStartStats = CharacterData.CharacterStatsSettings;
            ObjectOnSceneTransform = objectOnScene.transform;
            CharacterTransform = objectOnScene.transform.GetChild(2).transform;
            ObjectOnSceneTransform.position = groundedPosition.Position;
            ObjectOnSceneTransform.eulerAngles = groundedPosition.Eulers;
            ObjectOnSceneTransform.localScale = groundedPosition.Scale;
            CharacterTransform.rotation = Quaternion.Euler(0, CharacterCommonSettings.InstantiateDirection, 0);
            CharacterTransform.name = CharacterCommonSettings.InstanceName;
            CharacterTransform.tag = CharacterCommonSettings.InstanceTag;
            CurrentStats = CharacterStartStats.DeepCopy();
            InstanceID = CurrentStats.InstanceID = objectOnScene.GetInstanceID();
            CurrentStats.BuffHolder = new BuffHolder();
            BuffEffectPrefab = CharacterTransform.Find("Effects").gameObject;


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


            PuppetMaster = objectOnScene.transform.GetChild(1).gameObject.GetComponent<PuppetMaster>();
            BehaviorPuppet = objectOnScene.transform.GetChild(0).GetChild(0).gameObject.GetComponent<BehaviourPuppet>();
            BehaviorFall = objectOnScene.transform.GetChild(0).GetChild(1).gameObject.GetComponent<BehaviourFall>();

            EnemiesInTrigger = new ReactiveCollection<Collider>();

            ClosestEnemy = new ReactiveProperty<Collider>();
            IsGrounded = false;
            IsSneaking = false;
            IsEnemyNear = false;
            IsInHidingPlace = false;

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

        public void StartControl()
        {
            StartControlEvent?.Invoke();
        }

        public void StopControl()
        {
            StopControlEvent?.Invoke();
        }
    }
}
