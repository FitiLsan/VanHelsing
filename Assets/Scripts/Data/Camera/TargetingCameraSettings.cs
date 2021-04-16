using UnityEngine;
using Cinemachine;
using System;


namespace BeastHunter
{
    [Serializable]
    public struct TargetingCameraSettings
    {
        #region Fields

        [Tooltip("Character target camera.")]
        [SerializeField] private CinemachineVirtualCamera _characterTargetCamera;

        [Tooltip("Character target camera name.")]
        [SerializeField] private string _characterTargetCameraName;

        [Range(0.0f, 10.0f)]
        [Tooltip("Target camera blend time between 0 and 10.")]
        [SerializeField] private float _characterTargetCameraBlendTime;

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

        #endregion


        #region Properties

        public CinemachineVirtualCamera CharacterTargetCamera => _characterTargetCamera;
        public string CharacterTargetCameraName => _characterTargetCameraName;
        public float CharacterTargetCameraBlendTime => _characterTargetCameraBlendTime;

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

        #endregion


        #region Methods

        public void SaveTargetCameraSettings(CinemachineVirtualCamera targetCamera)
        {
            if (targetCamera == null)
            {
                throw new NullReferenceException("Can't save target camera settings, argument is null!");
            }

            CinemachineTransposer targetTransposer = targetCamera.GetCinemachineComponent<CinemachineTransposer>();
            CinemachineComposer targetComposer = targetCamera.GetCinemachineComponent<CinemachineComposer>();

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
        }

        public CinemachineVirtualCamera CreateCharacterTargetCamera(Transform followTransform, Transform lookAtTransform,
                Transform parent = null)
        {
            CinemachineVirtualCamera characterTargetCamera;

            if (parent == null)
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

            //characterTargetCamera.m_Lens.FieldOfView = TargetCameraFieldOfView;
            //characterTargetCamera.m_Lens.NearClipPlane = TargetCameraNearClipPlane;
            //characterTargetCamera.m_Lens.FarClipPlane = TargetCameraFarClipPlane;
            //characterTargetCamera.m_Lens.Dutch = TargetCameraDutch;

            //CinemachineTransposer targetTransposer = characterTargetCamera.
            //    GetCinemachineComponent<CinemachineTransposer>();

            //targetTransposer.m_FollowOffset.x = TargetCameraFollowOffsetX;
            //targetTransposer.m_FollowOffset.y = TargetCameraFollowOffsetY;
            //targetTransposer.m_FollowOffset.z = TargetCameraFollowOffsetZ;

            //targetTransposer.m_XDamping = TargetCameraDampingX;
            //targetTransposer.m_YDamping = TargetCameraDampingY;
            //targetTransposer.m_ZDamping = TargetCameraDampingZ;
            //targetTransposer.m_PitchDamping = TargetCameraPitchDamping;
            //targetTransposer.m_YawDamping = TargetCameraYawDamping;
            //targetTransposer.m_RollDamping = TargetCameraRollDamping;

            //CinemachineComposer targetComposer = characterTargetCamera.
            //    GetCinemachineComponent<CinemachineComposer>();

            //targetComposer.m_TrackedObjectOffset.x = TargetCameraTrackedOffsetX;
            //targetComposer.m_TrackedObjectOffset.y = TargetCameraTrackedOffsetY;
            //targetComposer.m_TrackedObjectOffset.z = TargetCameraTrackedOffsetZ;
            //targetComposer.m_LookaheadTime = TargetCameraLookaheadTime;
            //targetComposer.m_LookaheadSmoothing = TargetCameraLookaheadSmoothing;
            //targetComposer.m_HorizontalDamping = TargetCameraHorizontalDamping;
            //targetComposer.m_VerticalDamping = TargetCameraVerticalDamping;
            //targetComposer.m_ScreenX = TargetCameraScreenX;
            //targetComposer.m_ScreenY = TargetCameraScreenY;
            //targetComposer.m_DeadZoneWidth = TargetCameraDeadZoneWidth;
            //targetComposer.m_DeadZoneHeight = TargetCameraDeadZoneHeight;
            //targetComposer.m_SoftZoneWidth = TargetCameraSoftZoneWidth;
            //targetComposer.m_SoftZoneHeight = TargetCameraSoftZoneHeight;
            //targetComposer.m_BiasX = TargetCameraBiasX;
            //targetComposer.m_BiasY = TargetCameraBiasY;

            return characterTargetCamera;
        }

        #endregion
    }
}