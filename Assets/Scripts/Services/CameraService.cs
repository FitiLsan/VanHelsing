using UnityEngine;
using UnityEditor;
using Cinemachine;
using System;
using UniRx;


namespace BeastHunter
{
    public sealed class CameraService : Service, IDisposable
    {
        #region Constants

        private float AIM_LINE_DRAW_DISTANCE_STEP = 0.1f;
        private float AIM_LINE_DRAW_FIRST_STEP_DISTANCE = 1f;
        private int AMOUNT_OF_AIM_LINE_STEPS = 15;

        #endregion


        #region FIelds

        private readonly GameContext _context;
        private CameraData _cameraData;
        private GameObject _cameraDynamicTarget;
        private GameObject _cameraStaticTarget;
        private GameObject _aimCanvas;
        private Vector3 _staticTargetCenterPosition;
        private Vector3 _dynamicTargetCenterPosition;
        private Vector3 _shootinWeaponDirection;
        private Transform _weaponShootTransform;
        private GameObject[] _aimingDots;

        private float _weaponHitDistance;
        private float _projectileMass;

        private bool _isAiminDotsVisible;
        private bool _isCurrentWeaponWithProjectile;

        #endregion


        #region Properties

        public Camera CharacterCamera { get; private set; }
        public CinemachineFreeLook CharacterFreelookCamera { get; private set; }
        public CinemachineFreeLook CharacterKnockedDownCamera { get; private set; }
        public CinemachineVirtualCamera CharacterTargetCamera { get; private set; }
        public CinemachineVirtualCamera CharacterAimingCamera { get; private set; }
        public CinemachineBrain CameraCinemachineBrain { get; private set; }
        public ReactiveProperty<CinemachineVirtualCameraBase> CurrentActiveCamera { get; private set; }
        public CinemachineVirtualCameraBase PreviousActiveCamera { get; private set; }
        public Transform CameraDynamicTarget
        {
            get
            {
                return _cameraDynamicTarget.transform;
            }
        }

        #endregion


        #region ClassLifeCycles

        public CameraService(Contexts contexts) : base(contexts)
        {
            _context = contexts as GameContext;
            _cameraData = Data.CameraData;

#if (UNITY_EDITOR)
            EditorApplication.playModeStateChanged += SaveCameraSettings;
#endif
        }

        #endregion


        #region Methods

        public void Initialize(CharacterModel characterModel)
        {
            CharacterCamera = _cameraData._cameraSettings.CreateCharacterCamera();
            CameraCinemachineBrain = CharacterCamera.GetComponent<CinemachineBrain>() ?? null;

            _cameraDynamicTarget = GameObject.Instantiate(new GameObject(), characterModel.CharacterTransform);
            _dynamicTargetCenterPosition = new Vector3(0f, _cameraData._cameraSettings.CameraTargetHeight,
                _cameraData._cameraSettings.CameraTargetForwardMovementDistance);
            _cameraDynamicTarget.transform.localPosition = _dynamicTargetCenterPosition;
            _cameraDynamicTarget.name = _cameraData._cameraSettings.CameraTargetName + "Dynamic";

            _cameraStaticTarget = GameObject.Instantiate(new GameObject(), characterModel.CharacterTransform);
            _staticTargetCenterPosition = new Vector3(0f, _cameraData._cameraSettings.CameraTargetHeight, 0f);
            _cameraStaticTarget.transform.localPosition = _staticTargetCenterPosition;
            _cameraStaticTarget.name = _cameraData._cameraSettings.CameraTargetName + "Static";
            _aimCanvas = GameObject.Instantiate(_cameraData._cameraSettings.AimCanvasPrefab);
            _aimCanvas.SetActive(false);

            _aimingDots = new GameObject[AMOUNT_OF_AIM_LINE_STEPS];

            for (int i = 0; i < AMOUNT_OF_AIM_LINE_STEPS; i++)
            {
                _aimingDots[i] = GameObject.Instantiate(_cameraData._cameraSettings.AimProjectileLinePrefab,
                    _cameraStaticTarget.transform.position, Quaternion.identity);
            }

            StopDrawAimLine();

            CharacterCamera.transform.rotation = Quaternion.Euler(0, characterModel.CharacterCommonSettings.
                InstantiateDirection, 0);
            CharacterFreelookCamera = _cameraData._cameraSettings.CreateCharacterFreelookCamera(_cameraStaticTarget.
                transform, _cameraStaticTarget.transform);
            CharacterKnockedDownCamera = _cameraData._cameraSettings.CreateCharacterKnockedDownCamera(_cameraStaticTarget.
                transform, _cameraStaticTarget.transform);
            CharacterTargetCamera = _cameraData._cameraSettings.CreateCharacterTargetCamera(_cameraStaticTarget.
                transform, _cameraStaticTarget.transform);
            CharacterAimingCamera = _cameraData._cameraSettings.CreateCharacterAimingCamera(_cameraStaticTarget.
                transform, _cameraDynamicTarget.transform);

            CharacterFreelookCamera.m_RecenterToTargetHeading.m_RecenteringTime = 0;
            CharacterFreelookCamera.m_RecenterToTargetHeading.m_RecenterWaitTime = 0;

            CurrentActiveCamera = new ReactiveProperty<CinemachineVirtualCameraBase>();
            PreviousActiveCamera = CharacterFreelookCamera;
            SetActiveCamera(CharacterFreelookCamera);

            characterModel.CurrentCharacterState.Subscribe(UpdateCameraForCharacterState);
            CurrentActiveCamera.Subscribe(EnableDisableAimTarget);
        }

