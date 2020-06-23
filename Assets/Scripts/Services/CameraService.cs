using UnityEngine;
using UnityEditor;
using Cinemachine;


namespace BeastHunter
{
    public sealed class CameraService : Service
    {
        #region FIelds

        private readonly GameContext _context;
        private CameraData _cameraData;

        #endregion


        #region Properties

        public Camera CharacterCamera { get; private set; }
        public CinemachineFreeLook CharacterFreelookCamera { get; private set; }
        public CinemachineVirtualCamera CharacterTargetCamera { get; private set; }
        public CinemachineVirtualCamera CharacterDialogCamera { get; private set; }
        public CinemachineBrain CameraCinemachineBrain { get; private set; }
        public CinemachineVirtualCameraBase CurrentActiveCamera { get; private set; }
        public CinemachineVirtualCameraBase PreviousActiveCamera { get; private set; }

        #endregion


        #region ClassLifeCycles

        public CameraService(Contexts contexts) : base(contexts)
        {
            _context = contexts as GameContext;
            _cameraData = Data.CameraData;

            CharacterCamera = _cameraData._cameraSettings.CreateCharacterCamera();
            CameraCinemachineBrain = CharacterCamera.GetComponent<CinemachineBrain>() ?? null;

#if (UNITY_EDITOR)
            EditorApplication.playModeStateChanged += SaveCameraSettings;
#endif
        }

        #endregion


        #region Methods

        public void Initialise(CharacterModel characterModel)
        {
            CharacterCamera.transform.rotation = Quaternion.Euler(0, characterModel.CharacterCommonSettings.InstantiateDirection, 0);
            CharacterFreelookCamera = _cameraData._cameraSettings.CreateCharacterFreelookCamera(characterModel.CameraTargetTransform);
            CharacterTargetCamera = _cameraData._cameraSettings.CreateCharacterTargetCamera(characterModel.CameraTargetTransform);
            CharacterDialogCamera = _cameraData._cameraSettings.CreateCharacterDialogCamera(characterModel.CameraTargetTransform);

            PreviousActiveCamera = CharacterFreelookCamera;
            SetActiveCamera(CharacterFreelookCamera);
        }

        public GameObject CreateCameraTarget(Transform characterTransform)
        {
            GameObject target = new GameObject { name = _cameraData._cameraSettings.CameraTargetName };

            target.transform.SetParent(characterTransform);
            target.transform.localPosition = new Vector3(0, _cameraData._cameraSettings.CameraTargetHeight, 0);
            target.transform.localRotation = Quaternion.Euler(0, 0, 0);

            return target;
        }

        public void SetActiveCamera(CinemachineVirtualCameraBase newCamera)
        {
            if(newCamera != CurrentActiveCamera)
            {
                PreviousActiveCamera = CurrentActiveCamera;
                CurrentActiveCamera = newCamera;

                SetAllCamerasEqual();
                newCamera.Priority++;

                float blendTime = 0f;

                if (CurrentActiveCamera == CharacterFreelookCamera)
                {
                    blendTime = _cameraData._cameraSettings.CharacterFreelookCameraBlendTime;
                }
                else if (CurrentActiveCamera == CharacterTargetCamera)
                {
                    blendTime = _cameraData._cameraSettings.CHaracterTargetCameraBlendTime;
                }
                else if (CurrentActiveCamera == CharacterDialogCamera)
                {
                    blendTime = _cameraData._cameraSettings.CHaracterDialogCameraBlendTIme;
                }

                SetBlendTime(blendTime);
            }
        }

        private void SetAllCamerasEqual()
        {
            CharacterFreelookCamera.Priority = 0;
            CharacterTargetCamera.Priority = 0;
            CharacterDialogCamera.Priority = 0;
        }

        public void SetBlendTime(float time)
        {
            CameraCinemachineBrain.m_DefaultBlend.m_Time = time;
        }

#if (UNITY_EDITOR)
        private void SaveCameraSettings(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                _cameraData._cameraSettings.SaveCameraSettings(CharacterFreelookCamera, CharacterTargetCamera, CharacterDialogCamera);
                EditorApplication.playModeStateChanged -= SaveCameraSettings;
            }
        }
#endif

        #endregion
    }
}


