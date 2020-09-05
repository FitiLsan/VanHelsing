using UnityEngine;
using System.Collections.Generic;
using RootMotion.Dynamics;


namespace BeastHunter
{
    public sealed class CharacterModel
    {
        #region Properties

        public GameObject CurrentWeapon { get; set; }
        public WeaponItem CurrentWeaponItem { get; set; }



        public GameObject LeftHandWeaponObject { get; set; }
        public GameObject RightHandWeaponObject { get; set; }

        public Transform LeftHand { get; }
        public Transform RightHand { get; }
        public Transform LeftFoot { get; }
        public Transform RightFoot { get; }
        public Transform CharacterTransform { get; }

        public Vector3 LeftFootPosition { get; set; }
        public Vector3 RightFootPosition { get; set; }
        public Vector3 LeftFootRotation { get; set; }
        public Vector3 RightFootRotation { get; set; }

        public WeaponHitBoxBehavior LeftHandBehavior { get; }
        public WeaponHitBoxBehavior RightHandBehavior { get; }
        public WeaponHitBoxBehavior LeftWeaponBehavior { get; set; }
        public WeaponHitBoxBehavior RightWeaponBehavior { get; set; } 

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

        public Animator CharacterAnimator { get; set; }
        public List<Collider> EnemiesInTrigger { get; set; }
        public Collider ClosestEnemy { get; set; }

        public WeaponItem LeftHandFeast { get; set; }
        public WeaponItem RigheHandWeapon { get; set; }
        public WeaponItem LeftHandWeapon { get; set; }
        public WeaponItem RightHandWeapon { get; set; }

        public float CurrentSpeed { get; set; }
        public float AnimationSpeed { get; set; }
        public float Health { get; set; }

        public bool IsDodging { get; set; }
        public bool IsSneaking { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsInBattleMode { get; set; }
        public bool IsEnemyNear { get; set; }
        public bool IsWeaponInHands { get; set; }
        public bool IsDead { get; set; }

        #endregion


        #region ClassLifeCycle

        public CharacterModel(GameObject prefab, CharacterData characterData, Vector3 groundPosition)
        {
            CharacterData = characterData;
            CharacterCommonSettings = CharacterData._characterCommonSettings;
            CharacterStatsSettings = CharacterData._characterStatsSettings;
            CharacterTransform = prefab.transform.GetChild(2).transform;
            CharacterTransform.rotation = Quaternion.Euler(0, CharacterCommonSettings.InstantiateDirection, 0);
            CharacterTransform.name = CharacterCommonSettings.InstanceName;
            CharacterTransform.tag = CharacterCommonSettings.InstanceTag;

            if (CharacterTransform.GetComponent<Rigidbody>() != null)
            {
                CharacterRigitbody = CharacterTransform.GetComponent<Rigidbody>();
            }
            else
            {
                CharacterRigitbody = CharacterTransform.gameObject.AddComponent<Rigidbody>();
                CharacterRigitbody.freezeRotation = true;
                CharacterRigitbody.mass = CharacterCommonSettings.RigitbodyMass;
                CharacterRigitbody.drag = CharacterCommonSettings.RigitbodyDrag;
                CharacterRigitbody.angularDrag = CharacterCommonSettings.RigitbodyAngularDrag;
            }

            CharacterRigitbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            if (CharacterTransform.GetComponent<CapsuleCollider>() != null)
            {
                CharacterCapsuleCollider = CharacterTransform.GetComponent<CapsuleCollider>();
            }
            else
            {
                CharacterCapsuleCollider = CharacterTransform.gameObject.AddComponent<CapsuleCollider>();
                CharacterCapsuleCollider.center = CharacterCommonSettings.CapsuleColliderCenter;
                CharacterCapsuleCollider.radius = CharacterCommonSettings.CapsuleColliderRadius;
                CharacterCapsuleCollider.height = CharacterCommonSettings.CapsuleColliderHeight;
            }

            CharacterCapsuleCollider.transform.position = groundPosition;

            if (CharacterTransform.GetComponent<SphereCollider>() != null)
            {
                CharacterSphereCollider = CharacterTransform.GetComponent<SphereCollider>();
                CharacterSphereCollider.isTrigger = true;
            }
            else
            {
                CharacterSphereCollider = CharacterTransform.gameObject.AddComponent<SphereCollider>();
                CharacterSphereCollider.center = CharacterCommonSettings.SphereColliderCenter;
                CharacterSphereCollider.radius = CharacterCommonSettings.SphereColliderRadius;
                CharacterSphereCollider.isTrigger = true;
            }

            CharacterAnimator = prefab.transform.GetChild(2).GetComponent<Animator>();
            PuppetMaster = prefab.transform.GetChild(1).gameObject.GetComponent<PuppetMaster>();
            BehaviorPuppet = prefab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<BehaviourPuppet>();
            BehaviorFall = prefab.transform.GetChild(0).GetChild(1).gameObject.GetComponent<BehaviourFall>();

            CharacterAnimator.runtimeAnimatorController = CharacterCommonSettings.CharacterAnimator;
            CharacterAnimator.applyRootMotion = false;

            if (prefab.GetComponent<PlayerBehavior>() != null)
            {
                PlayerBehavior = prefab.GetComponent<PlayerBehavior>();
            }
            else
            {
                PlayerBehavior = prefab.AddComponent<PlayerBehavior>();
            }

            PlayerBehavior.SetType(InteractableObjectType.Player);
            PlayerBehavior.Stats = CharacterStatsSettings;

            EnemiesInTrigger = new List<Collider>();
            ClosestEnemy = null;
            IsGrounded = false;
            IsSneaking = false;
            IsEnemyNear = false;
            IsInBattleMode = false;
            IsWeaponInHands = false;
            IsDead = false;

            CurrentSpeed = 0;
            AnimationSpeed = CharacterData._characterCommonSettings.AnimatorBaseSpeed;

            LeftHand = CharacterAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
            RightHand = CharacterAnimator.GetBoneTransform(HumanBodyBones.RightHand);
            LeftFoot = CharacterAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
            RightFoot = CharacterAnimator.GetBoneTransform(HumanBodyBones.RightFoot);

            LeftHandWeapon = null;
            RightHandWeapon = null;
            LeftHandWeaponObject = null;
            RightHandWeaponObject = null;

            Services.SharedInstance.CameraService.Initialize(this);

            CurrentWeapon = null;
            CurrentWeaponItem = null;
        }

        #endregion
    }
}
