using UnityEngine;
using Cinemachine;
using System;
using System.Collections.Generic;


namespace BeastHunter
{
    [Serializable]
    public struct CharacterStruct
    {
        #region Fields

        [Header("Scene information fields")]

        [Tooltip("Character prefab.")]
        [SerializeField] private GameObject _prefab;

        [Tooltip("Character camera.")]
        [SerializeField] private Camera _characterCamera;

        [Tooltip("Character cinemachine camera.")]
        [SerializeField] private CinemachineFreeLook _characterCinemachineCamera;

        [Tooltip("Character cinemachine target camera.")]
        [SerializeField] private CinemachineVirtualCamera _characterCinemachineTargetCamera;

        [Tooltip("Character default movement runtime animator controller.")]
        [SerializeField] private RuntimeAnimatorController _characterDefaultMovementAnimator;

        [Tooltip("Character jumping runtime animator controller.")]
        [SerializeField] private RuntimeAnimatorController _characterJumpingAnimator;

        [Tooltip("Character falling runtime animator controller.")]
        [SerializeField] private RuntimeAnimatorController _characterFallingAnimator;

        [Tooltip("Character battle runtime animator controller.")]
        [SerializeField] private RuntimeAnimatorController _characterBattleAnimator;

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

        [Tooltip("Jump horizontal force reducer between 0 and 1.")]
        [Range(0.0f, 1f)]
        [SerializeField] private float _jumpForceReducer;

