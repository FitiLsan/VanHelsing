using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeastHunter
{

    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/Rabbit", order = 2)]
    public sealed class RabbitData : EnemyData
    {
        #region PrivateData

        public enum BehaviourState
        {
            None = 0,
            Idling = 1,
            Roaming = 2,
            Fleeing = 3,
            Returning = 4
        }

        #endregion


        #region Constants

        private const float TIME_UNTIL_CAN_CHANGE_STATE = 10.0f;
        private const float IDLE_ANIMATION_DURATION = 3.0f;
        private const float STOP_FLEEING_TIME = 2.0f;
        private const float DANGEROUS_OBJECTS_MAX_COUNT = 4.0f;
        private const float STOP_RETURNING_DISTANCE_FACTOR = 3.0f;

        private const float HOP_FREQUENCY = 1.0f;
        private const float MAX_HOP_FREQUENCY = 2.0f;
        private const float FLEE_ACCELERATION_FACTOR = 1.3f;
        private const float ROTATION_SPEED = 5.0f;
        private const float HOP_FORCE_MULTIPLIER = 100.0f;
        private const float MAX_ANGLE_DEVIATION = 40.0f;

        private const float FRONT_RAYCAST_DISTANCE = 2.0f;
        private const float DOWN_RAYCAST_DISTANCE = 1.0f;

        private const float TURN_FORWARD = 0.0f;
        private const float TURN_BACK = 180.0f;
        private const float TURN_RIGHT = 270.0f;
        private const float TURN_LEFT = 90.0f;

        #endregion


        #region Fields

        private PhysicsService _physicsService;

        public RabbitStats RabbitStats;

        #endregion


        #region ClassLifeCycles

        public void OnEnable()
        {
            _physicsService = Services.SharedInstance.PhysicsService;
        }

        #endregion


        #region Metods

        public void Act(RabbitModel rabbit)
        {
            if (_physicsService == null)
            {
                _physicsService = Services.SharedInstance.PhysicsService;
            }
            if ((rabbit.RabbitState != BehaviourState.Fleeing) && rabbit.DangerousObjects.Count > 0)
            {
                rabbit.RabbitState = BehaviourState.Fleeing;
            }
            switch (rabbit.RabbitState)
            {
                case BehaviourState.Idling:
                    {
                        Idle();
                        CheckForEnemiesInFieldOfView(rabbit.RabbitTransform, rabbit.DangerousObjects);
                        rabbit.TimeElapsedAfterStateChange += Time.deltaTime;
                        if (rabbit.TimeElapsedAfterStateChange > IDLE_ANIMATION_DURATION && Random.Range(0.0f, 1.0f) > 0.5f)
                        {
                            rabbit.RabbitState = BehaviourState.Roaming; // On idle animation end
                            rabbit.TimeElapsedAfterStateChange = 0.0f;
                        }

                        break;
                    }
                case BehaviourState.Roaming:
                    {
                        Roam(rabbit);
                        rabbit.TimeElapsedAfterStateChange += Time.deltaTime;
                        var distanceFromStart = new Vector2((rabbit.RabbitTransform.position - rabbit.RabbitStartPosition).x, (rabbit.RabbitTransform.position - rabbit.RabbitStartPosition).z);
                        if (distanceFromStart.sqrMagnitude > RabbitStats.RunningRadius * RabbitStats.RunningRadius)
                        {
                            rabbit.RabbitState = BehaviourState.Returning;
                        }
                        else if (RabbitStats.CanIdle && rabbit.TimeElapsedAfterStateChange > TIME_UNTIL_CAN_CHANGE_STATE && Random.Range(0.0f, 1.0f) > 0.95f)
                        {
                            rabbit.TimeElapsedAfterStateChange = 0.0f;
                            rabbit.RabbitState = BehaviourState.Idling;
                        }
                        break;
                    }
                case BehaviourState.Returning:
                    {
                        Return(rabbit);
                        var moveDistance = RabbitStats.RunningRadius / RabbitData.STOP_RETURNING_DISTANCE_FACTOR;
                        if ((rabbit.RabbitTransform.position - rabbit.RabbitStartPosition).sqrMagnitude < moveDistance * moveDistance)
                        {
                            rabbit.RabbitState = BehaviourState.Roaming;
                        }
                        break;
                    }
                case BehaviourState.Fleeing:
                    {
                        rabbit.TimeElapsedAfterStartFleeing += Time.deltaTime;
                        if (rabbit.DangerousObjects.Count >= 0)
                        {
                            Flee(rabbit);
                        }
                        if (rabbit.TimeElapsedAfterStartFleeing > STOP_FLEEING_TIME)
                        {
                            if (rabbit.DangerousObjects.Count > 0)
                            {
                                rabbit.TimeElapsedAfterStartFleeing = 0;
                            }
                            else
                            {
                                rabbit.RabbitState = BehaviourState.Roaming;
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
            //Debug.Log(rabbit.RabbitState);
        }

        private void Idle()
        {
            //Debug.Log("Idle animation");
        }

        private void Roam(RabbitModel rabbit)
        {
            Move(RandomNextCoord, rabbit);
        }

        private void Return(RabbitModel rabbit)
        {
            Move(ReturningNextCoord, rabbit);
        }

        private void Move(Func<Transform, Vector3, List<Transform>, Vector3> nextCoordFunc, RabbitModel rabbit)
        {
            rabbit.TimeLeft -= Time.deltaTime;
            rabbit.TimeElapsed += Time.deltaTime;
            if (rabbit.TimeLeft < 0.0f && _physicsService.CheckGround(rabbit.RabbitTransform.position, DOWN_RAYCAST_DISTANCE, out Vector3 hitPoint))
            {
                RotateTo(rabbit.RabbitTransform, rabbit.RabbitTransform.forward, rabbit.NextCoord, out bool canHop);
                if (rabbit.TimeElapsed >= MAX_HOP_FREQUENCY)
                {
                    canHop = true;
                }
                if (canHop)
                {
                    Hop(rabbit.RabbitRigidbody, rabbit.NextCoord, 1.0f);
                    rabbit.TimeLeft = HOP_FREQUENCY;
                    rabbit.NextCoord = NextCoord(rabbit.RabbitTransform, rabbit.RabbitStartPosition, rabbit.DangerousObjects, nextCoordFunc);
                    rabbit.TimeElapsed = 0.0f;
                }
            }
        }

        private void Flee(RabbitModel rabbit)
        {
            RotateTo(rabbit.RabbitTransform, rabbit.RabbitTransform.forward, rabbit.NextCoord, out bool canHop);
            rabbit.TimeLeft -= Time.deltaTime;
            if (rabbit.TimeLeft < 0.0f && _physicsService.CheckGround(rabbit.RabbitTransform.position, DOWN_RAYCAST_DISTANCE, out Vector3 hitPoint))
            {
                if (rabbit.DangerousObjects.Count > 0)
                {
                    rabbit.NextCoord = NextCoord(rabbit.RabbitTransform, rabbit.RabbitStartPosition, rabbit.DangerousObjects, FleeingNextCoord);
                }
                Hop(rabbit.RabbitRigidbody, rabbit.NextCoord, FLEE_ACCELERATION_FACTOR);
                rabbit.TimeLeft = HOP_FREQUENCY;
            }
        }

        private bool CheckForEnemiesInFieldOfView(Transform transform, List<Transform> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                var distToTarget = targets[i].position - transform.position;
                if (distToTarget.sqrMagnitude > RabbitStats.ViewRadius)
                {
                    targets.RemoveAt(i);
                }
            }

            //var triggers = _physicsService.GetObjectsInRadius(transform.position, RabbitStruct.ViewRadius, LayerManager.DefaultLayer);
            var triggers = Physics.OverlapSphere(transform.position, RabbitStats.ViewRadius, LayerManager.DefaultLayer);//change layer!!
            var result = false;
            foreach (Collider target in triggers)
            {
                if (!target.CompareTag(TagManager.RABBIT))
                {
                    var dirToTarget = target.bounds.center - transform.position;
                    if (Vector3.Angle(transform.forward, dirToTarget.normalized) < RabbitStats.ViewAngle)
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

        private void RotateTo(Transform transform, Vector3 from, Vector3 to, out bool rotationFinished)
        {
            float singleStep = ROTATION_SPEED * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(from, to, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            float dot = Vector3.Dot(from.normalized, to.normalized);
            rotationFinished = Mathf.Approximately(dot, 1.0f);
        }

        private void Hop(Rigidbody rigidbody, Vector3 direction, float acceleration)
        {
            Debug.Log(direction + " " + RabbitStats.MoveSpeed + " " + acceleration + " " + RabbitStats.JumpHeight + " " + HOP_FORCE_MULTIPLIER);
            rigidbody.AddForce((direction * RabbitStats.MoveSpeed * acceleration + Vector3.up * RabbitStats.JumpHeight) * HOP_FORCE_MULTIPLIER);
        }

        private Vector3 NextCoord(Transform transform, Vector3 startPosition, List<Transform> targets, Func<Transform, Vector3, List<Transform>, Vector3> nextCoordFunc)
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

        private Vector3 ReturningNextCoord(Transform transform, Vector3 startPosition, List<Transform> targets)
        {
            var next = (startPosition - transform.position).normalized;
            //var distance = Mathf.Min((transform.position - startPosition).magnitude, RabbitStruct.RunningRadius);
            //var angle = AngleDeviation(distance);

            //var distance = Mathf.Min((transform.position - startPosition).magnitude, RabbitStruct.RunningRadius);
            //var angle = AngleDeviationSqr(distance);

            var distance = Mathf.Min((transform.position - startPosition).sqrMagnitude, RabbitStats.RunningRadius * RabbitStats.RunningRadius);
            var angle = AngleDeviationSqrPlain(distance);
            angle = Random.Range(-angle, angle);
            angle *= Mathf.Deg2Rad;
            var forward = new Vector2(next.x, next.z);
            return new Vector3(RotateByAngle(forward, angle).x, transform.forward.y, RotateByAngle(forward, angle).y);
        }

        private Vector3 FleeingNextCoord(Transform transform, Vector3 startPosition, List<Transform> targets)
        {
            var sum = Vector3.zero;
            foreach (var obj in targets)
            {
                var dir = transform.position - obj.position;
                dir = dir.normalized * RabbitStats.ViewRadius * RabbitStats.ViewRadius / dir.sqrMagnitude;
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

        private float AngleDeviation(float distance) // linear, use with distance magnitude
        {
            var r = RabbitStats.RunningRadius;
            var f = STOP_RETURNING_DISTANCE_FACTOR;
            return (MAX_ANGLE_DEVIATION * f) / (f - 1.0f) * (-(distance / r) + 1);
        }

        private float AngleDeviationSqr(float distance) // parabolic, use with distance magnitude (variant 1, point on runningRadius/returnFactor)
        {
            var r = RabbitStats.RunningRadius;
            var f = STOP_RETURNING_DISTANCE_FACTOR;
            var a = (MAX_ANGLE_DEVIATION * f * f) / (r * r * (2.0f * f - f * f - 1.0f));
            var b = -(2.0f * a * r) / f;
            var c = (a * r * r * (2.0f - f)) / f;
            return a * distance * distance + b * distance + c;
        }

        private float AngleDeviationSqrPlain(float distance) // parabolic, use with distance sqrMagnitude (variant 2, point on startPos)
        {
            var r = RabbitStats.RunningRadius;
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


        #region NpcData

        public override void TakeDamage(EnemyModel instance, Damage damage)
        {
            base.TakeDamage(instance, damage);
            //Debug.Log("Rabbit got " + damage.PhysicalDamage + " damage");

            if (instance.IsDead)
            {
                //Debug.Log("You killed a bunny! You monster!");
                var rabbit = instance as RabbitModel;
                rabbit.Rabbit.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        #endregion
    }
}
