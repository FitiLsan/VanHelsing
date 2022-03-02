using UnityEngine;
using Cinemachine;
using System;
using System.IO;


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

        [Tooltip("Character knocked down camera JSON file name.")]
        [SerializeField] private string _characterKnockedDownCameraJSONFilePath;

        [Range(0.0f, 10.0f)]
        [Tooltip("Knocked down camera blend time between 0 and 10.")]
        [SerializeField] private float _characterKnockedDownCameraBlendTime;

        #endregion


        #region Properties

        public CinemachineFreeLook CharacterKnockedDownCamera => _characterKnockedDownCamera;
        public string CharacterKnockedDownCameraName => _characterKnockedDownCameraName;
        public float CharacterKnockedDownCameraBlendTime => _characterKnockedDownCameraBlendTime;

        #endregion


        #region Methods

        public void SaveCharacterKnockedDownSettings(CinemachineVirtualCamera targetCamera, bool hasToBeCamera)
        {
            if (targetCamera == null)
            {              
                if (hasToBeCamera)
                {
                    throw new NullReferenceException("Can't save knocked down camera settings, argument is null!");
                }
                else
                {
                    Debug.LogWarning("Knocked down camera was not saved cause it was not implemented");
                }
            }
            else
            {
                string parametersDataString = JsonUtility.ToJson(targetCamera);
                File.WriteAllText(Path.Combine(Application.dataPath, _characterKnockedDownCameraJSONFilePath),
                    parametersDataString);
            }
        }

        public CinemachineFreeLook CreateCharacterKnockedDownCamera(Transform followTransform, Transform lookAtTransform,
            Transform parent = null)
        {
            CinemachineFreeLook characterKnockeDownCameraObject;

            if (parent == null)
            {
                characterKnockeDownCameraObject = GameObject.Instantiate(CharacterKnockedDownCamera);
            }
            else
            {
                characterKnockeDownCameraObject = GameObject.Instantiate(CharacterKnockedDownCamera, parent);
            }

            if (File.Exists(Path.Combine(Application.dataPath, _characterKnockedDownCameraJSONFilePath)))
            {
                JsonUtility.FromJsonOverwrite(File.
                    ReadAllText(Path.Combine(Application.dataPath, _characterKnockedDownCameraJSONFilePath)),
                        characterKnockeDownCameraObject);
            }
            else
            {
                Debug.LogWarning("Can't find targeting camera settings file!");
            }

            characterKnockeDownCameraObject.name = CharacterKnockedDownCameraName;
            characterKnockeDownCameraObject.Follow = followTransform;
            characterKnockeDownCameraObject.LookAt = lookAtTransform;

            return characterKnockeDownCameraObject;
        }

        #endregion
    }
}

