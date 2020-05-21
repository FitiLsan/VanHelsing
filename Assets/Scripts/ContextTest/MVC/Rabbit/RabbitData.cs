using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{

    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/Rabbit", order = 2)]
    public sealed class RabbitData : ScriptableObject
    {
        #region Constants

        private const float DANGEROUS_OBJECTS_MAX_COUNT = 4.0f;
        private const float FRONT_RAYCAST_DISTANCE = 2.0f;
        private const float ROTATION_SPEED = 5.0f;
        private const float HOP_FORCE_MULTIPLIER = 100.0f;
        private const float MAX_ANGLE_DEVIATION = 40.0f;
        private const float TURN_FORWARD = 0.0f;
        private const float TURN_BACK = 180.0f;
        private const float TURN_RIGHT = 270.0f;
        private const float TURN_LEFT = 90.0f;

        public const float STOP_RETURNING_DISTANCE_FACTOR = 3.0f;

        #endregion


        #region Fields

        private PhysicsService _physicsService = Services.SharedInstance.PhysicsService;

        public RabbitStruct RabbitStruct;

        #endregion


        #region Metods

        public bool CheckForEnemiesInFieldOfView(Transform transform, List<Transform> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                var distToTarget = targets[i].position - transform.position;
                if (distToTarget.sqrMagnitude > RabbitStruct.ViewRadius)
                {
                    targets.RemoveAt(i);
                }
            }

            //var triggers = _physicsService.GetObjectsInRadius(transform.position, RabbitStruct.ViewRadius, LayerManager.DefaultLayer);
            var triggers = Physics.OverlapSphere(transform.position, RabbitStruct.ViewRadius, LayerManager.DefaultLayer);//change layer!!
            var result = false;
            foreach (Collider target in triggers)
            {
                if (!target.CompareTag(TagManager.RABBIT))
                {
                    var dirToTarget = target.bounds.center - transform.position;
                    if (Vector3.Angle(transform.forward, dirToTarget.normalized) < RabbitStruct.ViewAngle)
                    {
                        if (!Physics.Raycast(transform.position, dirToTarget.normalized, dirToTarget.magnitude, LayerManager.EnvironmentLayer))
                        {
                            if (targets.Count < DANGEROUS_OBJECTS_MAX_COUNT)
                            {
                                if (!targets.Contains(target.transform))
                                    targets.Add(target.transform);
                                result = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public void RotateTo(Transform transform, Vector3 from, Vector3 to, out bool rotationFinished)
        {
            float singleStep = ROTATION_SPEED * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(from, to, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            float dot = Vector3.Dot(from.normalized, to.normalized);
            rotationFinished = dot >= 1.0f;
        }

        public void Hop(Rigidbody rigidbody, Vector3 direction, float acceleration)
        {
            rigidbody.AddForce((direction * RabbitStruct.MoveSpeed * acceleration + Vector3.up * RabbitStruct.JumpHeight) * HOP_FORCE_MULTIPLIER);
        }

        public Vector3 NextCoord(Transform transform, Vector3 startPosition, List<Transform> targets, System.Func<Transform, Vector3, List<Transform>, Vector3> nextCoordFunc)
        {
            var nextCoord = nextCoordFunc(transform, startPosition, targets);
            if (Physics.Raycast(transform.position, nextCoord, FRONT_RAYCAST_DISTANCE, LayerManager.EnvironmentLayer))
            {
                var chance = Random.Range(0.0f, 1.0f);
                float angle;
                if (chance < 0.5f)
                {
                    angle = MAX_ANGLE_DEVIATION * Mathf.Deg2Rad;
                }
                else
                {
                    angle = -MAX_ANGLE_DEVIATION * Mathf.Deg2Rad;
                }
                var coord = new Vector2(nextCoord.x, nextCoord.z);
                nextCoord.x = RotateByAngle(coord, angle).x;
                nextCoord.z = RotateByAngle(coord, angle).y;
            }
            CheckForEnemiesInFieldOfView(transform, targets);
            return nextCoord;
        }

        public Vector3 RandomNextCoord(Transform transform, Vector3 startPosition, List<Transform> targets)
        {
            var direction = Random.Range(0.0f, 100.0f);
            var angle = TURN_FORWARD;
            if (direction >= 80.0f)
            {
                angle = TURN_LEFT;
            }
            else if (direction >= 60.0f)
            {
                angle = TURN_RIGHT;
            }
            else if (direction >= 45.0f)
            {
                angle = TURN_BACK;
            }
            angle += Random.Range(-MAX_ANGLE_DEVIATION, MAX_ANGLE_DEVIATION);
            angle *= Mathf.Deg2Rad;

            var forward = new Vector2(transform.forward.x, transform.forward.z);
            return new Vector3(RotateByAngle(forward, angle).x, transform.forward.y, RotateByAngle(forward, angle).y);
        }

        public Vector3 ReturningNextCoord(Transform transform, Vector3 startPosition, List<Transform> targets)
        {
            var next = (startPosition - transform.position).normalized;
            //var distance = Mathf.Min((transform.position - startPosition).magnitude, RabbitStruct.RunningRadius);
            //var angle = AngleDeviation(distance);

            //var distance = Mathf.Min((transform.position - startPosition).magnitude, RabbitStruct.RunningRadius);
            //var angle = AngleDeviationSqr(distance);

            var distance = Mathf.Min((transform.position - startPosition).sqrMagnitude, RabbitStruct.RunningRadius * RabbitStruct.RunningRadius);
            var angle = AngleDeviationSqrPlain(distance);
            angle = Random.Range(-angle, angle);
            angle *= Mathf.Deg2Rad;
            var forward = new Vector2(next.x, next.z);
            return new Vector3(RotateByAngle(forward, angle).x, transform.forward.y, RotateByAngle(forward, angle).y);
        }

        public Vector3 FleeingNextCoord(Transform transform, Vector3 startPosition, List<Transform> targets)
        {
            var sum = Vector3.zero;
            foreach (var obj in targets)
            {
                var dir = transform.position - obj.position;
                dir = dir.normalized * RabbitStruct.ViewRadius * RabbitStruct.ViewRadius / dir.sqrMagnitude;
                sum += dir;
            }
            var result = new Vector2(sum.x, sum.z);
            if (result == Vector2.zero)
            {
                result.x = targets[0].position.x;
                result.y = targets[0].position.z;
            }
            result.Normalize();
            return new Vector3(result.x, transform.forward.y, result.y);
        }

        public float AngleDeviation(float distance) // linear, use with distance magnitude
        {
            var r = RabbitStruct.RunningRadius;
            var f = STOP_RETURNING_DISTANCE_FACTOR;
            return (MAX_ANGLE_DEVIATION * f) / (f - 1.0f) * (-(distance / r) + 1);
        }

        public float AngleDeviationSqr(float distance) // parabolic, use with distance magnitude (variant 1, point on runningRadius/returnFactor)
        {
            var r = RabbitStruct.RunningRadius;
            var f = STOP_RETURNING_DISTANCE_FACTOR;
            var a = (MAX_ANGLE_DEVIATION * f * f) / (r * r * (2.0f * f - f * f - 1.0f));
            var b = -(2.0f * a * r) / f;
            var c = (a * r * r * (2.0f - f)) / f;
            return a * distance * distance + b * distance + c;
        }

        public float AngleDeviationSqrPlain(float distance) // parabolic, use with distance sqrMagnitude (variant 2, point on startPos)
        {
            var r = RabbitStruct.RunningRadius;
            var f = STOP_RETURNING_DISTANCE_FACTOR;
            var a = -((MAX_ANGLE_DEVIATION * f * f) / (r * r * (f * f - 1.0f)));
            var c = -a * r * r;
            return a * distance + c;
        }

        private Vector2 RotateByAngle(Vector2 vector, float angle)
        {
            return new Vector2(vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle), vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle));
        }

        #endregion
    }
}
