using UnityEngine;
using Cinemachine;
using System;
using System.IO;

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

        [Tooltip("Character target camera JSON file name.")]
        [SerializeField] private string _characterTargetCameraJSONFilePath;

        [Range(0.0f, 10.0f)]
        [Tooltip("Target camera blend time between 0 and 10.")]
        [SerializeField] private float _characterTargetCameraBlendTime;

        #endregion


        #region Properties

        public CinemachineVirtualCamera CharacterTargetCamera => _characterTargetCamera;
        public string CharacterTargetCameraName => _characterTargetCameraName;
        public float CharacterTargetCameraBlendTime => _characterTargetCameraBlendTime;

        #endregion


        #region Methods

        public void SaveCharacterTargetingCameraSettings(CinemachineVirtualCamera targetCamera, bool hasToBeCamera)
        {
            if (targetCamera == null)
            {
                if (hasToBeCamera)
                {
                    throw new NullReferenceException("Can't save target camera settings, argument is null!");
                }
                else
                {
                    Debug.LogWarning("Target camera was not saved cause it was not implemented");
                }
            }
            else
            {
                string parametersDataString = JsonUtility.ToJson(targetCamera);
                File.WriteAllText(Path.Combine(Application.dataPath, _characterTargetCameraJSONFilePath),
                    parametersDataString);
            }
        }

        public CinemachineVirtualCamera CreateCharacterTargetingCamera(Transform followTransform, Transform lookAtTransform,
                Transform parent = null)
        {
            CinemachineVirtualCamera characterTargetCameraObject;

            if (parent == null)
            {
                characterTargetCameraObject = GameObject.Instantiate(CharacterTargetCamera);
            }
            else
            {
                characterTargetCameraObject = GameObject.Instantiate(CharacterTargetCamera, parent);
            }

            if (File.Exists(Path.Combine(Application.dataPath, _characterTargetCameraJSONFilePath)))
            {
                JsonUtility.FromJsonOverwrite(File.
                    ReadAllText(Path.Combine(Application.dataPath, _characterTargetCameraJSONFilePath)),
                        characterTargetCameraObject);
            }
            else
            {
                Debug.LogWarning("Can't find targeting camera settings file!");
            }

            characterTargetCameraObject.name = CharacterTargetCameraName;
            characterTargetCameraObject.Follow = followTransform;
            characterTargetCameraObject.LookAt = lookAtTransform;

            return characterTargetCameraObject;
        }

        #endregion
    }
}