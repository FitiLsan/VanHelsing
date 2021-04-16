using System;
using UnityEngine;
using Cinemachine;


namespace BeastHunter
{
    [Serializable]
    public struct FreeLookCameraSettings
    {
        #region Fields

        [Tooltip("Character free look camera.")]
        [SerializeField] private CinemachineFreeLook _characterFreelookCamera;

        [Tooltip("Character free look camera name.")]
        [SerializeField] private string _characterFreelookCameraName;

        [Range(0.0f, 10.0f)]
        [Tooltip("Freelook camera blend time between 0 and 10.")]
        [SerializeField] private float _characterFreelookCameraBlendTime;

        [Header("Lens Settings")]
        [Header("Character free look camera settings")]

        [Tooltip("Free look field of view between 1 and 179.")]
        [Range(0.0f, 179.0f)]
        [SerializeField] private float _freeLookFieldOfView;

        [Tooltip("Free look near clip plane between 0 and 10 000.")]
        [Range(0.0f, 10000.0f)]
        [SerializeField] private float _freeLookNearClipPlane;

        [Tooltip("Free look far clip plane between 0 and 10 000.")]
        [Range(0.0f, 10000.0f)]
        [SerializeField] private float _freeLookFarClipPlane;

        [Tooltip("Free look dutch between -180 and 180.")]
        [Range(-180.0f, 180.0f)]
        [SerializeField] private float _freeLookDutch;

        [Header("Axis control")]

