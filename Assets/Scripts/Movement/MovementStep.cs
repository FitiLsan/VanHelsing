using System;
using UnityEngine;


namespace BeastHunter
{
    [ExecuteInEditMode]
    [Serializable]
    public sealed class MovementStep : MonoBehaviour
    {
        #region Constants

        public const string DEFAULT_ANIMATION_STATE = "MovingState";

        #endregion
        
        
        #region Fields
        
        private Vector3 _lastPosition;
        [SerializeField] private MovementPath _path;
        [SerializeField] private Vector3 _handleA;
        [SerializeField] private Vector3 _handleB;
        [SerializeField] private MovementHandleType _handleStyle;
        [SerializeField] [Min(0)] private float _waitingTime;
        [SerializeField] private string _animationState = DEFAULT_ANIMATION_STATE;
        [SerializeField] private bool _isGrounded = true;

        #endregion

        
        #region Properties

        public MovementPath Path
        {
            get => _path;
            set
            {
                if (_path)
                    _path.RemoveStep(this);

                _path = value;
                _path.AddStep(this);
            }
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public Quaternion LocalRotation
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
        }

        public Vector3 HandleA
        {
            get => _handleA;
            set
            {
                if (_handleA == value) return;

                _handleA = value;

                UpdateHandle(value);

                _path.SetDirty();
            }
        }

        public Vector3 GlobalHandleA
        {
            get => transform.TransformPoint(HandleA);
            set => HandleA = transform.InverseTransformPoint(value);
        }

        public Vector3 HandleB
        {
            get => _handleB;
            set
            {
                if (_handleB == value) return;

                _handleB = value;

                UpdateHandle(value);
                
                _path.SetDirty();
            }
        }

        public Vector3 GlobalHandleB
        {
            get => transform.TransformPoint(HandleB);
            set => HandleB = transform.InverseTransformPoint(value);
        }

        public MovementHandleType HandleStyle
        {
            get => _handleStyle;
            set => _handleStyle = value;
        }

        public float WaitingTime
        {
            get => _waitingTime;
            set => _waitingTime = value < 0 ? 0 : value;
        }

        public bool IsGrounded
        {
            get => _isGrounded;
            set
            {
                if (value)
                    transform.position = PhysicsService.GetGroundedPositionStatic(transform.position);

                _isGrounded = value;
            }
        }

        public string AnimationState
        {
            get => _animationState;
            set => _animationState = value;
        }

        #endregion


        #region Methods

        private void UpdateHandle(Vector3 value)
        {
            if (_handleStyle == MovementHandleType.None)
                _handleStyle = MovementHandleType.Broken;
            else if (_handleStyle == MovementHandleType.Connected)
                _handleB = -value;
        }

        #endregion
        

        #region UnityMethods

        private void Update()
        {
            if ((!_path.Dirty || _isGrounded) && transform.position != _lastPosition)
            {
                _path.SetDirty();
                _lastPosition = _isGrounded
                    ? transform.position = PhysicsService.GetGroundedPositionStatic(transform.position)
                    : transform.position;
            }
        }

        private void OnDestroy()
        {
            _path.RemoveStep(this);
        }

        #endregion
    }
}