using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Character/CameraData")]
    public sealed class CameraData : ScriptableObject
    {
        #region FIelds

        [Tooltip("Character camera.")]
        [SerializeField] private Camera _characterCamera;

        [Tooltip("Character camera name.")]
        [SerializeField] private string _characterCameraName;

        [Tooltip("Character free look camra")]
        [SerializeField] private FreeLookCameraSettings _characterFreeLookCameraSettings;

        [Tooltip("Character targeting camera")]
        [SerializeField] private TargetingCameraSettings _characterTargetingCameraSettings;

        [Tooltip("Character aiming camera camra")]
        [SerializeField] private AimingCameraSettings _characterAimingCameraSettings;

        #endregion


        #region Properties

        public Camera CharacterCamera => _characterCamera;
        public string CharacterCameraName => _characterCameraName;
        public FreeLookCameraSettings CharacterFreelookCameraSettings => _characterFreeLookCameraSettings;
        public TargetingCameraSettings CharacterTargetingCameraSettings => _characterTargetingCameraSettings;
        public AimingCameraSettings CharacterAimingCameraSettings => _characterAimingCameraSettings;
        public KnockedDownCameraSettings CharacterKnockedDownCameraSettings { get; }

        #endregion


        #region Methods

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

        #endregion
    }
}