        [Tooltip("Camera X-axis rotation speed between 0 and 1 000.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _freeLookSpeedX;

        [Tooltip("Camera X-axis rotation acceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookAccelerationLagX;

        [Tooltip("Camera X-axis rotation deceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookDecelerationLagX;

        [Tooltip("Is X axis inverted.")]
        [SerializeField] private bool _isInvertedAxisX;

        [Space(10)]

        [Tooltip("Camera Y-axis rotation speed between 0 and 1 000.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField] private float _freeLookSpeedY;

        [Tooltip("Camera Y-axis rotation acceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookAccelerationLagY;

        [Tooltip("Camera Y-axis rotation deceleration lag between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookDecelerationLagY;

        [Tooltip("Is Y axis inverted.")]
        [SerializeField] private bool _isInvertedAxisY;

        [Header("Orbits settings")]

        [Tooltip("Free look spline curvature between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookSplineCurvature;

        [Tooltip("Height of freelook camera top rig between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookTopRigHeight;

        [Tooltip("Radius of freelook camera top rig between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _freeLookTopRigRadius;

        [Tooltip("Height of freelook camera middle rig between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookMiddleRigHeight;

        [Tooltip("Radius of freelook camera middle rig between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _freeLookMiddleRigRadius;

        [Tooltip("Height of freelook camera bottom rig between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookBottomRigHeight;

        [Tooltip("Radius of freelook camera low rig between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _freeLookBottomRigRadius;

        [Header("Rigs orbital transposer/composer settings")]

        [Tooltip("Top rig body Y damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookTopRigBodyDampingY;

        [Tooltip("Top rig body Z damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookTopRigBodyDampingZ;

        [Tooltip("Top rig X-axis offset between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookTopRigOffsetX;

        [Tooltip("Top rig Y-axis offset between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookTopRigOffsetY;

        [Tooltip("Top rig Z-axis offset between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookTopRigOffsetZ;

        [Tooltip("Top rig look ahead time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _freeLookTopRigLookaheadTime;

        [Tooltip("Top rig lookahead smoothing between 0 and 10.")]
        [Range(3.0f, 30.0f)]
        [SerializeField] private float _freeLookTopRigLookaheadSmoothing;

        [Tooltip("Top rig horizontal damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookTopRigHorizontalDamping;

        [Tooltip("Top rig vertical damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookTopRigVerticalDamping;

        [Tooltip("Top rig screen X between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookTopRigScreenX;

        [Tooltip("Top rig screen Y between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookTopRigScreenY;

        [Tooltip("Camera top rig danger zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookTopRigDeadZoneWidth;

        [Tooltip("Camera top rig danger zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookTopRigDeadZoneHeight;

        [Tooltip("Camera top rig soft zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookTopRigSoftZoneWidth;

        [Tooltip("Camera top rig soft zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookTopRigSoftZoneHeight;

        [Tooltip("Top rig bias X between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _freeLookTopRigBiasX;

        [Tooltip("Top rig bias Y between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _freeLookTopRigBiasY;

        [Space(10)]

        [Tooltip("Top rig body Y damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookMiddleRigBodyDampingY;

        [Tooltip("Middle rig body Z damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookMiddleRigBodyDampingZ;

        [Tooltip("Middle rig X-axis offset between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookMiddleRigOffsetX;

        [Tooltip("Middle rig Y-axis offset between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookMiddleRigOffsetY;

        [Tooltip("Middle rig Z-axis offset between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookMiddleRigOffsetZ;

        [Tooltip("Middle rig look ahead time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _freeLookMiddleRigLookaheadTime;

        [Tooltip("Middle rig lookahead smoothing between 0 and 10.")]
        [Range(3.0f, 30.0f)]
        [SerializeField] private float _freeLookMiddleRigLookaheadSmoothing;

        [Tooltip("Middle rig horizontal damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookMiddleRigHorizontalDamping;

        [Tooltip("Middle rig vertical damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookMiddleRigVerticalDamping;

        [Tooltip("Middle rig screen X between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookMiddleRigScreenX;

        [Tooltip("Middle rig screen Y between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookMiddleRigScreenY;

        [Tooltip("Camera Middle rig danger zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookMiddleRigDeadZoneWidth;

        [Tooltip("Camera Middle rig danger zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookMiddleRigDeadZoneHeight;

        [Tooltip("Camera Middle rig soft zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookMiddleRigSoftZoneWidth;

        [Tooltip("Camera Middle rig soft zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookMiddleRigSoftZoneHeight;

        [Tooltip("Middle rig bias X between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _freeLookMiddleRigBiasX;

        [Tooltip("Middle rig bias Y between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _freeLookMiddleRigBiasY;

        [Space(10)]

        [Tooltip("Top rig body Y damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookBottomRigBodyDampingY;

        [Tooltip("Bottom rig body Z damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookBottomRigBodyDampingZ;

        [Tooltip("Bottom rig X-axis offset between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookBottomRigOffsetX;

        [Tooltip("Bottom rig Y-axis offset between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookBottomRigOffsetY;

        [Tooltip("Bottom rig Z-axis offset between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _freeLookBottomRigOffsetZ;

        [Tooltip("Bottom rig look ahead time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _freeLookBottomRigLookaheadTime;

        [Tooltip("Bottom rig lookahead smoothing between 0 and 10.")]
        [Range(3.0f, 30.0f)]
        [SerializeField] private float _freeLookBottomRigLookaheadSmoothing;

        [Tooltip("Bottom rig horizontal damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookBottomRigHorizontalDamping;

        [Tooltip("Bottom rig vertical damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _freeLookBottomRigVerticalDamping;

        [Tooltip("Bottom rig screen X between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookBottomRigScreenX;

        [Tooltip("Bottom rig screen Y between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookBottomRigScreenY;

        [Tooltip("Camera Bottom rig danger zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookBottomRigDeadZoneWidth;

        [Tooltip("Camera Bottom rig danger zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookBottomRigDeadZoneHeight;

        [Tooltip("Camera Bottom rig soft zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookBottomRigSoftZoneWidth;

        [Tooltip("Camera Bottom rig soft zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _freeLookBottomRigSoftZoneHeight;

        [Tooltip("Bottom rig bias X between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _freeLookBottomRigBiasX;

        [Tooltip("Bottom rig bias Y between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _freeLookBottomRigBiasY;

        [Header("Free look camera collider settings")]

        [Tooltip("Minimal distance from target between 0 and 100.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _colliderMinDistanceFromTarget;

        [Tooltip("Avoid obstacles.")]
        [SerializeField] private bool _isColliderAvoidingObstacles;

        [Tooltip("Distance limit between 0 and 100.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _colliderDistanceLimit;

        [Tooltip("Minimal distance from target between 0 and 100.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _colliderCameraRadius;

        [Tooltip("Minimal distance from target between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _colliderDamping;

        [Tooltip("Minimal distance from target between 0 and 100.")]
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _colliderOptimalTargetDistance;

        #endregion


        #region Properties

        public CinemachineFreeLook CharacterFreelookCamera => _characterFreelookCamera;
 
        public string CharacterFreelookCameraName => _characterFreelookCameraName;
        public float CharacterFreelookCameraBlendTime => _characterFreelookCameraBlendTime;  

        public float FreeLookFieldOfView => _freeLookFieldOfView;
        public float FreeLookNearClipPlane => _freeLookNearClipPlane;
        public float FreeLookFarClipPlane => _freeLookFarClipPlane;
        public float FreeLookDutch => _freeLookDutch;

        public float CameraSpeedX => _freeLookSpeedX;
        public float CameraAccelerationLagX => _freeLookAccelerationLagX;
        public float CameraDecelerationLagX => _freeLookDecelerationLagX;
        public bool IsInvertedX => _isInvertedAxisX;

        public float CameraSpeedY => _freeLookSpeedY;
        public float CameraAccelerationLagY => _freeLookAccelerationLagY;
        public float CameraDecelerationLagY => _freeLookDecelerationLagY;
        public bool IsInvertedY => _isInvertedAxisY;

        public float FreeLookSplineCurvature => _freeLookSplineCurvature;
        public float CameraTopRigHeight => _freeLookTopRigHeight;
        public float CameraTopRigRadius => _freeLookTopRigRadius;
        public float CameraMiddleRigHeight => _freeLookMiddleRigHeight;
        public float CameraMiddleRigRadius => _freeLookMiddleRigRadius;
        public float CameraBottomRigHeight => _freeLookBottomRigHeight;
        public float CameraBottomRigRadius => _freeLookBottomRigRadius;

        public float FreeLookTopRigBodyDampingY => _freeLookTopRigBodyDampingY;
        public float FreeLookTopRigBodyDampingZ => _freeLookTopRigBodyDampingZ;
        public float CameraTopRigOffsetY => _freeLookTopRigOffsetY;
        public float CameraTopRigOffsetX => _freeLookTopRigOffsetX;
        public float CameraTopRigOffsetZ => _freeLookTopRigOffsetZ;
        public float FreeLookTopRigLookaheadTime => _freeLookTopRigLookaheadTime;
        public float FreeLookTopRigLookaheadSmoothing => _freeLookTopRigLookaheadSmoothing;
        public float FreeLookTopRigHorizontalDamping => _freeLookTopRigHorizontalDamping;
        public float FreeLookTopRigVerticalDamping => _freeLookTopRigVerticalDamping;
        public float FreeLookTopRigScreenX => _freeLookTopRigScreenX;
        public float FreeLookTopRigScreenY => _freeLookTopRigScreenY;
        public float CameraTopRigDeadZoneWidth => _freeLookTopRigDeadZoneWidth;
        public float CameraTopRigDeadZoneHeight => _freeLookTopRigDeadZoneHeight;
        public float CameraTopRigSoftZoneWidth => _freeLookTopRigSoftZoneWidth;
        public float CameraTopRigSoftZoneHeight => _freeLookTopRigSoftZoneHeight;
        public float FreeLookTopRigBiasX => _freeLookTopRigBiasX;
        public float FreeLookTopRigBiasY => _freeLookTopRigBiasY;

        public float FreeLookMiddleRigBodyDampingY => _freeLookMiddleRigBodyDampingY;
        public float FreeLookMiddleRigBodyDampingZ => _freeLookMiddleRigBodyDampingZ;
        public float CameraMiddleRigOffsetY => _freeLookMiddleRigOffsetY;
        public float CameraMiddleRigOffsetX => _freeLookMiddleRigOffsetX;
        public float CameraMiddleRigOffsetZ => _freeLookMiddleRigOffsetZ;
        public float FreeLookMiddleRigLookaheadTime => _freeLookMiddleRigLookaheadTime;
        public float FreeLookMiddleRigLookaheadSmoothing => _freeLookMiddleRigLookaheadSmoothing;
        public float FreeLookMiddleRigHorizontalDamping => _freeLookMiddleRigHorizontalDamping;
        public float FreeLookMiddleRigVerticalDamping => _freeLookMiddleRigVerticalDamping;
        public float FreeLookMiddleRigScreenX => _freeLookMiddleRigScreenX;
        public float FreeLookMiddleRigScreenY => _freeLookMiddleRigScreenY;
        public float CameraMiddleRigDeadZoneWidth => _freeLookMiddleRigDeadZoneWidth;
        public float CameraMiddleRigDeadZoneHeight => _freeLookMiddleRigDeadZoneHeight;
        public float CameraMiddleRigSoftZoneWidth => _freeLookMiddleRigSoftZoneWidth;
        public float CameraMiddleRigSoftZoneHeight => _freeLookMiddleRigSoftZoneHeight;
        public float FreeLookMiddleRigBiasX => _freeLookMiddleRigBiasX;
        public float FreeLookMiddleRigBiasY => _freeLookMiddleRigBiasY;

        public float FreeLookBottomRigBodyDampingY => _freeLookBottomRigBodyDampingY;
        public float FreeLookBottomRigBodyDampingZ => _freeLookBottomRigBodyDampingZ;
        public float CameraBottomRigOffsetY => _freeLookBottomRigOffsetY;
        public float CameraBottomRigOffsetX => _freeLookBottomRigOffsetX;
        public float CameraBottomRigOffsetZ => _freeLookBottomRigOffsetZ;
        public float FreeLookBottomRigLookaheadTime => _freeLookBottomRigLookaheadTime;
        public float FreeLookBottomRigLookaheadSmoothing => _freeLookBottomRigLookaheadSmoothing;
        public float FreeLookBottomRigHorizontalDamping => _freeLookBottomRigHorizontalDamping;
        public float FreeLookBottomRigVerticalDamping => _freeLookBottomRigVerticalDamping;
        public float FreeLookBottomRigScreenX => _freeLookBottomRigScreenX;
        public float FreeLookBottomRigScreenY => _freeLookBottomRigScreenY;
        public float CameraBottomRigDeadZoneWidth => _freeLookBottomRigDeadZoneWidth;
        public float CameraBottomRigDeadZoneHeight => _freeLookBottomRigDeadZoneHeight;
        public float CameraBottomRigSoftZoneWidth => _freeLookBottomRigSoftZoneWidth;
        public float CameraBottomRigSoftZoneHeight => _freeLookBottomRigSoftZoneHeight;
        public float FreeLookBottomRigBiasX => _freeLookBottomRigBiasX;
        public float FreeLookBottomRigBiasY => _freeLookBottomRigBiasY;

        public float ColliderMinDistanceFromTarget => _colliderMinDistanceFromTarget;
        public bool IsColliderAvoidingObstacles => _isColliderAvoidingObstacles;
        public float ColliderDistanceLimit => _colliderDistanceLimit;
        public float ColliderCameraRadius => _colliderCameraRadius;
        public float ColliderDamping => _colliderDamping;
        public float ColliderOptimalTargetDistance => _colliderOptimalTargetDistance;

        #endregion


        #region Methods

        public CinemachineFreeLook.Orbit GetFreeCameraOrbit(int orbitNumber)
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

        public CinemachineOrbitalTransposer GetFreeCameraOrbitalTransposer(int rigNumber)
        {
            CinemachineOrbitalTransposer orbitalTransposer = new CinemachineOrbitalTransposer();

            if(rigNumber >= 0 && rigNumber < 3)
            {
                switch (rigNumber)
                {
                    case 0:
                        orbitalTransposer.m_YDamping = FreeLookBottomRigBodyDampingY;
                        orbitalTransposer.m_ZDamping = FreeLookBottomRigBodyDampingZ;
                        break;
                    case 1:
                        orbitalTransposer.m_YDamping = FreeLookMiddleRigBodyDampingY;
                        orbitalTransposer.m_ZDamping = FreeLookMiddleRigBodyDampingZ;
                        break;
                    case 2:
                        orbitalTransposer.m_YDamping = FreeLookTopRigBodyDampingY;
                        orbitalTransposer.m_ZDamping = FreeLookTopRigBodyDampingZ;
                        break;
                    default:
                        break;
                }
            }

            return orbitalTransposer;
        }

        public CinemachineComposer GetFreeCameraComposer(int rigNumber)
        {
            CinemachineComposer composer = new CinemachineComposer();

            if (rigNumber >= 0 && rigNumber < 3)
            {
                switch (rigNumber)
                {
                    case 0:
                        composer.m_TrackedObjectOffset.x = CameraBottomRigOffsetX;
                        composer.m_TrackedObjectOffset.y = CameraBottomRigOffsetY;
                        composer.m_TrackedObjectOffset.z = CameraBottomRigOffsetZ;
                        composer.m_LookaheadTime = FreeLookBottomRigLookaheadTime;
                        composer.m_LookaheadSmoothing = FreeLookBottomRigLookaheadSmoothing;
                        composer.m_HorizontalDamping = FreeLookBottomRigHorizontalDamping;
                        composer.m_VerticalDamping = FreeLookBottomRigVerticalDamping;
                        composer.m_ScreenX = FreeLookBottomRigScreenX;
                        composer.m_ScreenY = FreeLookBottomRigScreenY;
                        composer.m_DeadZoneWidth = CameraBottomRigDeadZoneWidth;
                        composer.m_DeadZoneHeight = CameraBottomRigDeadZoneHeight;
                        composer.m_SoftZoneWidth = CameraBottomRigSoftZoneWidth;
                        composer.m_SoftZoneHeight = CameraBottomRigSoftZoneHeight;
                        composer.m_BiasX = FreeLookBottomRigBiasX;
                        composer.m_BiasY = FreeLookBottomRigBiasY;
                        break;
                    case 1:
                        composer.m_TrackedObjectOffset.x = CameraMiddleRigOffsetX;
                        composer.m_TrackedObjectOffset.y = CameraMiddleRigOffsetY;
                        composer.m_TrackedObjectOffset.z = CameraMiddleRigOffsetZ;
                        composer.m_LookaheadTime = FreeLookMiddleRigLookaheadTime;
                        composer.m_LookaheadSmoothing = FreeLookMiddleRigLookaheadSmoothing;
                        composer.m_HorizontalDamping = FreeLookMiddleRigHorizontalDamping;
                        composer.m_VerticalDamping = FreeLookMiddleRigVerticalDamping;
                        composer.m_ScreenX = FreeLookMiddleRigScreenX;
                        composer.m_ScreenY = FreeLookMiddleRigScreenY;
                        composer.m_DeadZoneWidth = CameraMiddleRigDeadZoneWidth;
                        composer.m_DeadZoneHeight = CameraMiddleRigDeadZoneHeight;
                        composer.m_SoftZoneWidth = CameraMiddleRigSoftZoneWidth;
                        composer.m_SoftZoneHeight = CameraMiddleRigSoftZoneHeight;
                        composer.m_BiasX = FreeLookMiddleRigBiasX;
                        composer.m_BiasY = FreeLookMiddleRigBiasY;
                        break;
                    case 2:
                        composer.m_TrackedObjectOffset.x = CameraTopRigOffsetX;
                        composer.m_TrackedObjectOffset.y = CameraTopRigOffsetY;
                        composer.m_TrackedObjectOffset.z = CameraTopRigOffsetZ;
                        composer.m_LookaheadTime = FreeLookTopRigLookaheadTime;
                        composer.m_LookaheadSmoothing = FreeLookTopRigLookaheadSmoothing;
                        composer.m_HorizontalDamping = FreeLookTopRigHorizontalDamping;
                        composer.m_VerticalDamping = FreeLookTopRigVerticalDamping;
                        composer.m_ScreenX = FreeLookTopRigScreenX;
                        composer.m_ScreenY = FreeLookTopRigScreenY;
                        composer.m_DeadZoneWidth = CameraTopRigDeadZoneWidth;
                        composer.m_DeadZoneHeight = CameraTopRigDeadZoneHeight;
                        composer.m_SoftZoneWidth = CameraTopRigSoftZoneWidth;
                        composer.m_SoftZoneHeight = CameraTopRigSoftZoneHeight;
                        composer.m_BiasX = FreeLookTopRigBiasX;
                        composer.m_BiasY = FreeLookTopRigBiasY;
                        break;
                    default:
                        break;
                }
            }

            return composer;
        }

        public void SaveCameraSettings(CinemachineFreeLook freeLookCamera)
        {
            if (freeLookCamera == null)
            {
                throw new NullReferenceException("Can't save free look camera settings - argument is null!");
            }

            CinemachineComposer bottomComposer = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineComposer>();
            CinemachineComposer middleComposer = freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
            CinemachineComposer topComposer = freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineComposer>();
            CinemachineCollider collider = freeLookCamera.GetComponent<CinemachineCollider>();

            CinemachineFreeLook.Orbit bottomOrbit = freeLookCamera.m_Orbits[0];
            CinemachineFreeLook.Orbit middleOrbit = freeLookCamera.m_Orbits[1];
            CinemachineFreeLook.Orbit topOrbit = freeLookCamera.m_Orbits[2];

            _isInvertedAxisX = freeLookCamera.m_XAxis.m_InvertInput;
            _isInvertedAxisY = freeLookCamera.m_YAxis.m_InvertInput;

            _freeLookSpeedX = freeLookCamera.m_XAxis.m_MaxSpeed;
            _freeLookAccelerationLagX = freeLookCamera.m_XAxis.m_AccelTime;
            _freeLookDecelerationLagX = freeLookCamera.m_XAxis.m_DecelTime;
            
            _freeLookSpeedY = freeLookCamera.m_YAxis.m_MaxSpeed;
            _freeLookAccelerationLagY = freeLookCamera.m_YAxis.m_AccelTime;
            _freeLookDecelerationLagY = freeLookCamera.m_YAxis.m_DecelTime;

            _freeLookBottomRigHeight = bottomOrbit.m_Height;
            _freeLookBottomRigRadius = bottomOrbit.m_Radius;
            _freeLookMiddleRigHeight = middleOrbit.m_Height;
            _freeLookMiddleRigRadius = middleOrbit.m_Radius;
            _freeLookTopRigHeight = topOrbit.m_Height;
            _freeLookTopRigRadius = topOrbit.m_Radius;

            _freeLookBottomRigOffsetY = bottomComposer.m_TrackedObjectOffset.y;
            _freeLookBottomRigDeadZoneHeight = bottomComposer.m_DeadZoneHeight;
            _freeLookBottomRigDeadZoneWidth = bottomComposer.m_DeadZoneWidth;
            _freeLookBottomRigSoftZoneHeight = bottomComposer.m_SoftZoneHeight;
            _freeLookBottomRigSoftZoneWidth = bottomComposer.m_SoftZoneWidth;

            _freeLookMiddleRigOffsetY = middleComposer.m_TrackedObjectOffset.y;
            _freeLookMiddleRigDeadZoneHeight = middleComposer.m_DeadZoneHeight;
            _freeLookMiddleRigDeadZoneWidth = middleComposer.m_DeadZoneWidth;
            _freeLookMiddleRigSoftZoneHeight = middleComposer.m_SoftZoneHeight;
            _freeLookMiddleRigSoftZoneWidth = middleComposer.m_SoftZoneWidth;

            _freeLookTopRigOffsetY = topComposer.m_TrackedObjectOffset.y;
            _freeLookTopRigDeadZoneHeight = topComposer.m_DeadZoneHeight;
            _freeLookTopRigDeadZoneWidth = topComposer.m_DeadZoneWidth;
            _freeLookTopRigSoftZoneHeight = topComposer.m_SoftZoneHeight;
            _freeLookTopRigSoftZoneWidth = topComposer.m_SoftZoneWidth;

            _colliderMinDistanceFromTarget = collider.m_MinimumDistanceFromTarget;
            _isColliderAvoidingObstacles = collider.m_AvoidObstacles;
            _colliderDistanceLimit = collider.m_DistanceLimit;
            _colliderCameraRadius = collider.m_CameraRadius;
            _colliderDamping = collider.m_Damping;
            _colliderOptimalTargetDistance = collider.m_OptimalTargetDistance;           
        }

        public CinemachineFreeLook CreateCharacterFreelookCamera(Transform followTransform, Transform lookAtTransform, 
            Transform parent = null)
        {
            CinemachineFreeLook characterFreelookCamera;

            if(parent == null)
            {
                characterFreelookCamera = GameObject.Instantiate(CharacterFreelookCamera);
            }
            else
            {
                characterFreelookCamera = GameObject.Instantiate(CharacterFreelookCamera, parent);
            }
                                       
            characterFreelookCamera.name = CharacterFreelookCameraName;

            characterFreelookCamera.Follow = followTransform;
            characterFreelookCamera.LookAt = lookAtTransform;

            //characterFreelookCamera.m_Lens.FieldOfView = FreeLookFieldOfView;
            //characterFreelookCamera.m_Lens.NearClipPlane = FreeLookNearClipPlane;
            //characterFreelookCamera.m_Lens.FarClipPlane = FreeLookFarClipPlane;
            //characterFreelookCamera.m_Lens.Dutch = FreeLookDutch;

            //characterFreelookCamera.m_XAxis.m_InvertInput = IsInvertedX;
            //characterFreelookCamera.m_XAxis.m_MaxSpeed = CameraSpeedX;
            //characterFreelookCamera.m_XAxis.m_AccelTime = CameraAccelerationLagX;
            //characterFreelookCamera.m_XAxis.m_DecelTime = CameraDecelerationLagX;

            //characterFreelookCamera.m_YAxis.m_InvertInput = IsInvertedY;
            //characterFreelookCamera.m_YAxis.m_MaxSpeed = CameraSpeedY;
            //characterFreelookCamera.m_YAxis.m_AccelTime = CameraAccelerationLagY;
            //characterFreelookCamera.m_YAxis.m_DecelTime = CameraDecelerationLagY;

            //characterFreelookCamera.m_SplineCurvature = FreeLookSplineCurvature;

            for (var rig = 0; rig < 3; rig++)
            {
                CinemachineVirtualCamera cinemachineRig = characterFreelookCamera.GetRig(rig);
                cinemachineRig.LookAt = lookAtTransform;

                //CinemachineOrbitalTransposer transposerFromData = GetFreeCameraOrbitalTransposer(rig);
                //CinemachineOrbitalTransposer transposerFromCamera = cinemachineRig.
                //    GetCinemachineComponent<CinemachineOrbitalTransposer>();

                //transposerFromCamera.m_YDamping = transposerFromData.m_YDamping;
                //transposerFromCamera.m_ZDamping = transposerFromData.m_ZDamping;

                //CinemachineComposer composerFromData = GetFreeCameraComposer(rig);
                //CinemachineComposer composerFromCamera = cinemachineRig.GetCinemachineComponent<CinemachineComposer>();

                //composerFromCamera.m_TrackedObjectOffset.x = composerFromData.m_TrackedObjectOffset.x;
                //composerFromCamera.m_TrackedObjectOffset.y = composerFromData.m_TrackedObjectOffset.y;
                //composerFromCamera.m_TrackedObjectOffset.z = composerFromData.m_TrackedObjectOffset.z;
                //composerFromCamera.m_LookaheadTime = composerFromData.m_LookaheadTime;
                //composerFromCamera.m_LookaheadSmoothing = composerFromData.m_LookaheadSmoothing;
                //composerFromCamera.m_HorizontalDamping = composerFromData.m_HorizontalDamping;
                //composerFromCamera.m_VerticalDamping = composerFromData.m_VerticalDamping;
                //composerFromCamera.m_ScreenX = composerFromData.m_ScreenX;
                //composerFromCamera.m_ScreenY = composerFromData.m_ScreenY;
                //composerFromCamera.m_DeadZoneWidth = composerFromData.m_DeadZoneWidth;
                //composerFromCamera.m_DeadZoneHeight = composerFromData.m_DeadZoneHeight;
                //composerFromCamera.m_SoftZoneWidth = composerFromData.m_SoftZoneWidth;
                //composerFromCamera.m_SoftZoneHeight = composerFromData.m_SoftZoneHeight;
                //composerFromCamera.m_BiasX = composerFromData.m_BiasX;
                //composerFromCamera.m_BiasY = composerFromData.m_BiasY;

                //CinemachineFreeLook.Orbit currentOrbit = GetFreeCameraOrbit(rig);
                //characterFreelookCamera.m_Orbits[rig] = currentOrbit;
            }

            //CinemachineCollider colliderFromCamera = characterFreelookCamera.GetComponent<CinemachineCollider>();
            //colliderFromCamera.m_MinimumDistanceFromTarget = ColliderMinDistanceFromTarget;
            //colliderFromCamera.m_AvoidObstacles = IsColliderAvoidingObstacles;
            //colliderFromCamera.m_DistanceLimit = ColliderDistanceLimit;
            //colliderFromCamera.m_CameraRadius = ColliderCameraRadius;
            //colliderFromCamera.m_Damping = ColliderDamping;
            //colliderFromCamera.m_OptimalTargetDistance = ColliderOptimalTargetDistance;

            return characterFreelookCamera;
        }

        #endregion
    }
}


