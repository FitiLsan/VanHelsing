using System;
using Cinemachine;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct CharacterCameraStruct
    {
        #region Fields

        [Tooltip("Character camera.")]
        [SerializeField] private Camera _characterCamera;

        [Tooltip("Character camera name.")]
        [SerializeField] private string _characterCameraName;

        [Tooltip("Character free look camera.")]
        [SerializeField] private CinemachineFreeLook _characterFreelookCamera;

        [Tooltip("Character free look camera name.")]
        [SerializeField] private string _characterFreelookCameraName;

        [Range(0.0f, 10.0f)]
        [Tooltip("Freelook camera blend time between 0 and 10.")]
        [SerializeField] private float _characterFreelookCameraBlendTime;

        [Tooltip("Character free look knocked down camera.")]
        [SerializeField] private CinemachineFreeLook _characterKnockedDownCamera;

        [Tooltip("Character knocked down camera name.")]
        [SerializeField] private string _characterKnockedDownCameraName;

        [Range(0.0f, 10.0f)]
        [Tooltip("Knocked down camera blend time between 0 and 10.")]
        [SerializeField] private float _characterKnockedDownCameraBlendTime;

        [Tooltip("Character target camera.")]
        [SerializeField] private CinemachineVirtualCamera _characterTargetCamera;

        [Tooltip("Character target camera name.")]
        [SerializeField] private string _characterTargetCameraName;

        [Tooltip("Camera target name.")]
        [SerializeField] private string _cameraTargetName;

        [Range(0.0f, 10.0f)]
        [Tooltip("Target camera blend time between 0 and 10.")]
        [SerializeField] private float _characterTargetCameraBlendTime;

        [Range(0.0f, 5.0f)]
        [Tooltip("Camera target height between 0 and 5.")]
        [SerializeField] private float _cameraTargetHeight;

        [Tooltip("Character dialog camera.")]
        [SerializeField] private CinemachineVirtualCamera _characterAimingCamera;

        [Tooltip("Character dialog camera name.")]
        [SerializeField] private string _characterAimingCameraName;

        [Range(0.0f, 10.0f)]
        [Tooltip("Dialog camera blend time between 0 and 10.")]
        [SerializeField] private float _characterAimingCameraBlendTime;

        [Header("Lens Settings")]

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

        [Header("Lens Settings")]

        [Header("Target camera Settings")]

        [Tooltip("Target camera field of view between 1 and 179.")]
        [Range(0.0f, 179.0f)]
        [SerializeField] private float _targetCameraFieldOfView;

        [Tooltip("Target camera near clip plane between 0 and 10 000.")]
        [Range(0.0f, 10000.0f)]
        [SerializeField] private float _targetCameraNearClipPlane;

        [Tooltip("Target camera far clip plane between 0 and 10 000.")]
        [Range(0.0f, 10000.0f)]
        [SerializeField] private float _targetCameraFarClipPlane;

        [Tooltip("Target camera dutch between -180 and 180.")]
        [Range(-180.0f, 180.0f)]
        [SerializeField] private float _targetCameraDutch;

        [Header("Transposer/composer settings")]

        [Tooltip("Target camera follow offset X between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _targetCameraFollowOffsetX;

        [Tooltip("Target camera follow offset Y between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _targetCameraFollowOffsetY;

        [Tooltip("Target camera follow offset Z between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _targetCameraFollowOffsetZ;

        [Tooltip("Target camera X damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _targetCameraDampingX;

        [Tooltip("Target camera Y damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _targetCameraDampingY;

        [Tooltip("Target camera Z damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _targetCameraDampingZ;

        [Tooltip("Target camera pitch damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _targetCameraPitchDamping;

        [Tooltip("Target camera yaw damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _targetCameraYawDamping;

        [Tooltip("Target camera roll damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _targetCameraRollDamping;

        [Space(10)]

        [Tooltip("Target camera tracked object offset X between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _targetCameraTrackedOffsetX;

        [Tooltip("Target camera tracked object offset Y between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _targetCameraTrackedOffsetY;

        [Tooltip("Target camera tracked object offset Z between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _targetCameraTrackedOffsetZ;

        [Tooltip("Target camera look ahead time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _targetCameraLookaheadTime;

        [Tooltip("Target camera lookahead smoothing between 0 and 10.")]
        [Range(3.0f, 30.0f)]
        [SerializeField] private float _targetCameraLookaheadSmoothing;

        [Tooltip("Target camera horizontal damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _targetCameraHorizontalDamping;

        [Tooltip("Target camera vertical damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _targetCameraVerticalDamping;

        [Tooltip("Target camera screen X between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _targetCameraScreenX;

        [Tooltip("Target camera screen Y between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _targetCameraScreenY;

        [Tooltip("Target camera danger zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _targetCameraDeadZoneWidth;

        [Tooltip("Target camera danger zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _targetCameraDeadZoneHeight;

        [Tooltip("Target camera soft zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _targetCameraSoftZoneWidth;

        [Tooltip("Target camera soft zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _targetCameraSoftZoneHeight;

        [Tooltip("Target camera bias X between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _targetCameraBiasX;

        [Tooltip("Target camera bias Y between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _targetCameraBiasY;

        [Header("Lens Settings")]

        [Header("Aiming camera Settings")]

        [Tooltip("Aiming camera field of view between 1 and 179.")]
        [Range(0.0f, 179.0f)]
        [SerializeField] private float _aimingCameraFieldOfView;

        [Tooltip("Aiming camera near clip plane between 0 and 10 000.")]
        [Range(0.0f, 10000.0f)]
        [SerializeField] private float _aimingCameraNearClipPlane;

        [Tooltip("Aiming camera far clip plane between 0 and 10 000.")]
        [Range(0.0f, 10000.0f)]
        [SerializeField] private float _aimingCameraFarClipPlane;

        [Tooltip("Aiming camera dutch between -180 and 180.")]
        [Range(-180.0f, 180.0f)]
        [SerializeField] private float _aimingCameraDutch;

        [Header("Transposer/composer settings")]

        [Tooltip("Aiming camera follow offset X between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _aimingCameraFollowOffsetX;

        [Tooltip("Aiming camera follow offset Y between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _aimingCameraFollowOffsetY;

        [Tooltip("Aiming camera follow offset Z between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _aimingCameraFollowOffsetZ;

        [Tooltip("Aiming camera X damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimingCameraDampingX;

        [Tooltip("Aiming camera Y damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimingCameraDampingY;

        [Tooltip("Aiming camera Z damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimingCameraDampingZ;

        [Tooltip("Aiming camera pitch damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimingCameraPitchDamping;

        [Tooltip("Aiming camera yaw damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimingCameraYawDamping;

        [Tooltip("Aiming camera roll damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimingCameraRollDamping;

        [Space(10)]

        [Tooltip("Aiming camera tracked object offset X between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _aimingCameraTrackedOffsetX;

        [Tooltip("Aiming camera tracked object offset Y between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _aimingCameraTrackedOffsetY;

        [Tooltip("Aiming camera tracked object offset Z between -10 and 10.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _aimingCameraTrackedOffsetZ;

        [Tooltip("Aiming camera look ahead time between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _aimingCameraLookaheadTime;

        [Tooltip("Aiming camera lookahead smoothing between 0 and 10.")]
        [Range(3.0f, 30.0f)]
        [SerializeField] private float _aimingCameraLookaheadSmoothing;

        [Tooltip("Aiming camera horizontal damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimingCameraHorizontalDamping;

        [Tooltip("Aiming camera vertical damping between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _aimingCameraVerticalDamping;

        [Tooltip("Aiming camera screen X between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _aimingCameraScreenX;

        [Tooltip("Aiming camera screen Y between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _aimingCameraScreenY;

        [Tooltip("Aiming camera danger zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _aimingCameraDeadZoneWidth;

        [Tooltip("Aiming camera danger zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _aimingCameraDeadZoneHeight;

        [Tooltip("Aiming camera soft zone width between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _aimingCameraSoftZoneWidth;

        [Tooltip("Aiming camera soft zone height between 0 and 1.")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _aimingCameraSoftZoneHeight;

        [Tooltip("Aiming camera bias X between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _aimingCameraBiasX;

        [Tooltip("Aiming camera bias Y between 0 and 1.")]
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float _aimingCameraBiasY;

        [Header("Camera target settings")]

        [Tooltip("Camera target object forward movement distace")]
        [SerializeField] private float _cameraTargetForwardMovementDistance;

        [Tooltip("Camera target object horizontal movement speed")]
        [SerializeField] private float _cameraTargetSpeedX;

        [Tooltip("Camera target object vertical movement speed")]
        [SerializeField] private float _cameraTargetSpeedY;

        [Tooltip("Camera target object minimal and maximal horizontal movement distance")]
        [SerializeField] private Vector2 _cameraTargetDistanceMoveX;

        [Tooltip("Camera target object minimal and maximal vertical movement distance")]
        [SerializeField] private Vector2 _cameraTargetDistanceMoveY;

        #endregion


        #region Properties

        public Camera CharacterCamera => _characterCamera;
        public CinemachineFreeLook CharacterFreelookCamera => _characterFreelookCamera;
        public CinemachineFreeLook CharacterKnockedDownCamera => _characterKnockedDownCamera;
        public CinemachineVirtualCamera CharacterTargetCamera => _characterTargetCamera;
        public CinemachineVirtualCamera CharacterAimingCamera => _characterAimingCamera;
        public Vector2 CameraTargetDistanceMoveX => _cameraTargetDistanceMoveX;
        public Vector2 CameraTargetDistanceMoveY => _cameraTargetDistanceMoveY;

        public string CharacterCameraName => _characterCameraName;
        public string CharacterFreelookCameraName => _characterFreelookCameraName;
        public string CharacterKnockedDownCameraName => _characterKnockedDownCameraName;
        public string CharacterTargetCameraName => _characterTargetCameraName;
        public string CameraTargetName => _cameraTargetName;
        public string CharacterAimingCameraName => _characterAimingCameraName;

        public float CharacterFreelookCameraBlendTime => _characterFreelookCameraBlendTime;
        public float CharacterKnockedDownCameraBlendTime => _characterKnockedDownCameraBlendTime;
        public float CharacterTargetCameraBlendTime => _characterTargetCameraBlendTime;
        public float CHaracterAimingCameraBlendTIme => _characterAimingCameraBlendTime;
        public float CameraTargetHeight => _cameraTargetHeight;

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

        public float TargetCameraFieldOfView => _targetCameraFieldOfView;
        public float TargetCameraNearClipPlane => _targetCameraNearClipPlane;
        public float TargetCameraFarClipPlane => _targetCameraFarClipPlane;
        public float TargetCameraDutch => _targetCameraDutch;
        public float TargetCameraFollowOffsetX => _targetCameraFollowOffsetX;
        public float TargetCameraFollowOffsetY => _targetCameraFollowOffsetY;
        public float TargetCameraFollowOffsetZ => _targetCameraFollowOffsetZ;
        public float TargetCameraDampingX => _targetCameraDampingX;
        public float TargetCameraDampingY => _targetCameraDampingY;
        public float TargetCameraDampingZ => _targetCameraDampingZ;
        public float TargetCameraPitchDamping => _targetCameraPitchDamping;
        public float TargetCameraYawDamping => _targetCameraYawDamping;
        public float TargetCameraRollDamping => _targetCameraRollDamping;
        public float TargetCameraTrackedOffsetX => _targetCameraTrackedOffsetX;
        public float TargetCameraTrackedOffsetY => _targetCameraTrackedOffsetY;
        public float TargetCameraTrackedOffsetZ => _targetCameraTrackedOffsetZ;
        public float TargetCameraLookaheadTime => _targetCameraLookaheadTime;
        public float TargetCameraLookaheadSmoothing => _targetCameraLookaheadSmoothing;
        public float TargetCameraHorizontalDamping => _targetCameraHorizontalDamping;
        public float TargetCameraVerticalDamping => _targetCameraVerticalDamping;
        public float TargetCameraScreenX => _targetCameraScreenX;
        public float TargetCameraScreenY => _targetCameraScreenY;
        public float TargetCameraDeadZoneWidth => _targetCameraDeadZoneWidth;
        public float TargetCameraDeadZoneHeight => _targetCameraDeadZoneHeight;
        public float TargetCameraSoftZoneWidth => _targetCameraSoftZoneWidth;
        public float TargetCameraSoftZoneHeight => _targetCameraSoftZoneHeight;
        public float TargetCameraBiasX => _targetCameraBiasX;
        public float TargetCameraBiasY => _targetCameraBiasY;

        public float AimingCameraFieldOfView => _aimingCameraFieldOfView;
        public float AimingCameraNearClipPlane => _aimingCameraNearClipPlane;
        public float AimingCameraFarClipPlane => _aimingCameraFarClipPlane;
        public float AimingCameraDutch => _aimingCameraDutch;
        public float AimingCameraFollowOffsetX => _aimingCameraFollowOffsetX;
        public float AimingCameraFollowOffsetY => _aimingCameraFollowOffsetY;
        public float AimingCameraFollowOffsetZ => _aimingCameraFollowOffsetZ;
        public float AimingCameraDampingX => _aimingCameraDampingX;
        public float AimingCameraDampingY => _aimingCameraDampingY;
        public float AimingCameraDampingZ => _aimingCameraDampingZ;
        public float AimingCameraPitchDamping => _aimingCameraPitchDamping;
        public float AimingCameraYawDamping => _aimingCameraYawDamping;
        public float AimingCameraRollDamping => _aimingCameraRollDamping;
        public float AimingCameraTrackedOffsetX => _aimingCameraTrackedOffsetX;
        public float AimingCameraTrackedOffsetY => _aimingCameraTrackedOffsetY;
        public float AimingCameraTrackedOffsetZ => _aimingCameraTrackedOffsetZ;
        public float AimingCameraLookaheadTime => _aimingCameraLookaheadTime;
        public float AimingCameraLookaheadSmoothing => _aimingCameraLookaheadSmoothing;
        public float AimingCameraHorizontalDamping => _aimingCameraHorizontalDamping;
        public float AimingCameraVerticalDamping => _aimingCameraVerticalDamping;
        public float AimingCameraScreenX => _aimingCameraScreenX;
        public float AimingCameraScreenY => _aimingCameraScreenY;
        public float AimingCameraDeadZoneWidth => _aimingCameraDeadZoneWidth;
        public float AimingCameraDeadZoneHeight => _aimingCameraDeadZoneHeight;
        public float AimingCameraSoftZoneWidth => _aimingCameraSoftZoneWidth;
        public float AimingCameraSoftZoneHeight => _aimingCameraSoftZoneHeight;
        public float AimingCameraBiasX => _aimingCameraBiasX;
        public float AimingCameraBiasY => _aimingCameraBiasY;

        public float CameraTargetForwardMovementDistance => _cameraTargetForwardMovementDistance;
        public float CameraTargetSpeedX => _cameraTargetSpeedX;
        public float CameraTargetSpeedY => _cameraTargetSpeedY;

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

        public void SaveCameraSettings(CinemachineFreeLook freeLookCamera, CinemachineVirtualCamera targetCamera, 
            CinemachineVirtualCamera dialogCamera)
        {
            if (freeLookCamera == null || targetCamera == null || dialogCamera == null)
            {
                throw new NullReferenceException("Can't save camera settings, some cameras are null!");
            }

            CinemachineComposer bottomComposer = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineComposer>();
            CinemachineComposer middleComposer = freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
            CinemachineComposer topComposer = freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineComposer>();
            CinemachineCollider collider = freeLookCamera.GetComponent<CinemachineCollider>();

            CinemachineFreeLook.Orbit bottomOrbit = freeLookCamera.m_Orbits[0];
            CinemachineFreeLook.Orbit middleOrbit = freeLookCamera.m_Orbits[1];
            CinemachineFreeLook.Orbit topOrbit = freeLookCamera.m_Orbits[2];

            CinemachineTransposer targetTransposer = targetCamera.GetCinemachineComponent<CinemachineTransposer>();
            CinemachineComposer targetComposer = targetCamera.GetCinemachineComponent<CinemachineComposer>();
            CinemachineTransposer dialogTransposer = dialogCamera.GetCinemachineComponent<CinemachineTransposer>();
            CinemachineComposer dialogComposer = dialogCamera.GetCinemachineComponent<CinemachineComposer>();

            _isInvertedAxisX = freeLookCamera.m_XAxis.m_InvertAxis;
            _isInvertedAxisY = freeLookCamera.m_YAxis.m_InvertAxis;

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

            _targetCameraFieldOfView = targetCamera.m_Lens.FieldOfView;
            _targetCameraNearClipPlane = targetCamera.m_Lens.NearClipPlane;
            _targetCameraFarClipPlane = targetCamera.m_Lens.FarClipPlane;
            _targetCameraDutch = targetCamera.m_Lens.Dutch;
            _targetCameraFollowOffsetX = targetTransposer.m_FollowOffset.x;
            _targetCameraFollowOffsetY = targetTransposer.m_FollowOffset.y;
            _targetCameraFollowOffsetZ = targetTransposer.m_FollowOffset.z;
            _targetCameraDampingX = targetTransposer.m_XDamping;
            _targetCameraDampingY = targetTransposer.m_YDamping;
            _targetCameraDampingZ = targetTransposer.m_ZDamping;
            _targetCameraPitchDamping = targetTransposer.m_PitchDamping;
            _targetCameraYawDamping = targetTransposer.m_YawDamping;
            _targetCameraRollDamping = targetTransposer.m_RollDamping;
            _targetCameraTrackedOffsetX = targetComposer.m_TrackedObjectOffset.x;
            _targetCameraTrackedOffsetY = targetComposer.m_TrackedObjectOffset.y;
            _targetCameraTrackedOffsetZ = targetComposer.m_TrackedObjectOffset.z;
            _targetCameraLookaheadTime = targetComposer.m_LookaheadTime;
            _targetCameraLookaheadSmoothing = targetComposer.m_LookaheadSmoothing;
            _targetCameraHorizontalDamping = targetComposer.m_HorizontalDamping;
            _targetCameraVerticalDamping = targetComposer.m_VerticalDamping;
            _targetCameraScreenX = targetComposer.m_ScreenX;
            _targetCameraScreenY = targetComposer.m_ScreenY;
            _targetCameraDeadZoneWidth = targetComposer.m_DeadZoneWidth;
            _targetCameraDeadZoneHeight = targetComposer.m_DeadZoneHeight;
            _targetCameraSoftZoneWidth = targetComposer.m_SoftZoneWidth;
            _targetCameraSoftZoneHeight = targetComposer.m_SoftZoneHeight;
            _targetCameraBiasX = targetComposer.m_BiasX;
            _targetCameraBiasY = targetComposer.m_BiasY;

            _aimingCameraFieldOfView = dialogCamera.m_Lens.FieldOfView;
            _aimingCameraNearClipPlane = dialogCamera.m_Lens.NearClipPlane;
            _aimingCameraFarClipPlane = dialogCamera.m_Lens.FarClipPlane;
            _aimingCameraDutch = dialogCamera.m_Lens.Dutch;
            _aimingCameraFollowOffsetX = dialogTransposer.m_FollowOffset.x;
            _aimingCameraFollowOffsetY = dialogTransposer.m_FollowOffset.y;
            _aimingCameraFollowOffsetZ = dialogTransposer.m_FollowOffset.z;
            _aimingCameraDampingX = dialogTransposer.m_XDamping;
            _aimingCameraDampingY = dialogTransposer.m_YDamping;
            _aimingCameraDampingZ = dialogTransposer.m_ZDamping;
            _aimingCameraPitchDamping = dialogTransposer.m_PitchDamping;
            _aimingCameraYawDamping = dialogTransposer.m_YawDamping;
            _aimingCameraRollDamping = dialogTransposer.m_RollDamping;
            _aimingCameraTrackedOffsetX = dialogComposer.m_TrackedObjectOffset.x;
            _aimingCameraTrackedOffsetY = dialogComposer.m_TrackedObjectOffset.y;
            _aimingCameraTrackedOffsetZ = dialogComposer.m_TrackedObjectOffset.z;
            _aimingCameraLookaheadTime = dialogComposer.m_LookaheadTime;
            _aimingCameraLookaheadSmoothing = dialogComposer.m_LookaheadSmoothing;
            _aimingCameraHorizontalDamping = dialogComposer.m_HorizontalDamping;
            _aimingCameraVerticalDamping = dialogComposer.m_VerticalDamping;
            _aimingCameraScreenX = dialogComposer.m_ScreenX;
            _aimingCameraScreenY = dialogComposer.m_ScreenY;
            _aimingCameraDeadZoneWidth = dialogComposer.m_DeadZoneWidth;
            _aimingCameraDeadZoneHeight = dialogComposer.m_DeadZoneHeight;
            _aimingCameraSoftZoneWidth = dialogComposer.m_SoftZoneWidth;
            _aimingCameraSoftZoneHeight = dialogComposer.m_SoftZoneHeight;
            _aimingCameraBiasX = dialogComposer.m_BiasX;
            _aimingCameraBiasY = dialogComposer.m_BiasY;
        }

        public Camera CreateCharacterCamera(Transform parent = null)
        {
            Camera characterCamera;

            if (parent == null)
            {
                characterCamera = GameObject.Instantiate(CharacterCamera, parent);
            }
            else
            {
                characterCamera = GameObject.Instantiate(CharacterCamera);
            }
            
            characterCamera.name = CharacterCameraName;
            return characterCamera;
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

            characterFreelookCamera.m_Lens.FieldOfView = FreeLookFieldOfView;
            characterFreelookCamera.m_Lens.NearClipPlane = FreeLookNearClipPlane;
            characterFreelookCamera.m_Lens.FarClipPlane = FreeLookFarClipPlane;
            characterFreelookCamera.m_Lens.Dutch = FreeLookDutch;

            characterFreelookCamera.m_XAxis.m_InvertAxis = IsInvertedX;
            characterFreelookCamera.m_XAxis.m_MaxSpeed = CameraSpeedX;
            characterFreelookCamera.m_XAxis.m_AccelTime = CameraAccelerationLagX;
            characterFreelookCamera.m_XAxis.m_DecelTime = CameraDecelerationLagX;

            characterFreelookCamera.m_YAxis.m_InvertAxis = IsInvertedY;
            characterFreelookCamera.m_YAxis.m_MaxSpeed = CameraSpeedY;
            characterFreelookCamera.m_YAxis.m_AccelTime = CameraAccelerationLagY;
            characterFreelookCamera.m_YAxis.m_DecelTime = CameraDecelerationLagY;

            characterFreelookCamera.m_SplineCurvature = FreeLookSplineCurvature;

            for (var rig = 0; rig < 3; rig++)
            {
                CinemachineVirtualCamera cinemachineRig = characterFreelookCamera.GetRig(rig);
                cinemachineRig.LookAt = lookAtTransform;

                CinemachineOrbitalTransposer transposerFromData = GetFreeCameraOrbitalTransposer(rig);
                CinemachineOrbitalTransposer transposerFromCamera = cinemachineRig.
                    GetCinemachineComponent<CinemachineOrbitalTransposer>();

                transposerFromCamera.m_YDamping = transposerFromData.m_YDamping;
                transposerFromCamera.m_ZDamping = transposerFromData.m_ZDamping;

                CinemachineComposer composerFromData = GetFreeCameraComposer(rig);
                CinemachineComposer composerFromCamera = cinemachineRig.GetCinemachineComponent<CinemachineComposer>();

                composerFromCamera.m_TrackedObjectOffset.x = composerFromData.m_TrackedObjectOffset.x;
                composerFromCamera.m_TrackedObjectOffset.y = composerFromData.m_TrackedObjectOffset.y;
                composerFromCamera.m_TrackedObjectOffset.z = composerFromData.m_TrackedObjectOffset.z;
                composerFromCamera.m_LookaheadTime = composerFromData.m_LookaheadTime;
                composerFromCamera.m_LookaheadSmoothing = composerFromData.m_LookaheadSmoothing;
                composerFromCamera.m_HorizontalDamping = composerFromData.m_HorizontalDamping;
                composerFromCamera.m_VerticalDamping = composerFromData.m_VerticalDamping;
                composerFromCamera.m_ScreenX = composerFromData.m_ScreenX;
                composerFromCamera.m_ScreenY = composerFromData.m_ScreenY;
                composerFromCamera.m_DeadZoneWidth = composerFromData.m_DeadZoneWidth;
                composerFromCamera.m_DeadZoneHeight = composerFromData.m_DeadZoneHeight;
                composerFromCamera.m_SoftZoneWidth = composerFromData.m_SoftZoneWidth;
                composerFromCamera.m_SoftZoneHeight = composerFromData.m_SoftZoneHeight;
                composerFromCamera.m_BiasX = composerFromData.m_BiasX;
                composerFromCamera.m_BiasY = composerFromData.m_BiasY;

                CinemachineFreeLook.Orbit currentOrbit = GetFreeCameraOrbit(rig);
                characterFreelookCamera.m_Orbits[rig] = currentOrbit;
            }

            CinemachineCollider colliderFromCamera = characterFreelookCamera.GetComponent<CinemachineCollider>();
            colliderFromCamera.m_MinimumDistanceFromTarget = ColliderMinDistanceFromTarget;
            colliderFromCamera.m_AvoidObstacles = IsColliderAvoidingObstacles;
            colliderFromCamera.m_DistanceLimit = ColliderDistanceLimit;
            colliderFromCamera.m_CameraRadius = ColliderCameraRadius;
            colliderFromCamera.m_Damping = ColliderDamping;
            colliderFromCamera.m_OptimalTargetDistance = ColliderOptimalTargetDistance;

            return characterFreelookCamera;
        }

        public CinemachineFreeLook CreateCharacterKnockedDownCamera(Transform followTransform, Transform lookAtTransform, 
            Transform parent = null)
        {
            CinemachineFreeLook characterKnockedDownCamera;

            if(parent == null)
            {
                characterKnockedDownCamera = GameObject.Instantiate(CharacterFreelookCamera);
            }
            else
            {
                characterKnockedDownCamera = GameObject.Instantiate(CharacterFreelookCamera, parent);
            }

            characterKnockedDownCamera.name = CharacterKnockedDownCameraName;

            characterKnockedDownCamera.Follow = followTransform;
            characterKnockedDownCamera.LookAt = lookAtTransform;

            return characterKnockedDownCamera;
        }

        public CinemachineVirtualCamera CreateCharacterTargetCamera(Transform followTransform, Transform lookAtTransform, 
            Transform parent = null)
        {
            CinemachineVirtualCamera characterTargetCamera;

            if(parent == null)
            {
                characterTargetCamera = GameObject.Instantiate(CharacterTargetCamera);
            }
            else
            {
                characterTargetCamera = GameObject.Instantiate(CharacterTargetCamera, parent);
            }

            characterTargetCamera.name = CharacterTargetCameraName;

            characterTargetCamera.Follow = followTransform;
            characterTargetCamera.LookAt = lookAtTransform;

            characterTargetCamera.m_Lens.FieldOfView = TargetCameraFieldOfView;
            characterTargetCamera.m_Lens.NearClipPlane = TargetCameraNearClipPlane;
            characterTargetCamera.m_Lens.FarClipPlane = TargetCameraFarClipPlane;
            characterTargetCamera.m_Lens.Dutch = TargetCameraDutch;

            CinemachineTransposer targetTransposer = characterTargetCamera.
                GetCinemachineComponent<CinemachineTransposer>();

            targetTransposer.m_FollowOffset.x = TargetCameraFollowOffsetX;
            targetTransposer.m_FollowOffset.y = TargetCameraFollowOffsetY;
            targetTransposer.m_FollowOffset.z = TargetCameraFollowOffsetZ;

            targetTransposer.m_XDamping = TargetCameraDampingX;
            targetTransposer.m_YDamping = TargetCameraDampingY;
            targetTransposer.m_ZDamping = TargetCameraDampingZ;
            targetTransposer.m_PitchDamping = TargetCameraPitchDamping;
            targetTransposer.m_YawDamping = TargetCameraYawDamping;
            targetTransposer.m_RollDamping = TargetCameraRollDamping;

            CinemachineComposer targetComposer = characterTargetCamera.
                GetCinemachineComponent<CinemachineComposer>();

            targetComposer.m_TrackedObjectOffset.x = TargetCameraTrackedOffsetX;
            targetComposer.m_TrackedObjectOffset.y = TargetCameraTrackedOffsetY;
            targetComposer.m_TrackedObjectOffset.z = TargetCameraTrackedOffsetZ;
            targetComposer.m_LookaheadTime = TargetCameraLookaheadTime;
            targetComposer.m_LookaheadSmoothing = TargetCameraLookaheadSmoothing;
            targetComposer.m_HorizontalDamping = TargetCameraHorizontalDamping;
            targetComposer.m_VerticalDamping = TargetCameraVerticalDamping;
            targetComposer.m_ScreenX = TargetCameraScreenX;
            targetComposer.m_ScreenY = TargetCameraScreenY;
            targetComposer.m_DeadZoneWidth = TargetCameraDeadZoneWidth;
            targetComposer.m_DeadZoneHeight = TargetCameraDeadZoneHeight;
            targetComposer.m_SoftZoneWidth = TargetCameraSoftZoneWidth;
            targetComposer.m_SoftZoneHeight = TargetCameraSoftZoneHeight;
            targetComposer.m_BiasX = TargetCameraBiasX;
            targetComposer.m_BiasY = TargetCameraBiasY;

            return characterTargetCamera;
        }

        public CinemachineVirtualCamera CreateCharacterAimingCamera(Transform followTransform, Transform lookAtTransform, 
            Transform parent = null)
        {
            CinemachineVirtualCamera characterAimingCamera;

            if(parent == null)
            {
                characterAimingCamera = GameObject.Instantiate(CharacterAimingCamera);
            }
            else
            {
                characterAimingCamera = GameObject.Instantiate(CharacterAimingCamera, parent);
            }

            characterAimingCamera.name = CharacterAimingCameraName;

            characterAimingCamera.Follow = followTransform;
            characterAimingCamera.LookAt = lookAtTransform;

            characterAimingCamera.m_Lens.FieldOfView = AimingCameraFieldOfView;
            characterAimingCamera.m_Lens.NearClipPlane = AimingCameraNearClipPlane;
            characterAimingCamera.m_Lens.FarClipPlane = AimingCameraFarClipPlane;
            characterAimingCamera.m_Lens.Dutch = AimingCameraDutch;

            CinemachineTransposer aimingTransposer = characterAimingCamera.
                GetCinemachineComponent<CinemachineTransposer>();

            aimingTransposer.m_FollowOffset.x = AimingCameraFollowOffsetX;
            aimingTransposer.m_FollowOffset.y = AimingCameraFollowOffsetY;
            aimingTransposer.m_FollowOffset.z = AimingCameraFollowOffsetZ;

            aimingTransposer.m_XDamping = AimingCameraDampingX;
            aimingTransposer.m_YDamping = AimingCameraDampingY;
            aimingTransposer.m_ZDamping = AimingCameraDampingZ;
            aimingTransposer.m_PitchDamping = AimingCameraPitchDamping;
            aimingTransposer.m_YawDamping = AimingCameraYawDamping;
            aimingTransposer.m_RollDamping = AimingCameraRollDamping;

            CinemachineComposer aimingComposer = characterAimingCamera.
                GetCinemachineComponent<CinemachineComposer>();

            aimingComposer.m_TrackedObjectOffset.x = AimingCameraTrackedOffsetX;
            aimingComposer.m_TrackedObjectOffset.y = AimingCameraTrackedOffsetY;
            aimingComposer.m_TrackedObjectOffset.z = AimingCameraTrackedOffsetZ;
            aimingComposer.m_LookaheadTime = AimingCameraLookaheadTime;
            aimingComposer.m_LookaheadSmoothing = AimingCameraLookaheadSmoothing;
            aimingComposer.m_HorizontalDamping = AimingCameraHorizontalDamping;
            aimingComposer.m_VerticalDamping = AimingCameraVerticalDamping;
            aimingComposer.m_ScreenX = AimingCameraScreenX;
            aimingComposer.m_ScreenY = AimingCameraScreenY;
            aimingComposer.m_DeadZoneWidth = AimingCameraDeadZoneWidth;
            aimingComposer.m_DeadZoneHeight = AimingCameraDeadZoneHeight;
            aimingComposer.m_SoftZoneWidth = AimingCameraSoftZoneWidth;
            aimingComposer.m_SoftZoneHeight = AimingCameraSoftZoneHeight;
            aimingComposer.m_BiasX = AimingCameraBiasX;
            aimingComposer.m_BiasY = AimingCameraBiasY;

            return characterAimingCamera;
        }

        #endregion
    }
}


