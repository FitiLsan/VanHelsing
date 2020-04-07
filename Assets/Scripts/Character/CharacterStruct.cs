using UnityEngine;
using Cinemachine;
using System;


[Serializable]
public struct CharacterStruct
{
    #region Fields

    [Header("Scene information fields")]

    [Tooltip("Character prefab.")]
    public GameObject _prefab;

    [Tooltip("Character camera.")]
    public Camera _characterCamera;

    [Tooltip("Character cinemachine camera.")]
    public CinemachineFreeLook _characterCinemachineCamera;

    [Tooltip("Character animator controller.")]
    public RuntimeAnimatorController _characterAnimator;

    [Tooltip("Vector 3 prefab position on the scene")]
    [SerializeField] private Vector3 _instantiatePosition;

    [Tooltip("Prefab direction on the scene")]
    [SerializeField] private float _instantiateDirection;

    [Header("Prefab rigitbody information fields")]

    [Tooltip("Character rigitbody mass between 0 and 10.")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float _rigitbodyMass;

    [Tooltip("Character rigitbody drag between 0 and 10.")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float _rigitbodyDrag;

    [Tooltip("Character rigitbody angular drag between 0 and 10.")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float _rigitbodyAngularDrag;

    [Header("Prefab sphere collider information fields")]
    [Tooltip("Sphere collider center position")]
    [SerializeField] private Vector3 _capsuleColliderCenter;

    [Tooltip("Sphere collider radius between 0 and 2.")]
    [Range(0.0f, 2.0f)]
    [SerializeField] private float _capsuleColliderRadius;

    [Tooltip("Sphere collider height between 0 and 5.")]
    [Range(0.0f, 5.0f)]
    [SerializeField] private float _capsuleColliderHeight;

    [Header("Character movement settings")]

    [Tooltip("Walk speed value between 0 and 20.")]
    [Range(0.0f, 20.0f)]
    [SerializeField] private float _walkSpeed;

    [Tooltip("Run speed value between 0 and 20.")]
    [Range(0.0f, 20.0f)]
    [SerializeField] private float _runSpeed;

    [Tooltip("Reduction of the speed while in battle between 0 and 1.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _inBattleSpeedReduction;

    [Tooltip("Jump force between 0 and 10.")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float _jumpForce;

    [Tooltip("Character falling force between 0 and 10.")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float _fallingForce;

    [Tooltip("Character long jump falling force between 0 and 10.")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float _longJumpFallingForce;

    [Tooltip("Ground check height under character between 0 and 2.")]
    [Range(0.0f, 2.0f)]
    [SerializeField] private float _groundCheckHeight;

    [Tooltip("Character speed measuring time frame between 0 and 1.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _speedMeasureFrame;

    [Tooltip("Character acceleration lag between 0 and 1.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _accelerationLag;

    [Tooltip("Character deceleraton lag between 0 and 1.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _decelerationLag;

    [Tooltip("Character direction change lag between 0 and 1.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _directionChangeLag;

    [Header("Character stats")]

    [Tooltip("Maximal health between 0 and 100.")]
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _maximalHealth;

    [Tooltip("Current health between 0 and 100.")]
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _currentlHealth;

    [Tooltip("Maximal stamina between 0 and 100.")]
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _maximalStamina;

    [Tooltip("Current stamina between 0 and 100.")]
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _currentStamina;

    [Header("Camera Settings")]

    [Tooltip("Height of camera from the character between 0 and 3.")]
    [Range(0.0f, 3.0f)]
    [SerializeField] private float _cameraHeight;

    [Tooltip("Angle of camera when going forward and to any side between 0 and 360.")]
    [Range(0.0f, 360f)]
    [SerializeField] private float _cameraLowSideAngle;

    [Tooltip("Angle of camera when going only to any side between 0 and 360.")]
    [Range(0.0f, 360f)]
    [SerializeField] private float _cameraHalfSideAngle;

    [Tooltip("Angle of camera when going back and to any side between 0 and 360.")]
    [Range(0.0f, 360f)]
    [SerializeField] private float _cameraBackSideAngle;

    [Tooltip("Angle of camera when going only back between 0 and 360.")]
    [Range(0.0f, 360f)]
    [SerializeField] private float _cameraBackAngle;

    #endregion


    #region Properties

    public Vector3 InstantiatePosition => _instantiatePosition;
    public Vector3 CapsuleColliderCenter => _capsuleColliderCenter;

    public float InstantiateDirection => _instantiateDirection;
    public float RigitbodyMass => _rigitbodyMass;
    public float RigitbodyDrag => _rigitbodyDrag;
    public float RigitbodyAngularDrag => _rigitbodyAngularDrag;
    public float CapsuleColliderRadius => _capsuleColliderRadius;
    public float CapsuleColliderHeight => _capsuleColliderHeight;

    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float InBatlleSpeedReduction => _inBattleSpeedReduction;
    public float JumpForce => _jumpForce;
    public float FallingForce => _fallingForce;
    public float LongJumpFallingForce => _longJumpFallingForce;
    public float GroundCheckHeight => _groundCheckHeight;
    public float SpeedMeasureFrame => _speedMeasureFrame;
    public float AccelerationLag => _accelerationLag;
    public float DecelerationLag => _decelerationLag;
    public float DirectionChangeLag => _directionChangeLag;

    public float MaximalHealth => _maximalHealth;
    public float CurrentHealth => _currentlHealth;
    public float MaximalStamina => _maximalStamina;
    public float CurrentStamina => _currentStamina;

    public float CameraHeight => _cameraHeight;
    public float CameraLowSideAngle => _cameraLowSideAngle;
    public float CameraHalfSideAngle => _cameraHalfSideAngle;
    public float CameraBackSideAngle => _cameraBackSideAngle;
    public float CameraBackAngle => _cameraBackAngle;

    #endregion
}