        public void SetActiveCamera(CinemachineVirtualCameraBase newCamera)
        {
            if (_context.CharacterModel != null && newCamera != CurrentActiveCamera.Value)
            {
                if (newCamera != CharacterFreelookCamera)
                {
                    CharacterFreelookCamera.m_RecenterToTargetHeading.m_enabled = true;
                    LockFreeLookCamera();
                }
                else
                {
                    CharacterFreelookCamera.m_RecenterToTargetHeading.m_enabled = false;
                    UnlockFreeLookCamera();
                }

                PreviousActiveCamera = CurrentActiveCamera.Value;
                CurrentActiveCamera.Value = newCamera;
                SetAllCamerasEqual();

                float blendTime = 0f;

                if (CurrentActiveCamera.Value == CharacterFreelookCamera)
                {
                    blendTime = _cameraData._cameraSettings.CharacterFreelookCameraBlendTime;
                }
                else if (CurrentActiveCamera.Value == CharacterKnockedDownCamera)
                {
                    blendTime = _cameraData._cameraSettings.CharacterKnockedDownCameraBlendTime;
                }
                else if (CurrentActiveCamera.Value == CharacterTargetCamera)
                {
                    blendTime = _cameraData._cameraSettings.CharacterTargetCameraBlendTime;
                }
                else if (CurrentActiveCamera.Value == CharacterAimingCamera)
                {
                    blendTime = _cameraData._cameraSettings.CHaracterAimingCameraBlendTIme;
                }

                SetBlendTime(blendTime);
                CurrentActiveCamera.Value.Priority++;
            }
        }

        private void UpdateCameraForCharacterState(CharacterBaseState currentState)
        {
            switch (currentState?.StateName)
            {
                case CharacterStatesEnum.Aiming:
                    SetActiveCamera(CharacterAimingCamera);
                    GetWeaponShootTransform();
                    break;
                case CharacterStatesEnum.Attacking:                 
                    break;
                case CharacterStatesEnum.Shooting:                
                    break;
                case CharacterStatesEnum.Battle:
                    SetActiveCamera(CharacterTargetCamera);
                    break;
                case CharacterStatesEnum.Dead:
                    SetActiveCamera(CharacterFreelookCamera);
                    break;
                case CharacterStatesEnum.Dodging:
                    break;
                case CharacterStatesEnum.Idle:
                    SetActiveCamera(CharacterFreelookCamera);
                    break;
                case CharacterStatesEnum.Jumping:
                    break;
                case CharacterStatesEnum.Movement:
                    SetActiveCamera(CharacterFreelookCamera);
                    break;
                case CharacterStatesEnum.Sliding:
                    break;
                case CharacterStatesEnum.Sneaking:
                    break;
                case CharacterStatesEnum.TrapPlacing:
                    break;
                case CharacterStatesEnum.KnockedDown:
                    SetActiveCamera(CharacterFreelookCamera);
                    break;
                case CharacterStatesEnum.GettingUp:
                    break;
                default:
                    break;
            }
        }

        private void SetAllCamerasEqual()
        {
            CharacterFreelookCamera.Priority = 0;
            CharacterKnockedDownCamera.Priority = 0;
            CharacterTargetCamera.Priority = 0;
            CharacterAimingCamera.Priority = 0;
        }

        public void SetBlendTime(float time)
        {
            CameraCinemachineBrain.m_DefaultBlend.m_Time = time;
        }

        public void LockFreeLookCamera()
        {
            CharacterFreelookCamera.m_XAxis.m_MaxSpeed = 0f;
            CharacterFreelookCamera.m_YAxis.m_MaxSpeed = 0f;
        }

        public void UnlockFreeLookCamera()
        {
            CharacterFreelookCamera.m_XAxis.m_MaxSpeed = _cameraData._cameraSettings.CharacterFreelookCamera.
                m_XAxis.m_MaxSpeed;
            CharacterFreelookCamera.m_YAxis.m_MaxSpeed = _cameraData._cameraSettings.CharacterFreelookCamera.
                m_YAxis.m_MaxSpeed;
        }

