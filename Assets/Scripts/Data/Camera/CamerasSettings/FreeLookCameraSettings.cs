using System;
using UnityEngine;
using Cinemachine;
using System.IO;


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

        [Tooltip("Character free look camera JSON file name.")]
        [SerializeField] private string _characterFreelookCameraJSONFileName;

        [Range(0.0f, 10.0f)]
        [Tooltip("Freelook camera blend time between 0 and 10.")]
        [SerializeField] private float _characterFreelookCameraBlendTime;

        #endregion


        #region Properties

        public CinemachineFreeLook CharacterFreelookCamera => _characterFreelookCamera;
 
        public string CharacterFreelookCameraName => _characterFreelookCameraName;
        public float CharacterFreelookCameraBlendTime => _characterFreelookCameraBlendTime;  

        #endregion


        #region Methods

        public void SaveCharacterFreelookCameraSettings(CinemachineFreeLook freeLookCamera)
        {
            if (freeLookCamera == null)
            {
                throw new NullReferenceException("Can't save free look camera settings - argument is null!");
            }

            string parametersDataString = JsonUtility.ToJson(freeLookCamera);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, _characterFreelookCameraJSONFileName), 
                parametersDataString);
        }

        public CinemachineFreeLook CreateCharacterFreelookCamera(Transform followTransform, Transform lookAtTransform, 
            Transform parent = null)
        {
            CinemachineFreeLook characterFreelookCameraObject;

            if (parent == null)
            {
                characterFreelookCameraObject = GameObject.Instantiate(CharacterFreelookCamera);
            }
            else
            {
                characterFreelookCameraObject = GameObject.Instantiate(CharacterFreelookCamera, parent);
            }

            if (File.Exists(Path.Combine(Application.persistentDataPath, _characterFreelookCameraJSONFileName)))
            {
                JsonUtility.FromJsonOverwrite(File.
                    ReadAllText(Path.Combine(Application.persistentDataPath, _characterFreelookCameraJSONFileName)), 
                        characterFreelookCameraObject);
            }
            else
            {
                Debug.LogWarning("Can't find free look camera settings file!");
            }

            characterFreelookCameraObject.Follow = followTransform;
            characterFreelookCameraObject.LookAt = lookAtTransform;
            characterFreelookCameraObject.name = CharacterFreelookCameraName;

            return characterFreelookCameraObject;
        }

        #endregion
    }
}


