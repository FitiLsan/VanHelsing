using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [ExecuteInEditMode]
    [Serializable]
    public sealed class MovementPath : MonoBehaviour
    {
        #region PrivateData

        public class Point
        {
            public Vector3 Position;
            public float WaitingTime;
        }

        #endregion
        
        
        #region Fields

        [SerializeField] private MovementPoint[] _points = new MovementPoint[0];
        [SerializeField, Min(1)] private int _resolution = 10;
        [SerializeField] private Color _drawColor = Color.white;
        [SerializeField] private bool _close;
        private float _length;

        #endregion


        #region Properties

        public MovementPoint this[int index] => _points[index];

        public bool Dirty { get; private set; }

        public bool Close
        {
            get => _close;
            set
            {
                if (_close == value) return;
                _close = value;
                Dirty = true;
            }
        }

        public int PointCount => _points.Length;

        public float Length
        {
            get
            {
                if (Dirty)
                {
                    _length = 0;

                    for (var i = 0; i < _points.Length - 1; i++)
                        _length += ApproximateLength(_points[i], _points[i + 1], _resolution);

                    if (Close)
                        _length += ApproximateLength(_points[_points.Length - 1], _points[0], _resolution);

                    Dirty = false;
                }

                return _length;
            }
        }

        #endregion


        #region UnityMethods

        private void OnDrawGizmos()
        {
            Gizmos.color = _drawColor;

            if (_points.Length > 1)
            {
                for (var i = 0; i < _points.Length - 1; i++)
                    DrawPath(_points[i], _points[i + 1], _resolution);

                if (Close)
                    DrawPath(_points[_points.Length - 1], _points[0], _resolution);
            }
        }

        private void Awake()
        {
            Dirty = true;
        }

        #endregion


        #region Methods

        public void AddPoint(MovementPoint point)
        {
            var tempArray = new List<MovementPoint>(_points) {point};

            _points = tempArray.ToArray();
            Dirty = true;
        }

        public MovementPoint AddPointAt(Vector3 position)
        {
            var pointObject = new GameObject("Point " + PointCount);

            pointObject.transform.parent = transform;
            pointObject.transform.position = position;

            var newPoint = pointObject.AddComponent<MovementPoint>();
            newPoint.Path = this;

            return newPoint;
        }

        public void RemovePoint(MovementPoint point)
        {
            var tempArray = new List<MovementPoint>(_points);
            tempArray.Remove(point);
            _points = tempArray.ToArray();
            Dirty = false;
        }

        public MovementPoint[] GetAnchorPoints()
        {
            return (MovementPoint[]) _points.Clone();
        }

        public Vector3 GetPointAt(float t)
        {
            if (t <= 0) return _points[0].Position;
            if (t >= 1) return _points[_points.Length - 1].Position;

            float totalPercent = 0;
            float curvePercent = 0;

            MovementPoint pointA = null;
            MovementPoint pointB = null;

            for (var i = 0; i < _points.Length - 1; i++)
            {
                curvePercent = ApproximateLength(_points[i], _points[i + 1]) / Length;
                
                if (totalPercent + curvePercent > t)
                {
                    pointA = _points[i];
                    pointB = _points[i + 1];
                    
                    break;
                }

                totalPercent += curvePercent;
            }

            if (Close && pointA == null)
            {
                pointA = _points[_points.Length - 1];
                pointB = _points[0];
            }

            t -= totalPercent;

            return GetPoint(pointA, pointB, t / curvePercent);
        }

        public int GetPointIndex(MovementPoint point)
        {
            var result = -1;

            for (var i = 0; i < _points.Length; i++)
                if (_points[i] == point)
                {
                    result = i;
                    
                    break;
                }

            return result;
        }
        
        public List<Point> GetPoints()
        {
            var result = new List<Point>();
            
            if (_points.Length > 0)
            {
                for (var i = 0; i < _points.Length - 1; i++)
                {
                    result.Add(new Point()
                    {
                        Position = _points[i].Position,
                        WaitingTime = _points[i].WaitingTime
                    });
                    
                    for (var j = 1; j < _resolution; j++)
                    {
                        var currentPosition = GetPoint(_points[i], _points[i + 1], j / (float) _resolution);
                        
                        result.Add(new Point()
                        {
                            Position = currentPosition,
                            WaitingTime = 0
                        });
                    }
                }
                
                var lastIndex = _points.Length - 1;
                    
                result.Add(new Point()
                {
                    Position = _points[lastIndex].Position,
                    WaitingTime = _points[lastIndex].WaitingTime
                });

                if (Close)
                {
                    for (var j = 1; j < _resolution; j++)
                    {
                        var currentPosition = GetPoint(_points[lastIndex], _points[0], j / (float) _resolution);
                        
                        result.Add(new Point()
                        {
                            Position = currentPosition,
                            WaitingTime = 0
                        });
                    }
                }
            }
            
            for (var i = 0; i < result.Count - 1; i++)
            {
                Debug.DrawLine(result[i].Position, result[i + 1].Position);
            }

            return result;
        }

        public void SetDirty()
        {
            Dirty = true;
        }

        public static void DrawPath(MovementPoint pointA, MovementPoint pointB, int resolution)
        {
            var limit = resolution + 1;
            var lastPoint = pointA.Position;

            for (var i = 1; i < limit; i++)
            {
                var currentPoint = GetPoint(pointA, pointB, i / (float) resolution);

                Gizmos.DrawLine(lastPoint, currentPoint);
                lastPoint = currentPoint;
            }
        }

        public static Vector3 GetPoint(MovementPoint pointA, MovementPoint pointB, float t)
        {
            if (pointA.HandleB != Vector3.zero)
            {
                if (pointB.HandleA != Vector3.zero)
                    return GetCubicCurvePoint(pointA.Position, pointA.GlobalHandleB, pointB.GlobalHandleA, pointB.Position, t);

                return GetQuadraticCurvePoint(pointA.Position, pointA.GlobalHandleB, pointB.Position, t);
            }

            if (pointB.HandleA != Vector3.zero)
                return GetQuadraticCurvePoint(pointA.Position, pointB.GlobalHandleA, pointB.Position, t);

            return GetLinearPoint(pointA.Position, pointB.Position, t);
        }

        public static Vector3 GetCubicCurvePoint(Vector3 pointA, Vector3 pointB, Vector3 p3, Vector3 p4, float t)
        {
            t = Mathf.Clamp01(t);

            var part1 = Mathf.Pow(1 - t, 3) * pointA;
            var part2 = 3 * Mathf.Pow(1 - t, 2) * t * pointB;
            var part3 = 3 * (1 - t) * Mathf.Pow(t, 2) * p3;
            var part4 = Mathf.Pow(t, 3) * p4;

            return part1 + part2 + part3 + part4;
        }

        public static Vector3 GetQuadraticCurvePoint(Vector3 pointA, Vector3 pointB, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);

            var part1 = Mathf.Pow(1 - t, 2) * pointA;
            var part2 = 2 * (1 - t) * t * pointB;
            var part3 = Mathf.Pow(t, 2) * p3;

            return part1 + part2 + part3;
        }

        public static Vector3 GetLinearPoint(Vector3 pointA, Vector3 pointB, float t)
        {
            return pointA + (pointB - pointA) * t;
        }

        public static Vector3 GetPoint(float t, params Vector3[] points)
        {
            t = Mathf.Clamp01(t);

            var order = points.Length - 1;
            var point = Vector3.zero;

            for (var i = 0; i < points.Length; i++)
            {
                var vectorToAdd = points[points.Length - i - 1] *
                                  (BinomialCoefficient(i, order) * Mathf.Pow(t, order - i) * Mathf.Pow(1 - t, i));
                point += vectorToAdd;
            }

            return point;
        }

        public static float ApproximateLength(MovementPoint pointA, MovementPoint pointB, int resolution = 10)
        {
            var total = 0f;
            var lastPosition = pointA.Position;

            for (var i = 0; i < resolution + 1; i++)
            {
                var currentPosition = GetPoint(pointA, pointB, i / (float) resolution);

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