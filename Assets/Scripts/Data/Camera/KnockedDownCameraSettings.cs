using UnityEngine;
using Cinemachine;
using System;


namespace BeastHunter
{
    [Serializable]
    public struct KnockedDownCameraSettings
    {
        #region Fields

        [Tooltip("Character free look knocked down camera.")]
        [SerializeField] private CinemachineFreeLook _characterKnockedDownCamera;

        [Tooltip("Character knocked down camera name.")]
        [SerializeField] private string _characterKnockedDownCameraName;

        [Range(0.0f, 10.0f)]
        [Tooltip("Knocked down camera blend time between 0 and 10.")]
        [SerializeField] private float _characterKnockedDownCameraBlendTime;

        [Header("Knocked down camera collider settings")]

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

        public CinemachineFreeLook CharacterKnockedDownCamera => _characterKnockedDownCamera;
        public string CharacterKnockedDownCameraName => _characterKnockedDownCameraName;
        public float CharacterKnockedDownCameraBlendTime => _characterKnockedDownCameraBlendTime;

        public float ColliderMinDistanceFromTarget => _colliderMinDistanceFromTarget;
        public bool IsColliderAvoidingObstacles => _isColliderAvoidingObstacles;
        public float ColliderDistanceLimit => _colliderDistanceLimit;
        public float ColliderCameraRadius => _colliderCameraRadius;
        public float ColliderDamping => _colliderDamping;
        public float ColliderOptimalTargetDistance => _colliderOptimalTargetDistance;

        #endregion


        #region Methods

        public CinemachineFreeLook CreateCharacterKnockedDownCamera(Transform followTransform, Transform lookAtTransform,
            Transform parent = null)
        {
            CinemachineFreeLook characterKnockeDownCamera;

            if (parent == null)
            {
                characterKnockeDownCamera = GameObject.Instantiate(CharacterKnockedDownCamera);
            }
            else
            {
                characterKnockeDownCamera = GameObject.Instantiate(CharacterKnockedDownCamera, parent);
            }

            characterKnockeDownCamera.name = CharacterKnockedDownCameraName;

            characterKnockeDownCamera.Follow = followTransform;
            characterKnockeDownCamera.LookAt = lookAtTransform;

            //for (var rig = 0; rig < 3; rig++)
            //{
            //    CinemachineVirtualCamera cinemachineRig = characterKnockeDownCamera.GetRig(rig);
            //    cinemachineRig.LookAt = lookAtTransform;
            //}

            //CinemachineCollider colliderFromCamera = characterKnockeDownCamera.GetComponent<CinemachineCollider>();
            //colliderFromCamera.m_MinimumDistanceFromTarget = ColliderMinDistanceFromTarget;
            //colliderFromCamera.m_AvoidObstacles = IsColliderAvoidingObstacles;
            //colliderFromCamera.m_DistanceLimit = ColliderDistanceLimit;
            //colliderFromCamera.m_CameraRadius = ColliderCameraRadius;
            //colliderFromCamera.m_Damping = ColliderDamping;
            //colliderFromCamera.m_OptimalTargetDistance = ColliderOptimalTargetDistance;

            return characterKnockeDownCamera;
        }

        #endregion
    }
}

