using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [ExecuteInEditMode]
    [Serializable]
    public sealed partial class MovementPath : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<MovementStep> _steps = new List<MovementStep>();
        [SerializeField] [Min(1)] private int _resolution = 10;
        [SerializeField] private Color _drawColor = Color.magenta;
        [SerializeField] private bool _loop;
        private float _length;

        #endregion


        #region Properties

        public MovementStep this[int index] => index < _steps.Count ? _steps[index] : null;

        public bool Dirty { get; private set; }

        public bool Loop
        {
            get => _loop;
            set
            {
                if (_loop == value) return;

                _loop = value;
                Dirty = true;
            }
        }

        public int StepCount => _steps.Count;

        #endregion


        #region UnityMethods

        private void OnDrawGizmos()
        {
            Gizmos.color = _drawColor;

            var points = GetPoints();

            if (points.Count > 1)
            {
                for (var i = 0; i < points.Count - 1; i++)
                {
                    var currentStep = points[i].Position;
                    currentStep.y += .1f;

                    var nextStep = points[i + 1].Position;
                    nextStep.y += .1f;

                    Gizmos.DrawLine(currentStep, nextStep);
                }

                if (Loop)
                {
                    var currentStep = points[points.Count - 1].Position;
                    currentStep.y += .1f;

                    var nextStep = points[0].Position;
                    nextStep.y += .1f;

                    Gizmos.DrawLine(currentStep, nextStep);
                }
            }
        }

        private void Awake()
        {
            Dirty = true;
        }

        #endregion


        #region Methods

        public void AddStep(MovementStep step)
        {
            var tempArray = new List<MovementStep>(_steps) {step};

            _steps = tempArray;
            Dirty = true;
        }

        public MovementStep AddStepAt(Vector3 position)
        {
            var stepObject = new GameObject("Step " + StepCount);

            stepObject.transform.parent = transform;
            stepObject.transform.position = position;

            var newStep = stepObject.AddComponent<MovementStep>();
            newStep.Path = this;

            return newStep;
        }

        public void RemoveStep(MovementStep step)
        {
            var tempArray = new List<MovementStep>(_steps);
            tempArray.Remove(step);
            _steps = tempArray;
            Dirty = false;
        }

        public List<MovementStep> GetAnchorSteps()
        {
            return _steps;
        }
        
        public float GetLength()
        {
            if (Dirty)
            {
                _length = 0;

                for (var i = 0; i < _steps.Count - 1; i++)
                    _length += ApproximateLength(_steps[i], _steps[i + 1], _resolution);

                if (Loop)
                    _length += ApproximateLength(_steps[_steps.Count - 1], _steps[0], _resolution);

                Dirty = false;
            }

            return _length;
        }

        public Vector3 GetStepAt(float t)
        {
            if (t <= 0) return _steps[0].Position;
            if (t >= 1) return _steps[_steps.Count - 1].Position;

            var totalPercent = 0f;
            var curvePercent = 0f;

            MovementStep stepA = null;
            MovementStep stepB = null;

            for (var i = 0; i < _steps.Count - 1; i++)
            {
                curvePercent = ApproximateLength(_steps[i], _steps[i + 1]) / GetLength();

                if (totalPercent + curvePercent > t)
                {
                    stepA = _steps[i];
                    stepB = _steps[i + 1];

                    break;
                }

                totalPercent += curvePercent;
            }

            if (Loop && stepA == null)
            {
                stepA = _steps[_steps.Count - 1];
                stepB = _steps[0];
            }

            t -= totalPercent;

            return GetStep(stepA, stepB, t / curvePercent);
        }

        public int GetStepIndex(MovementStep step)
        {
            var result = -1;

            for (var i = 0; i < _steps.Count; i++)
                if (_steps[i] == step)
                {
                    result = i;

                    break;
                }

            return result;
        }

        public List<MovementPoint> GetPoints()
        {
            var result = new List<MovementPoint>();

            if (_steps.Count > 0)
            {
                for (var i = 0; i < _steps.Count - 1; i++)
                {
                    result.Add(new MovementPoint()
                    {
                        Position = _steps[i].Position,
                        IsGrounded = _steps[i].IsGrounded,
                        WaitingTime = _steps[i].WaitingTime,
                        AnimationState = _steps[i].AnimationState
                    });

                    for (var j = 1; j < _resolution; j++)
                    {
                        var currentPosition = GetStep(_steps[i], _steps[i + 1], j / (float) _resolution);

                        result.Add(new MovementPoint()
                        {
                            Position = _steps[i].IsGrounded
                                ? PhysicsService.GetGroundedPositionStatic(currentPosition)
                                : currentPosition,
                            IsGrounded = _steps[i].IsGrounded,
                            WaitingTime = 0,
                            AnimationState = MovementStep.DEFAULT_ANIMATION_STATE
                        });
                    }
                }

                var lastIndex = _steps.Count - 1;

                result.Add(new MovementPoint()
                {
                    Position = _steps[lastIndex].Position,
                    IsGrounded = _steps[lastIndex].IsGrounded,
                    WaitingTime = _steps[lastIndex].WaitingTime,
                    AnimationState = _steps[lastIndex].AnimationState
                });

                if (Loop)
                    for (var j = 1; j < _resolution; j++)
                    {
                        var currentPosition = GetStep(_steps[lastIndex], _steps[0], j / (float) _resolution);

                        result.Add(new MovementPoint()
                        {
                            Position = _steps[lastIndex].IsGrounded
                                ? PhysicsService.GetGroundedPositionStatic(currentPosition)
                                : currentPosition,
                            IsGrounded = _steps[lastIndex].IsGrounded,
                            WaitingTime = 0,
                            AnimationState = MovementStep.DEFAULT_ANIMATION_STATE
                        });
                    }
            }

            return result;
        }

        public void SetDirty()
        {
            Dirty = true;
        }

        public static Vector3 GetStep(MovementStep stepA, MovementStep stepB, float t)
        {
            if (stepA.HandleB != Vector3.zero)
            {
                if (stepB.HandleA != Vector3.zero)
                    return GetCubicCurveStep(stepA.Position, stepA.GlobalHandleB, stepB.GlobalHandleA,
                        stepB.Position, t);

                return GetQuadraticCurveStep(stepA.Position, stepA.GlobalHandleB, stepB.Position, t);
            }

            if (stepB.HandleA != Vector3.zero)
                return GetQuadraticCurveStep(stepA.Position, stepB.GlobalHandleA, stepB.Position, t);

            return GetLinearStep(stepA.Position, stepB.Position, t);
        }

        public static Vector3 GetCubicCurveStep(Vector3 stepA, Vector3 stepB, Vector3 p3, Vector3 p4, float t)
        {
            t = Mathf.Clamp01(t);

            var part1 = Mathf.Pow(1 - t, 3) * stepA;
            var part2 = 3 * Mathf.Pow(1 - t, 2) * t * stepB;
            var part3 = 3 * (1 - t) * Mathf.Pow(t, 2) * p3;
            var part4 = Mathf.Pow(t, 3) * p4;

            return part1 + part2 + part3 + part4;
        }

        public static Vector3 GetQuadraticCurveStep(Vector3 stepA, Vector3 stepB, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);

            var part1 = Mathf.Pow(1 - t, 2) * stepA;
            var part2 = 2 * (1 - t) * t * stepB;
            var part3 = Mathf.Pow(t, 2) * p3;

            return part1 + part2 + part3;
        }

        public static Vector3 GetLinearStep(Vector3 stepA, Vector3 stepB, float t)
        {
            return stepA + (stepB - stepA) * t;
        }

        public static Vector3 GetStep(float t, params Vector3[] steps)
        {
            t = Mathf.Clamp01(t);

            var order = steps.Length - 1;
            var step = Vector3.zero;

            for (var i = 0; i < steps.Length; i++)
            {
                var vectorToAdd = steps[steps.Length - i - 1] *
                                  (BinomialCoefficient(i, order) * Mathf.Pow(t, order - i) * Mathf.Pow(1 - t, i));
                step += vectorToAdd;
            }

            return step;
        }

        public static float ApproximateLength(MovementStep stepA, MovementStep stepB, int resolution = 10)
        {
            var total = 0f;
            var lastPosition = stepA.Position;

            for (var i = 0; i < resolution + 1; i++)
            {
                var currentPosition = GetStep(stepA, stepB, i / (float) resolution);

                total += (currentPosition - lastPosition).magnitude;
                lastPosition = currentPosition;
            }

            return total;
        }

        private static int BinomialCoefficient(int i, int n)
        {
            return Factorial(n) / (Factorial(i) * Factorial(n - i));
        }

        private static int Factorial(int i)
        {
            if (i == 0) return 1;

            var total = 1;

            while (i - 1 >= 0)
            {
                total *= i;
                i--;
            }

            return total;
        }

        #endregion
    }
}