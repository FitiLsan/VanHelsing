using UnityEngine;
using Cinemachine;
using System;


namespace BeastHunter
{
    [Serializable]
    public struct AimingCameraSettings
    {
        #region Fields

        [Tooltip("Character aiming camera.")]
        [SerializeField] private CinemachineVirtualCamera _characterAimingCamera;

        [Tooltip("Character aiming camera name.")]
        [SerializeField] private string _characterAimingCameraName;

        [Range(0.0f, 10.0f)]
        [Tooltip("Aiming camera blend time between 0 and 10.")]
        [SerializeField] private float _characterAimingCameraBlendTime;

        [Header("Lens Settings")]
        [Header("Aiming camera Settings")]

        [Tooltip("Aim canvas prefab")]
        [SerializeField] private GameObject _aimCanvasPrefab;

        [Tooltip("Aim target cross prefab")]
        [SerializeField] private GameObject _aimProjectileLinePrefab;

        [Tooltip("Aim dot prefab")]
        [SerializeField] private GameObject _aimDotPrefab;

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

        [Header("Camera aiming target settings")]

        [Tooltip("Camera target name.")]
        [SerializeField] private string _cameraTargetName;

        [Range(0.0f, 5.0f)]
        [Tooltip("Camera target height between 0 and 5.")]
        [SerializeField] private float _cameraTargetHeight;

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

        public CinemachineVirtualCamera CharacterAimingCamera => _characterAimingCamera;
        public string CharacterAimingCameraName => _characterAimingCameraName;
        public float CharacterAimingCameraBlendTIme => _characterAimingCameraBlendTime;

        public Vector2 CameraTargetDistanceMoveX => _cameraTargetDistanceMoveX;
        public Vector2 CameraTargetDistanceMoveY => _cameraTargetDistanceMoveY;
        public GameObject AimCanvasPrefab => _aimCanvasPrefab;
        public GameObject AimProjectileLinePrefab => _aimProjectileLinePrefab;
        public GameObject AimDotPrefab => _aimDotPrefab;

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

        public string CameraTargetName => _cameraTargetName;
        public float CameraTargetHeight => _cameraTargetHeight;
        public float CameraTargetForwardMovementDistance => _cameraTargetForwardMovementDistance;
        public float CameraTargetSpeedX => _cameraTargetSpeedX;
        public float CameraTargetSpeedY => _cameraTargetSpeedY;

        #endregion


        #region Methods

        public void SaveAimingCameraSettings(CinemachineVirtualCamera aimingCamera)
        {
            if(aimingCamera == null)
            {
                throw new NullReferenceException("Can't save aiming camera settings, argument is null!");
            }

            CinemachineTransposer dialogTransposer = aimingCamera.GetCinemachineComponent<CinemachineTransposer>();
            CinemachineComposer dialogComposer = aimingCamera.GetCinemachineComponent<CinemachineComposer>();

            _aimingCameraFieldOfView = aimingCamera.m_Lens.FieldOfView;
            _aimingCameraNearClipPlane = aimingCamera.m_Lens.NearClipPlane;
            _aimingCameraFarClipPlane = aimingCamera.m_Lens.FarClipPlane;
            _aimingCameraDutch = aimingCamera.m_Lens.Dutch;
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

        public CinemachineVirtualCamera CreateCharacterAimingCamera(Transform followTransform, Transform lookAtTransform,
            Transform parent = null)
        {
            CinemachineVirtualCamera characterAimingCamera;

            if (parent == null)
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

            //characterAimingCamera.m_Lens.FieldOfView = AimingCameraFieldOfView;
            //characterAimingCamera.m_Lens.NearClipPlane = AimingCameraNearClipPlane;
            //characterAimingCamera.m_Lens.FarClipPlane = AimingCameraFarClipPlane;
            //characterAimingCamera.m_Lens.Dutch = AimingCameraDutch;

            //CinemachineTransposer aimingTransposer = characterAimingCamera.
            //    GetCinemachineComponent<CinemachineTransposer>();

            //aimingTransposer.m_FollowOffset.x = AimingCameraFollowOffsetX;
            //aimingTransposer.m_FollowOffset.y = AimingCameraFollowOffsetY;
            //aimingTransposer.m_FollowOffset.z = AimingCameraFollowOffsetZ;

            //aimingTransposer.m_XDamping = AimingCameraDampingX;
            //aimingTransposer.m_YDamping = AimingCameraDampingY;
            //aimingTransposer.m_ZDamping = AimingCameraDampingZ;
            //aimingTransposer.m_PitchDamping = AimingCameraPitchDamping;
            //aimingTransposer.m_YawDamping = AimingCameraYawDamping;
            //aimingTransposer.m_RollDamping = AimingCameraRollDamping;

            //CinemachineComposer aimingComposer = characterAimingCamera.
            //    GetCinemachineComponent<CinemachineComposer>();

            //aimingComposer.m_TrackedObjectOffset.x = AimingCameraTrackedOffsetX;
            //aimingComposer.m_TrackedObjectOffset.y = AimingCameraTrackedOffsetY;
            //aimingComposer.m_TrackedObjectOffset.z = AimingCameraTrackedOffsetZ;
            //aimingComposer.m_LookaheadTime = AimingCameraLookaheadTime;
            //aimingComposer.m_LookaheadSmoothing = AimingCameraLookaheadSmoothing;
            //aimingComposer.m_HorizontalDamping = AimingCameraHorizontalDamping;
            //aimingComposer.m_VerticalDamping = AimingCameraVerticalDamping;
            //aimingComposer.m_ScreenX = AimingCameraScreenX;
            //aimingComposer.m_ScreenY = AimingCameraScreenY;
            //aimingComposer.m_DeadZoneWidth = AimingCameraDeadZoneWidth;
            //aimingComposer.m_DeadZoneHeight = AimingCameraDeadZoneHeight;
            //aimingComposer.m_SoftZoneWidth = AimingCameraSoftZoneWidth;
            //aimingComposer.m_SoftZoneHeight = AimingCameraSoftZoneHeight;
            //aimingComposer.m_BiasX = AimingCameraBiasX;
            //aimingComposer.m_BiasY = AimingCameraBiasY;

            return characterAimingCamera;
        }

        #endregion
    }
}