        public void CenterCameraTarget()
        {
            _cameraDynamicTarget.transform.localPosition = _dynamicTargetCenterPosition;
        }

        public void SetCameraTargetPosition(Vector3 position, bool isPositionAdded)
        {
            if (isPositionAdded)
            {
                _cameraDynamicTarget.transform.localPosition = new Vector3(
                    Mathf.Clamp(_cameraDynamicTarget.transform.localPosition.x - position.x * _cameraData._cameraSettings.
                        CameraTargetSpeedX * Time.deltaTime, _cameraData._cameraSettings.CameraTargetDistanceMoveX.x, 
                            _cameraData._cameraSettings.CameraTargetDistanceMoveX.y),
                    Mathf.Clamp(_cameraDynamicTarget.transform.localPosition.y - position.y * _cameraData._cameraSettings.
                        CameraTargetSpeedY * Time.deltaTime,_cameraData._cameraSettings.CameraTargetDistanceMoveY.x, 
                            _cameraData._cameraSettings.CameraTargetDistanceMoveY.y),
                    _cameraDynamicTarget.transform.localPosition.z);              
            }
            else
            {
                _cameraDynamicTarget.transform.localPosition = new Vector3(
                    Mathf.Clamp(position.x * _cameraData._cameraSettings.
                        CameraTargetSpeedX * Time.deltaTime, _cameraData._cameraSettings.CameraTargetDistanceMoveX.x,
                            _cameraData._cameraSettings.CameraTargetDistanceMoveX.y),
                    Mathf.Clamp(position.y * _cameraData._cameraSettings.
                        CameraTargetSpeedY * Time.deltaTime, _cameraData._cameraSettings.CameraTargetDistanceMoveY.x,
                            _cameraData._cameraSettings.CameraTargetDistanceMoveY.y),
                    _cameraDynamicTarget.transform.localPosition.z);
            }
        }

        private void EnableDisableAimTarget(CinemachineVirtualCameraBase currentCamera)
        {
            if(currentCamera == CharacterAimingCamera && !_isCurrentWeaponWithProjectile)
            {
                _aimCanvas.SetActive(true);
            }
            else if(PreviousActiveCamera == CharacterAimingCamera && _aimCanvas.activeSelf)
            {
                _aimCanvas.SetActive(false);
            }
        }

        private void GetWeaponShootTransform()
        {
            if (_context.CharacterModel.CurrentWeaponData.Value is OneHandedShootingWeapon weapon)
            {
                _weaponShootTransform = weapon.ParticleSystem.transform;
                _projectileMass = weapon.ProjectileData.ProjectilePrefab.GetComponent<Rigidbody>().mass;
                _weaponHitDistance = weapon.HitDistance;
            }
            else
            {
                _weaponShootTransform = null;
                _projectileMass = 0f;
                _weaponHitDistance = 0f;
            }
        }

        public void DrawAimLine()
        {
            _shootinWeaponDirection = _cameraDynamicTarget.transform.position - _weaponShootTransform.position;

            for (int i = 0; i < AMOUNT_OF_AIM_LINE_STEPS; i++)
            {
                _aimingDots[i].transform.position = GetAimLinePointPosition((AIM_LINE_DRAW_FIRST_STEP_DISTANCE + i) * 
                    AIM_LINE_DRAW_DISTANCE_STEP);
            }
        }

        public void StartDrawAimLine()
        {
            foreach (var aimDot in _aimingDots)
            {
                aimDot.SetActive(true);
            }

            _isAiminDotsVisible = true;
        }

        public void StopDrawAimLine()
        {
            foreach (var aimDot in _aimingDots)
            {
                aimDot.transform.position = Vector3.zero;
                aimDot.SetActive(false);
            }

            _isAiminDotsVisible = false;
        }

        public void UpdateWeaponProjectileExistence(bool isCurrentWeaponWithProjectile)
        {
            _isCurrentWeaponWithProjectile = isCurrentWeaponWithProjectile;
        }

        private Vector3 GetAimLinePointPosition(float stepDistance)
        {
            return _weaponShootTransform.position + (_weaponShootTransform.forward.normalized * 
                (_weaponHitDistance / _projectileMass) * stepDistance) + 0.5f * Physics.gravity * 
                    (stepDistance * stepDistance);
        }

        public void Dispose()
        {
            _context.CharacterModel.CurrentCharacterState.Dispose();
            CurrentActiveCamera.Dispose();
        }

#if (UNITY_EDITOR)
        private void SaveCameraSettings(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                _cameraData._cameraSettings.SaveCameraSettings(CharacterFreelookCamera, 
                    CharacterTargetCamera, CharacterAimingCamera);
                EditorApplication.playModeStateChanged -= SaveCameraSettings;
            }
        }

#endif

        #endregion
    }
}


