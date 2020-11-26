using UnityEngine;
using UniRx;


namespace BeastHunter
{
    public sealed class PlayerBehavior : InteractableObjectBehavior
    {
        #region Fields

        private Animator _animatorController;
        private bool _doAim;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _animatorController = gameObject.GetComponent<Animator>();
            _doAim = false;
            Services.SharedInstance.CameraService.CurrentActiveCamera.Subscribe(EnableAiming);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (_doAim)
            {
                _animatorController.SetLookAtPosition(Services.SharedInstance.CameraService.CameraDynamicTarget.position);
                _animatorController.SetLookAtWeight(1f, 1f, 1f, 1f, 1f);
            }
        }

        private void OnDestroy()
        {
            Services.SharedInstance.CameraService.CurrentActiveCamera.Dispose();
        }

        private void EnableAiming(Cinemachine.CinemachineVirtualCameraBase currentCamera)
        {
            _doAim = currentCamera == Services.SharedInstance.CameraService.CharacterAimingCamera;
        }

        #endregion
    }
}