        [Tooltip("Character falling force between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _fallingForce;

        [Tooltip("Character long jump falling force between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _longJumpFallingForce;

        [Tooltip("Ground check height under character between 0 and 2.")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _groundCheckHeight;

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

        [Header("Character battle settings")]

        [Tooltip("Character battle ignoring time between 0 and 60.")]
        [Range(0.0f, 60.0f)]
        [SerializeField] private float _battleIgnoreTime;

        [Header("Character stats")]

        [Tooltip("Maximal health between 0 and 100.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _maximalHealth;

        [Tooltip("Current health between 0 and 100.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _currentlHealth;

        [Tooltip("Maximal stamina between 0 and 100.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _maximalStamina;

        [Tooltip("Current stamina between 0 and 100.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _currentStamina;

        [Header("Common free look camera Settings")]

        [Tooltip("Is X axis inverted.")]
        [SerializeField] private bool _isInvertedX;

        [Tooltip("Is Y axis inverted.")]
        [SerializeField] private bool _isInvertedY;

        [Space(10)]

        [Tooltip("Camera X-axis rotation speed between 0 and 1 000.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _cameraSpeedX;

        [Tooltip("Camera X-axis rotation acceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraAccelerationLagX;

        [Tooltip("Camera X-axis rotation deceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraDecelerationLagX;

        [Space(10)]

        [Tooltip("Camera Y-axis rotation speed between 0 and 1 000.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _cameraSpeedY;

        [Tooltip("Camera Y-axis rotation acceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraAccelerationLagY;

        [Tooltip("Camera Y-axis rotation deceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraDecelerationLagY;

        [Header("Camera bottom rig Settings")]

        [Tooltip("Y-axis offset of cinemachine camera bottom rig between 0 and 3.")]
        [Range(0.0f, 3.0f)]
        [SerializeField] private float _cameraBottomRigOffsetY;

        [Tooltip("Height of cinemachine camera bottom ring between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _cameraBottomRigHeight;

        [Tooltip("Radius of cinemachine camera low ring between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _cameraBottomRigRadius;

        [Tooltip("Camera bottom rig danger zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraBottomRigDeadZoneHeight;

        [Tooltip("Camera bottom rig danger zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraBottomRigDeadZoneWidth;

        [Tooltip("Camera bottom rig soft zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraBottomRigSoftZoneHeight;

        [Tooltip("Camera bottom rig soft zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraBottomRigSoftZoneWidth;

        [Header("Camera middle rig Settings")]

        [Tooltip("Y-axis offset of cinemachine camera middle rig between 0 and 3.")]
        [Range(0.0f, 3.0f)]
        [SerializeField] private float _cameraMiddleRigOffsetY;

        [Tooltip("Height of cinemachine camera middle ring between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _cameraMiddleRigHeight;

        [Tooltip("Radius of cinemachine camera middle ring between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _cameraMiddleRigRadius;

        [Tooltip("Camera middle rig danger zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraMiddleRigDeadZoneHeight;

        [Tooltip("Camera middle rig danger zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraMiddleRigDeadZoneWidth;

        [Tooltip("Camera middle rig soft zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraMiddleRigSoftZoneHeight;

        [Tooltip("Camera middle rig soft zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraMiddleRigSoftZoneWidth;

        [Header("Camera top rig Settings")]

        [Tooltip("Y-axis offset of cinemachine camera top rig between 0 and 3.")]
        [Range(0.0f, 3.0f)]
        [SerializeField] private float _cameraTopRigOffsetY;

        [Tooltip("Height of cinemachine camera top ring between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _cameraTopRigHeight;

        [Tooltip("Radius of cinemachine camera top ring between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _cameraTopRigRadius;

        [Tooltip("Camera top rig danger zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraTopRigDeadZoneHeight;

        [Tooltip("Camera top rig danger zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraTopRigDeadZoneWidth;

        [Tooltip("Camera top rig soft zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraTopRigSoftZoneHeight;

        [Tooltip("Camera top rig soft zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _cameraTopRigSoftZoneWidth;

        [Header("Free camera Angles Settings")]

        [Tooltip("Angle of camera when going forward and to any side between 0 and 360.")]
        [Range(0.0f, 360f)]
        [SerializeField] private float _cameraLowSideAngle;

        [Tooltip("Angle of camera when going only to any side between 0 and 360.")]
        [Range(0.0f, 360f)]
        [SerializeField] private float _cameraHalfSideAngle;

        [Tooltip("Angle of camera when going back and to any side between 0 and 360.")]
        [Range(0.0f, 360f)]
        [SerializeField] private float _cameraBackSideAngle;

        [Tooltip("Angle of camera when going only back between 0 and 360.")]
        [Range(0.0f, 360f)]
        [SerializeField] private float _cameraBackAngle;

        [Header("Target camera Settings")]

        [Tooltip("Target camera body offset Y between 0 and 10.")]
        [Range(0.0f, 10f)]
        [SerializeField] private float _targetCameraBodyOffsetY;

        [Tooltip("Target camera body offset Z between 0 and 10.")]
        [Range(0.0f, 10f)]
        [SerializeField] private float _targetCameraBodyOffsetZ;

        [Tooltip("Target camera aim offset Y between 0 and 10.")]
        [Range(0.0f, 10f)]
        [SerializeField] private float _targetCameraAimOffsetY;

        [Tooltip("Target camera aim offset Z between 0 and 10.")]
        [Range(0.0f, 10f)]
        [SerializeField] private float _targetCameraAimOffsetZ;

        [Tooltip("Target camera dead zone width between 0 and 1.")]
        [Range(0.0f, 1f)]
        [SerializeField] private float _targetCameraDeadZoneWidth;

        [Tooltip("Target camera dead zone height between 0 and 1.")]
        [Range(0.0f, 1f)]
        [SerializeField] private float _targetCameraDeadZoneHeight;

        [Tooltip("Target camera soft zone width between 0 and 1.")]
        [Range(0.0f, 1f)]
        [SerializeField] private float _targetCameraSoftZoneWidth;

        [Tooltip("Target camera soft zone height between 0 and 1.")]
        [Range(0.0f, 1f)]
        [SerializeField] private float _targetCameraSoftZoneHeight;

        #endregion


        #region Properties

        public GameObject Prefab => _prefab;
        public Camera Camera => _characterCamera;
        public CinemachineFreeLook CinemachineCamera => _characterCinemachineCamera;
        public CinemachineVirtualCamera CinemachineTargetCamera => _characterCinemachineTargetCamera;

        public RuntimeAnimatorController CharacterDefaultMovementAnimator => _characterDefaultMovementAnimator;
        public RuntimeAnimatorController CharacterJumpingAnimator => _characterJumpingAnimator;
        public RuntimeAnimatorController CharacterFallingAnimator => _characterFallingAnimator;
        public RuntimeAnimatorController CharacterBattleAnimator => _characterBattleAnimator;

        public Vector3 InstantiatePosition => _instantiatePosition;
        public Vector3 CapsuleColliderCenter => _capsuleColliderCenter;
        public Vector3 SphereColliderCenter => _sphereColliderCenter;

        public float InstantiateDirection => _instantiateDirection;
        public float RigitbodyMass => _rigitbodyMass;
        public float RigitbodyDrag => _rigitbodyDrag;
        public float RigitbodyAngularDrag => _rigitbodyAngularDrag;

        public float CapsuleColliderRadius => _capsuleColliderRadius;
        public float CapsuleColliderHeight => _capsuleColliderHeight;
        public float SphereColliderRadius => _sphereColliderRadius;
        public float SphereColliderRadiusIncrese => _sphereColliderRadiusIncrease;

        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float InBattleWalkSpeed => _inBattleWalkSpeed;
        public float InBattleRunSpeed => _inBattleRunSpeed;
        public float JumpHorizontalForce => _jumpHorizontalForce;
        public float JumpVerticalForce => _jumpVerticalForce;
        public float JumpForceReducer => _jumpForceReducer;
        public float FallingForce => _fallingForce;
        public float LongJumpFallingForce => _longJumpFallingForce;
        public float GroundCheckHeight => _groundCheckHeight;
        public float SpeedMeasureFrame => _speedMeasureFrame;
        public float AccelerationLag => _accelerationLag;
        public float DecelerationLag => _decelerationLag;
        public float InBattleAccelerationLag => _inBattleAccelerationLag;
        public float InBattleDecelerationLag => _inBattleDecelerationLag;
        public float DirectionChangeLag => _directionChangeLag;

        public float BattleIgnoreTime => _battleIgnoreTime;

        public float MaximalHealth => _maximalHealth;
        public float CurrentHealth => _currentlHealth;
        public float MaximalStamina => _maximalStamina;
        public float CurrentStamina => _currentStamina;

        public bool IsInvertedX => _isInvertedX;
        public bool IsInvertedY => _isInvertedY;

        public float CameraSpeedX => _cameraSpeedX;
        public float CameraAccelerationLagX => _cameraAccelerationLagX;
        public float CameraDecelerationLagX => _cameraDecelerationLagX;
        public float CameraSpeedY => _cameraSpeedY;
        public float CameraAccelerationLagY => _cameraAccelerationLagY;
        public float CameraDecelerationLagY => _cameraDecelerationLagY;

        public float CameraBottomRigDeadZoneHeight => _cameraBottomRigDeadZoneHeight;
        public float CameraBottomRigDeadZoneWidth => _cameraBottomRigDeadZoneWidth;
        public float CameraBottomRigSoftZoneHeight => _cameraBottomRigSoftZoneHeight;
        public float CameraBottomRigSoftZoneWidth => _cameraBottomRigSoftZoneWidth;

        public float CameraMiddleRigDeadZoneHeight => _cameraMiddleRigDeadZoneHeight;
        public float CameraMiddleRigDeadZoneWidth => _cameraMiddleRigDeadZoneWidth;
        public float CameraMiddleRigSoftZoneHeight => _cameraMiddleRigSoftZoneHeight;
        public float CameraMiddleRigSoftZoneWidth => _cameraMiddleRigSoftZoneWidth;

        public float CameraTopRigDeadZoneHeight => _cameraTopRigDeadZoneHeight;
        public float CameraTopRigDeadZoneWidth => _cameraTopRigDeadZoneWidth;
        public float CameraTopRigSoftZoneHeight => _cameraTopRigSoftZoneHeight;
        public float CameraTopRigSoftZoneWidth => _cameraTopRigSoftZoneWidth;

        public float CameraBottomRigOffsetY => _cameraBottomRigOffsetY;
        public float CameraMiddleRigOffsetY => _cameraMiddleRigOffsetY;
        public float CameraTopRigOffsetY => _cameraTopRigOffsetY;
        public float CameraBottomRigHeight => _cameraBottomRigHeight;
        public float CameraMiddleRigHeight => _cameraMiddleRigHeight;
        public float CameraTopRigHeight => _cameraTopRigHeight;
        public float CameraBottomRigRadius => _cameraBottomRigRadius;
        public float CameraMiddleRigRadius => _cameraMiddleRigRadius;
        public float CameraTopRigRadius => _cameraTopRigRadius;

        public float CameraLowSideAngle => _cameraLowSideAngle;
        public float CameraHalfSideAngle => _cameraHalfSideAngle;
        public float CameraBackSideAngle => _cameraBackSideAngle;
        public float CameraBackAngle => _cameraBackAngle;

        public float TargetCameraBodyOffsetY => _targetCameraBodyOffsetY;
        public float TargetCameraBodyOffsetZ => -_targetCameraBodyOffsetZ;
        public float TargetCameraAimOffsetY => _targetCameraAimOffsetY;
        public float TargetCameraAimOffsetZ => _targetCameraAimOffsetZ;
        public float TargetCameraDeadZoneWidth => _targetCameraDeadZoneWidth;
        public float TargetCameraDeadZoneHeight => _targetCameraDeadZoneHeight;
        public float TargetCameraSoftZoneWidth => _targetCameraSoftZoneWidth;
        public float TargetCameraSoftZoneHeight => _targetCameraSoftZoneHeight;

        #endregion


        #region Methods

        public CinemachineFreeLook.Orbit GetOrbit(int orbitNumber)
        {
            CinemachineFreeLook.Orbit orbit = new CinemachineFreeLook.Orbit();

            if (orbitNumber >= 0 && orbitNumber < 3)
            {
                switch (orbitNumber)
                {
                    case 0:
                        orbit.m_Height = CameraBottomRigHeight;
                        orbit.m_Radius = CameraBottomRigRadius;
                        break;
                    case 1:
                        orbit.m_Height = CameraMiddleRigHeight;
                        orbit.m_Radius = CameraMiddleRigRadius;
                        break;
                    case 2:
                        orbit.m_Height = CameraTopRigHeight;
                        orbit.m_Radius = CameraTopRigRadius;
                        break;
                    default:
                        break;
                }
            }

            return orbit;
        }

        public CinemachineComposer GetComposer(int rigNumber)
        {
            CinemachineComposer composer = new CinemachineComposer();

            if (rigNumber >= 0 && rigNumber < 3)
            {
                switch (rigNumber)
                {
                    case 0:
                        composer.m_TrackedObjectOffset.y = CameraBottomRigOffsetY;
                        composer.m_DeadZoneHeight = CameraBottomRigDeadZoneHeight;
                        composer.m_DeadZoneWidth = CameraBottomRigDeadZoneWidth;
                        composer.m_SoftZoneHeight = CameraBottomRigSoftZoneHeight;
                        composer.m_SoftZoneWidth = CameraBottomRigSoftZoneWidth;
                        break;
                    case 1:
                        composer.m_TrackedObjectOffset.y = CameraMiddleRigOffsetY;
                        composer.m_DeadZoneHeight = CameraMiddleRigDeadZoneHeight;
                        composer.m_DeadZoneWidth = CameraMiddleRigDeadZoneWidth;
                        composer.m_SoftZoneHeight = CameraMiddleRigSoftZoneHeight;
                        composer.m_SoftZoneWidth = CameraMiddleRigSoftZoneWidth;
                        break;
                    case 2:
                        composer.m_TrackedObjectOffset.y = CameraTopRigOffsetY;
                        composer.m_DeadZoneHeight = CameraTopRigDeadZoneHeight;
                        composer.m_DeadZoneWidth = CameraTopRigDeadZoneWidth;
                        composer.m_SoftZoneHeight = CameraTopRigSoftZoneHeight;
                        composer.m_SoftZoneWidth = CameraTopRigSoftZoneWidth;
                        break;
                    default:
                        break;
                }
            }

            return composer;
        }

        public void SaveCameraSettings(CinemachineFreeLook camera)
        {
            if(camera != null)
            {
                //TODO
            }
        }

        #endregion
    }
}
