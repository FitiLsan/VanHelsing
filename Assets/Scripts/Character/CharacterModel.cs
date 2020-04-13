using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace BeastHunter
{
    public sealed class CharacterModel
    {
        #region Properties

        public Camera CharacterCamera { get; }
        public CinemachineFreeLook CharacterCinemachineCamera { get; }
        public CinemachineVirtualCamera CharacterCinemachineTargetCamera { get; }
        public CapsuleCollider CharacterCapsuleCollider { get; }
        public SphereCollider CharacterSphereCollider { get; }
        public Transform CharacterTransform { get; }
        public Rigidbody CharacterRigitbody { get; }
        public GameObject CameraTarget { get; }
        public Transform CameraTargetTransform { get; }
        public CharacterData CharacterData { get; }
        public CharacterStruct CharacterStruct { get; }
        public Animator CharacterAnimator { get; }
        public PlayerBehaviour PlayerBehaviour { get; }
        public List<Collider> _collidersInTrigger { get; }

        #endregion


        #region ClassLifeCycle

        public CharacterModel(GameObject prefab, CharacterData characterData, Vector3 groundPosition)
        {
            CharacterData = characterData;
            CharacterStruct = CharacterData._characterStruct;
            CharacterTransform = prefab.transform;
            CharacterTransform.rotation = Quaternion.Euler(0, CharacterStruct.InstantiateDirection, 0);
            CharacterTransform.name = "Player";
            CharacterTransform.tag = "Player";

            if (prefab.GetComponent<Rigidbody>() != null)
            {
                CharacterRigitbody = prefab.GetComponent<Rigidbody>();
            }
            else
            {
                CharacterRigitbody = prefab.AddComponent<Rigidbody>();
                CharacterRigitbody.freezeRotation = true;
                CharacterRigitbody.mass = CharacterStruct.RigitbodyMass;
                CharacterRigitbody.drag = CharacterStruct.RigitbodyDrag;
                CharacterRigitbody.angularDrag = CharacterStruct.RigitbodyAngularDrag;
            }

            if (prefab.GetComponent<CapsuleCollider>() != null)
            {
                CharacterCapsuleCollider = prefab.GetComponent<CapsuleCollider>();
            }
            else
            {
                CharacterCapsuleCollider = prefab.AddComponent<CapsuleCollider>();
                CharacterCapsuleCollider.center = CharacterStruct.CapsuleColliderCenter;
                CharacterCapsuleCollider.radius = CharacterStruct.CapsuleColliderRadius;
                CharacterCapsuleCollider.height = CharacterStruct.CapsuleColliderHeight;
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
                CharacterSphereCollider.center = CharacterStruct.SphereColliderCenter;
                CharacterSphereCollider.radius = CharacterStruct.SphereColliderRadius;
                CharacterSphereCollider.isTrigger = true;
            }

            CharacterCamera = GameObject.Instantiate(CharacterStruct.Camera);
            CharacterCamera.name = "CharacterCamera";
            CharacterCinemachineCamera = GameObject.Instantiate(CharacterStruct.CinemachineCamera);
            CharacterCinemachineCamera.name = "CharacterCinemachineCamera";
            CharacterCinemachineTargetCamera = GameObject.Instantiate(CharacterStruct.CinemachineTargetCamera);
            CharacterCinemachineTargetCamera.name = "CharacterCinemachineTargetCamera";

            CameraTarget = new GameObject{ name = "CameraTarget" };

            CameraTarget.transform.SetParent(CharacterTransform);
            CameraTargetTransform = CameraTarget.transform;
            CameraTargetTransform.localPosition = Vector3.zero;
            CameraTargetTransform.localRotation = Quaternion.Euler(0, 0, 0);

            CharacterCinemachineCamera.Follow = CameraTargetTransform;
            CharacterCinemachineCamera.LookAt = CameraTargetTransform;

            CharacterCamera.transform.rotation = Quaternion.Euler(0, CharacterStruct.InstantiateDirection, 0);

            CharacterCinemachineCamera.m_XAxis.m_InvertAxis = CharacterStruct.IsInvertedX;
            CharacterCinemachineCamera.m_XAxis.m_MaxSpeed = CharacterStruct.CameraSpeedX;
            CharacterCinemachineCamera.m_XAxis.m_AccelTime = CharacterStruct.CameraAccelerationLagX;
            CharacterCinemachineCamera.m_XAxis.m_DecelTime = CharacterStruct.CameraDecelerationLagX;

            CharacterCinemachineCamera.m_YAxis.m_InvertAxis = CharacterStruct.IsInvertedY;
            CharacterCinemachineCamera.m_YAxis.m_MaxSpeed = CharacterStruct.CameraSpeedY;
            CharacterCinemachineCamera.m_YAxis.m_AccelTime = CharacterStruct.CameraAccelerationLagY;
            CharacterCinemachineCamera.m_YAxis.m_DecelTime = CharacterStruct.CameraDecelerationLagY;

            for (var rig = 0; rig < 3; rig++)
            {
                CinemachineVirtualCamera cinemachineRig = CharacterCinemachineCamera.GetRig(rig);
                cinemachineRig.LookAt = CameraTargetTransform;

                CinemachineComposer composerFromData = CharacterStruct.GetComposer(rig);
                CinemachineComposer composerFromCamera = cinemachineRig.GetCinemachineComponent<CinemachineComposer>();

                composerFromCamera.m_TrackedObjectOffset.y = composerFromData.m_TrackedObjectOffset.y;
                composerFromCamera.m_DeadZoneHeight = composerFromData.m_DeadZoneHeight;
                composerFromCamera.m_DeadZoneWidth = composerFromData.m_DeadZoneWidth;
                composerFromCamera.m_SoftZoneHeight = composerFromData.m_SoftZoneHeight;
                composerFromCamera.m_SoftZoneWidth = composerFromData.m_SoftZoneWidth;

                CinemachineFreeLook.Orbit currentOrbit = CharacterStruct.GetOrbit(rig);
                CharacterCinemachineCamera.m_Orbits[rig] = currentOrbit;
            }

            CharacterCinemachineTargetCamera.Follow = CameraTargetTransform;
            CharacterCinemachineTargetCamera.LookAt = CameraTargetTransform;

            CinemachineTransposer targetTransposer = CharacterCinemachineTargetCamera.
                GetCinemachineComponent<CinemachineTransposer>();

            targetTransposer.m_FollowOffset.y = CharacterStruct.TargetCameraBodyOffsetY;
            targetTransposer.m_FollowOffset.z = CharacterStruct.TargetCameraBodyOffsetZ;

            CinemachineComposer targetComposer = CharacterCinemachineTargetCamera.
                GetCinemachineComponent<CinemachineComposer>();

            targetComposer.m_TrackedObjectOffset.y = CharacterStruct.TargetCameraAimOffsetY;
            targetComposer.m_TrackedObjectOffset.z = CharacterStruct.TargetCameraAimOffsetZ;
            targetComposer.m_DeadZoneWidth = CharacterStruct.TargetCameraDeadZoneWidth;
            targetComposer.m_DeadZoneHeight = CharacterStruct.TargetCameraDeadZoneHeight;
            targetComposer.m_SoftZoneWidth = CharacterStruct.TargetCameraSoftZoneWidth;
            targetComposer.m_SoftZoneHeight = CharacterStruct.TargetCameraSoftZoneHeight;

            if (prefab.GetComponent<Animator>() != null)
            {
                CharacterAnimator = prefab.GetComponent<Animator>();
            }
            else
            {
                CharacterAnimator = prefab.AddComponent<Animator>();
            }

            CharacterAnimator.runtimeAnimatorController = CharacterStruct.CharacterDefaultMovementAnimator;
            CharacterAnimator.applyRootMotion = false;        

            if(prefab.GetComponent<PlayerBehaviour>() != null)
            {
                PlayerBehaviour = prefab.GetComponent<PlayerBehaviour>();
                _collidersInTrigger = PlayerBehaviour.CollidersInTriger;
            }
            else
            {
                PlayerBehaviour = prefab.AddComponent<PlayerBehaviour>();
                _collidersInTrigger = PlayerBehaviour.CollidersInTriger;
            }
        }

        #endregion
    }
}
