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
        public CinemachineBrain CameraCinemachineBrain { get; }
        public GameObject CameraTarget { get; }
        public Transform CameraTargetTransform { get; }
        public CapsuleCollider CharacterCapsuleCollider { get; }
        public SphereCollider CharacterSphereCollider { get; }
        public Transform CharacterTransform { get; }
        public Rigidbody CharacterRigitbody { get; }
        public PlayerBehavior PlayerBehaviour { get; }
        public CharacterData CharacterData { get; }
        public CharacterCommonSettingsStruct CharacterCommonSettings { get; }
        public CharacterCameraStruct CharacterCameraSettings { get; }
        public Animator CharacterAnimator { get; set; }
        public PlayerHitBoxBehavior[] PlayerHitBoxes { get; set; }
        public List<Collider> EnemiesInTrigger { get; set; }

        public float CurrentSpeed { get; set; }
        public float VerticalSpeed { get; set; }
        public float AnimationSpeed { get; set; }
        public bool IsMoving { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsInBattleMode { get; set; }
        public bool IsEnemyNear { get; set; }
        public bool IsAxisInputsLocked { get; set; }

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
            CameraCinemachineBrain = CharacterCamera.GetComponent<CinemachineBrain>() ?? null;
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

            CharacterAnimator.runtimeAnimatorController = CharacterCommonSettings.CharacterAnimator;
            CharacterAnimator.applyRootMotion = false;

            if (prefab.GetComponent<PlayerBehavior>() != null)
            {
                PlayerBehaviour = prefab.GetComponent<PlayerBehavior>();
            }
            else
            {
                PlayerBehaviour = prefab.AddComponent<PlayerBehavior>();
            }

            PlayerBehaviour.SetType(InteractableObjectType.Player);

            EnemiesInTrigger = new List<Collider>();
            IsMoving = false;
            IsGrounded = false;
            IsEnemyNear = false;
            IsInBattleMode = false;
            IsAxisInputsLocked = false;
            CurrentSpeed = 0;
            AnimationSpeed = CharacterData._characterCommonSettings.AnimatorBaseSpeed;

            PlayerHitBoxes = new PlayerHitBoxBehavior[3];

            string[] hitBoxesPaths = new string[3]
            {
                CharacterCommonSettings.FirstHitBoxObjectPath,
                CharacterCommonSettings.SecondHitBoxObjectPath,
                CharacterCommonSettings.ThirdHitBoxObjectPath
            };

            float[] hitBoxesRadiuses = new float[3]
{
                CharacterCommonSettings.FirstHitBoxRadius,
                CharacterCommonSettings.SecondHitBoxRadius,
                CharacterCommonSettings.ThirdHitBoxRadius
            };

            for (int hitBox = 0; hitBox < PlayerHitBoxes.Length; hitBox++)
            {
                Transform hitBoxTransform = CharacterTransform.Find(hitBoxesPaths[hitBox]);

                if (hitBoxTransform != null && hitBoxTransform.GetComponent<Collider>() == null)
                {
                    SphereCollider trigger = hitBoxTransform.gameObject.AddComponent<SphereCollider>();
                    trigger.radius = hitBoxesRadiuses[hitBox];
                    trigger.isTrigger = true;
                }
                else if (hitBoxTransform.GetComponent<SphereCollider>() != null)
                {
                    SphereCollider trigger = hitBoxTransform.GetComponent<SphereCollider>();
                    trigger.radius = hitBoxesRadiuses[hitBox];
                    trigger.isTrigger = true;
                }

                PlayerHitBoxes[hitBox] = hitBoxTransform.gameObject.AddComponent<PlayerHitBoxBehavior>();
                PlayerHitBoxes[hitBox].SetType(InteractableObjectType.HitBox);
                PlayerHitBoxes[hitBox].IsInteractable = false;
                hitBoxTransform.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            }

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
