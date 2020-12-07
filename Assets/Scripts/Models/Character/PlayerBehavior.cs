using UnityEngine;
using UniRx;


namespace BeastHunter
{
    public sealed class PlayerBehavior : InteractableObjectBehavior
    {
        #region Fields

        private Animator _animatorController;
        private CharacterAnimationData _animationData;
        private bool _doAim;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _animatorController = gameObject.GetComponent<Animator>();
            _animationData = Data.CharacterData.CharacterAnimationData;
            _doAim = false;
            Services.SharedInstance.CameraService.CurrentActiveCamera.Subscribe(EnableAiming);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (_doAim)
            {
                _animatorController.SetLookAtPosition(Services.SharedInstance.CameraService.CameraDynamicTarget.position);
                _animatorController.SetLookAtWeight(
                    _animationData.AimingLookToWeightIK, _animationData.AimingLookToBodyWeightIK, 
                        _animationData.AimingLookToHeadWeightIK, _animationData.AimingLookToEyesWeightIK, 
                            _animationData.AimingLookToClampWeightIK);
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

        public void EnableHiding(bool isHiding)
        {
            if (isHiding)
            {
                _type = InteractableObjectType.None;
            }
            else
            {
                _type = InteractableObjectType.Player;
            }
        }

        #endregion
    }
}

