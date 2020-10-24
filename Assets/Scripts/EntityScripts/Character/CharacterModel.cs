using UnityEngine;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class CharacterModel
    {
        #region Properties

        public GameObject CameraTarget { get; }
        public GameObject TargetMark { get; }
        public GameObject LeftHandWeaponObject { get; set; }
        public GameObject RightHandWeaponObject { get; set; }

        public Transform LeftHand { get; }
        public Transform RightHand { get; }
        public Transform LeftFoot { get; }
        public Transform RightFoot { get; }
        public Transform CameraTargetTransform { get; }
        public Transform TargetMarkTransform { get; }
        public Transform CharacterTransform { get; }

        public WeaponHitBoxBehavior LeftFootBehavior { get; }
        public WeaponHitBoxBehavior RightFootBehavior { get; }
        public WeaponHitBoxBehavior LeftWeaponBehavior { get; set; }
        public WeaponHitBoxBehavior RightWeaponBehavior { get; set; } 

        public Vector3 TargetMarkBasePosition { get; }
        public CapsuleCollider CharacterCapsuleCollider { get; }
        public SphereCollider CharacterSphereCollider { get; }
        public Rigidbody CharacterRigitbody { get; }
        public PlayerBehavior PlayerBehavior { get; }
        public CharacterData CharacterData { get; }
        public CharacterCommonSettingsStruct CharacterCommonSettings { get; }
        public BaseStatsClass CharacterStatsSettings { get; }

        public Animator CharacterAnimator { get; set; }
        public List<Collider> EnemiesInTrigger { get; set; }
        public Collider ClosestEnemy { get; set; }
        public WeaponItem LeftHandWeapon { get; set; }
        public WeaponItem RightHandWeapon { get; set; }

        public float CurrentSpeed { get; set; }
        public float VerticalSpeed { get; set; }
        public float AnimationSpeed { get; set; }

        public bool IsMoving { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsInBattleMode { get; set; }
        public bool IsEnemyNear { get; set; }
        public bool IsWeaponInHands { get; set; }

        #endregion


        #region ClassLifeCycle

        public CharacterModel(GameObject prefab, CharacterData characterData, Vector3 groundPosition)
        {
            CharacterData = characterData;
            CharacterCommonSettings = CharacterData._characterCommonSettings;
            CharacterStatsSettings = CharacterData._characterStatsSettings;
            CharacterTransform = prefab.transform;
            CharacterTransform.rotation = Quaternion.Euler(0, CharacterCommonSettings.InstantiateDirection, 0);
            CharacterTransform.name = CharacterCommonSettings.InstanceName;
            CharacterTransform.tag = CharacterCommonSettings.InstanceTag;
            CharacterTransform.gameObject.layer = CharacterCommonSettings.InstanceLayer;

            Transform[] children = CharacterTransform.GetComponentsInChildren<Transform>();

            foreach (var child in children)
            {
                child.gameObject.layer = CharacterCommonSettings.InstanceLayer;
            }

            if (prefab.GetComponent<Rigidbody>() != null)
            {
                CharacterRigitbody = prefab.GetComponent<Rigidbody>();
            }
            else
            {
                CharacterRigitbody = prefab.AddComponent<Rigidbody>();
                CharacterRigitbody.freezeRotation = true;
                CharacterRigitbody.mass = CharacterCommonSettings.RigitbodyMass;
                CharacterRigitbody.drag = CharacterCommonSettings.RigitbodyDrag;
                CharacterRigitbody.angularDrag = CharacterCommonSettings.RigitbodyAngularDrag;
            }

            CharacterRigitbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            if (prefab.GetComponent<CapsuleCollider>() != null)
            {
                CharacterCapsuleCollider = prefab.GetComponent<CapsuleCollider>();
            }
            else
            {
                CharacterCapsuleCollider = prefab.AddComponent<CapsuleCollider>();
                CharacterCapsuleCollider.center = CharacterCommonSettings.CapsuleColliderCenter;
                CharacterCapsuleCollider.radius = CharacterCommonSettings.CapsuleColliderRadius;
                CharacterCapsuleCollider.height = CharacterCommonSettings.CapsuleColliderHeight;
            }

            CharacterCapsuleCollider.transform.position = groundPosition;

            if (prefab.GetComponent<SphereCollider>() != null)
            {
                CharacterSphereCollider = prefab.GetComponent<SphereCollider>();
                CharacterSphereCollider.isTrigger = true;
            }
            else
            {
                CharacterSphereCollider = prefab.AddComponent<SphereCollider>();
                CharacterSphereCollider.center = CharacterCommonSettings.SphereColliderCenter;
                CharacterSphereCollider.radius = CharacterCommonSettings.SphereColliderRadius;
                CharacterSphereCollider.isTrigger = true;
            }

            CameraTarget = Services.SharedInstance.CameraService.CreateCameraTarget(CharacterTransform);
            CameraTargetTransform = CameraTarget.transform;

            TargetMarkBasePosition = new Vector3(CharacterTransform.localPosition.x,
                CharacterTransform.localPosition.y + CharacterCapsuleCollider.height +
                    CharacterCommonSettings.TargetMarkHeight, CharacterTransform.localPosition.z);

            TargetMark = CharacterCommonSettings.CreateTargetMark(CharacterTransform, TargetMarkBasePosition);
            TargetMarkTransform = TargetMark.transform;


            if (prefab.GetComponent<Animator>() != null)
            {
                CharacterAnimator = prefab.GetComponent<Animator>();
            }
            else
            {
                CharacterAnimator = prefab.AddComponent<Animator>();
            }

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
            IsMoving = false;
            IsGrounded = false;
            IsEnemyNear = false;
            IsInBattleMode = false;
            IsWeaponInHands = false;
            CurrentSpeed = 0;
            AnimationSpeed = CharacterData._characterCommonSettings.AnimatorBaseSpeed;

            LeftHand = CharacterTransform.Find(CharacterCommonSettings.LeftHandObjectPath);
            RightHand = CharacterTransform.Find(CharacterCommonSettings.RightHandObjectPath);
            LeftFoot = CharacterTransform.Find(CharacterCommonSettings.LeftFootObjectPath);
            RightFoot = CharacterTransform.Find(CharacterCommonSettings.RightFootObjectPath);

            //SphereCollider LeftFootTrigger = LeftFoot.gameObject.AddComponent<SphereCollider>();
            //LeftFootTrigger.radius = CharacterCommonSettings.LeftFootHitBoxRadius;
            //LeftFootTrigger.isTrigger = true;
            //LeftFoot.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            //LeftFootBehavior = LeftFoot.gameObject.AddComponent<WeaponHitBoxBehavior>();
            //LeftFootBehavior.SetType(InteractableObjectType.HitBox);
            //LeftFootBehavior.IsInteractable = false;

            //SphereCollider RightFootTrigger = RightFoot.gameObject.AddComponent<SphereCollider>();
            //RightFootTrigger.radius = CharacterCommonSettings.RightFootHitBoxRadius;
            //RightFootTrigger.isTrigger = true;
            //RightFoot.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            //RightFootBehavior = RightFoot.gameObject.AddComponent<WeaponHitBoxBehavior>();
            //RightFootBehavior.SetType(InteractableObjectType.HitBox);
            //RightFootBehavior.IsInteractable = false;

            LeftHandWeapon = null;
            RightHandWeapon = null;
            LeftHandWeaponObject = null;
            RightHandWeaponObject = null;

            Services.SharedInstance.CameraService.Initialise(this);
        }

        #endregion
    }
}
