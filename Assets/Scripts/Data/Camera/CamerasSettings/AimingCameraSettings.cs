using UnityEngine;
using Cinemachine;
using System;
using System.IO;


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

        [Tooltip("Character aiming camera JSON file name.")]
        [SerializeField] private string _characterAimingCameraJSONFilePath;

        [Range(0.0f, 10.0f)]
        [Tooltip("Aiming camera blend time between 0 and 10.")]
        [SerializeField] private float _characterAimingCameraBlendTime;

        [Header("Lens Settings")]
        [Header("Aiming camera Settings")]

        [Tooltip("Aim target cross prefab")]
        [SerializeField] private GameObject _aimProjectileLinePrefab;

        [Tooltip("Aim dot prefab")]
        [SerializeField] private GameObject _aimDotPrefab;

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
        public GameObject AimProjectileLinePrefab => _aimProjectileLinePrefab;
        public GameObject AimDotPrefab => _aimDotPrefab;

        public string CameraTargetName => _cameraTargetName;
        public float CameraTargetHeight => _cameraTargetHeight;
        public float CameraTargetForwardMovementDistance => _cameraTargetForwardMovementDistance;
        public float CameraTargetSpeedX => _cameraTargetSpeedX;
        public float CameraTargetSpeedY => _cameraTargetSpeedY;

        #endregion


        #region Methods

        public void SaveCharacterAimingCameraSettings(CinemachineVirtualCamera aimingCamera, bool hasToBeCamera)
        {
            if(aimingCamera == null)
            {            
                if (hasToBeCamera)
                {
                    throw new NullReferenceException("Can't save aiming camera settings, argument is null!");
                }
                else
                {
                    Debug.LogWarning("Aiming camera was not saved cause it was not implemented");
                }
            }
            else
            {
                string parametersDataString = JsonUtility.ToJson(aimingCamera);
                File.WriteAllText(Path.Combine(Application.dataPath, _characterAimingCameraJSONFilePath),
                    parametersDataString);
            }
        }

        public CinemachineVirtualCamera CreateCharacterAimingCamera(Transform followTransform, Transform lookAtTransform,
            Transform parent = null)
        {
            CinemachineVirtualCamera characterAimingCameraObject;

            if (parent == null)
            {
                characterAimingCameraObject = GameObject.Instantiate(CharacterAimingCamera);
            }
            else
            {
                characterAimingCameraObject = GameObject.Instantiate(CharacterAimingCamera, parent);
            }

            if (File.Exists(Path.Combine(Application.dataPath, _characterAimingCameraJSONFilePath)))
            {
                JsonUtility.FromJsonOverwrite(File.
                    ReadAllText(Path.Combine(Application.dataPath, _characterAimingCameraJSONFilePath)),
                        characterAimingCameraObject);
            }
            else
            {
                Debug.LogWarning("Can't find aiming camera settings file!");
            }

            characterAimingCameraObject.name = CharacterAimingCameraName;
            characterAimingCameraObject.Follow = followTransform;
            characterAimingCameraObject.LookAt = lookAtTransform;

            return characterAimingCameraObject;
        }

        #endregion
    }
}

