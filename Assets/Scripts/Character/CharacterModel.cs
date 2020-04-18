using UnityEngine;
using UnityEditor;
using Cinemachine;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class CharacterModel
    {
        #region Properties

        public Camera CharacterCamera { get; }
        public CinemachineFreeLook CharacterFreelookCamera { get; }
        public CinemachineVirtualCamera CharacterTargetCamera { get; }
        public GameObject CameraTarget { get; }
        public Transform CameraTargetTransform { get; }
        public CapsuleCollider CharacterCapsuleCollider { get; }
        public SphereCollider CharacterSphereCollider { get; }
        public Transform CharacterTransform { get; }
        public Rigidbody CharacterRigitbody { get; }
        public PlayerBehaviour PlayerBehaviour { get; }
        public CharacterData CharacterData { get; }
        public CharacterCommonSettingsStruct CharacterCommonSettings { get; }
        public CharacterCameraStruct CharacterCameraSettings { get; }
        public Animator CharacterAnimator { get; set; }
        public List<Collider> EnemiesInTrigger { get; set; }

        public float CurrentSpeed { get; set; }
        public float VerticalSpeed { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsEnemyNear { get; set; }
        public bool IsTargeting { get; set; }
        public bool IsInBattleMode { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsCameraFixed { get; set; }
        public bool IsDead { get; set; }
        public int AttackIndex { get; set; }

        #endregion


        #region ClassLifeCycle

        public CharacterModel(GameObject prefab, CharacterData characterData, Vector3 groundPosition)
        {
            CharacterData = characterData;
            CharacterCommonSettings = CharacterData._characterCommonSettings;
            CharacterCameraSettings = CharacterData._characterCameraSettings;
            CharacterTransform = prefab.transform;
            CharacterTransform.rotation = Quaternion.Euler(0, CharacterCommonSettings.InstantiateDirection, 0);
            CharacterTransform.name = CharacterCommonSettings.InstanceName;
            CharacterTransform.tag = CharacterCommonSettings.InstanceTag;

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

            CameraTarget = CharacterCameraSettings.CreateCameraTarget(CharacterTransform);
            CameraTargetTransform = CameraTarget.transform;
            CharacterCamera = CharacterCameraSettings.CreateCharacterCamera();
            CharacterCamera.transform.rotation = Quaternion.Euler(0, CharacterCommonSettings.InstantiateDirection, 0);
            CharacterFreelookCamera = CharacterCameraSettings.CreateCharacterFreelookCamera(CameraTargetTransform);
            CharacterTargetCamera = CharacterCameraSettings.CreateCharacterTargetCamera(CameraTargetTransform);

            if (prefab.GetComponent<Animator>() != null)
            {
                CharacterAnimator = prefab.GetComponent<Animator>();
            }
            else
            {
                CharacterAnimator = prefab.AddComponent<Animator>();
            }

            CharacterAnimator.runtimeAnimatorController = CharacterCommonSettings.CharacterDefaultMovementAnimator;
            CharacterAnimator.applyRootMotion = false;

            if (prefab.GetComponent<PlayerBehaviour>() != null)
            {
                PlayerBehaviour = prefab.GetComponent<PlayerBehaviour>();
            }
            else
            {
                PlayerBehaviour = prefab.AddComponent<PlayerBehaviour>();
            }

            PlayerBehaviour.SetType(InteractableObjectType.Player);

            EnemiesInTrigger = new List<Collider>();
            IsGrounded = false;
            IsCameraFixed = false;
            IsEnemyNear = false;
            IsTargeting = false;
            IsInBattleMode = false;
            IsAttacking = false;
            IsDead = false;
            CurrentSpeed = 0;
            AttackIndex = 0;

#if (UNITY_EDITOR)
            EditorApplication.playModeStateChanged += SaveCameraSettings;
#endif
        }

        #endregion

        #region Methods

#if (UNITY_EDITOR)
        private void SaveCameraSettings(PlayModeStateChange state)
        {
            if(state == PlayModeStateChange.ExitingPlayMode)
            {
                Data.CharacterData._characterCameraSettings.
                    SaveCameraSettings(CharacterFreelookCamera, CharacterTargetCamera);
                EditorApplication.playModeStateChanged -= SaveCameraSettings;
            }
        }
#endif
        #endregion
    }
}
