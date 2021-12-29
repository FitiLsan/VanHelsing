using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeastHunter
{

    [CreateAssetMenu(fileName = "NewModelRat", menuName = "CreateData/Rat", order = 2)]
    public sealed class RatData : EnemyData
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
        private const float DANGEROUS_OBJECTS_MAX_COUNT = 1.0f;
        private const float STOP_RETURNING_DISTANCE_FACTOR = 0.2f;

        private const float HOP_FREQUENCY = 1.0f;
        private const float MAX_HOP_FREQUENCY = 2.0f;
        private const float FLEE_ACCELERATION_FACTOR = 1.5f;
        private const float ROTATION_SPEED = 5.0f;
        private const float HOP_FORCE_MULTIPLIER = 100.0f;
        private const float MAX_ANGLE_DEVIATION = 40.0f;

        private const float FRONT_RAYCAST_DISTANCE = 4.0f;
        private const float DOWN_RAYCAST_DISTANCE = 1.0f;

        private const float TURN_FORWARD = 0.0f;
        private const float TURN_BACK = 180.0f;
        private const float TURN_RIGHT = 270.0f;
        private const float TURN_LEFT = 90.0f;

        #endregion


        #region Fields

        private PhysicsService _physicsService;

        public RatStats RatStats;

        #endregion


        #region ClassLifeCycles

        public void OnEnable()
        {
            _physicsService = Services.SharedInstance.PhysicsService;
        }

        #endregion


        #region Metods

        public void Act(RatModel rat)
        {
            if (_physicsService == null)
            {
                _physicsService = Services.SharedInstance.PhysicsService;
            }
            if ((rat.RatState != BehaviourState.Fleeing) && rat.DangerousObjects.Count > 0)
            {
                rat.RatState = BehaviourState.Fleeing;
            }
            switch (rat.RatState)
            {
                case BehaviourState.Idling:
                    {
                        Idle();
                        CheckForEnemiesInFieldOfView(rat.RatTransform, rat.DangerousObjects);
                        rat.TimeElapsedAfterStateChange += Time.deltaTime;
                        if (rat.TimeElapsedAfterStateChange > IDLE_ANIMATION_DURATION && Random.Range(0.0f, 1.0f) > 0.5f)
                        {
                            rat.RatState = BehaviourState.Roaming; // On idle animation end
                            rat.TimeElapsedAfterStateChange = 0.0f;
                        }

                        break;
                    }
                case BehaviourState.Roaming:
                    {
                        Roam(rat);
                        rat.TimeElapsedAfterStateChange += Time.deltaTime;
                        var distanceFromStart = new Vector2((rat.RatTransform.position - rat.RatStartPosition).x, (rat.RatTransform.position - rat.RatStartPosition).z);
                        if (distanceFromStart.sqrMagnitude > RatStats.RunningRadius * RatStats.RunningRadius)
                        {
                            rat.RatState = BehaviourState.Returning;
                        }
                        else if (RatStats.CanIdle && rat.TimeElapsedAfterStateChange > TIME_UNTIL_CAN_CHANGE_STATE && Random.Range(0.0f, 1.0f) > 0.95f)
                        {
                            rat.TimeElapsedAfterStateChange = 0.0f;
                            rat.RatState = BehaviourState.Idling;
                        }
                        break;
                    }
                case BehaviourState.Returning:
                    {
                        Return(rat);
                        var moveDistance = RatStats.RunningRadius / RatData.STOP_RETURNING_DISTANCE_FACTOR;
                        if ((rat.RatTransform.position - rat.RatStartPosition).sqrMagnitude < moveDistance * moveDistance)
                        {
                            rat.RatState = BehaviourState.Roaming;
                        }
                        break;
                    }
                case BehaviourState.Fleeing:
                    {
                        rat.TimeElapsedAfterStartFleeing += Time.deltaTime;
                        if (rat.DangerousObjects.Count >= 0)
                        {
                            Flee(rat);
                        }
                        if (rat.TimeElapsedAfterStartFleeing > STOP_FLEEING_TIME)
                        {
                            if (rat.DangerousObjects.Count > 0)
                            {
                                rat.TimeElapsedAfterStartFleeing = 0;
                            }
                            else
                            {
                                rat.RatState = BehaviourState.Roaming;
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
            Debug.Log("Состояние крысы: " + rat.RatState);
        }

        private void Idle()
        {
            //Debug.Log("Idle animation");
        }

        private void Roam(RatModel rat)
        {
            Move(RandomNextCoord, rat);
        }

        private void Return(RatModel rat)
        {
            Move(ReturningNextCoord, rat);
        }

        private void Move(Func<Transform, Vector3, List<Transform>, Vector3> nextCoordFunc, RatModel rat)
        {
            rat.TimeLeft -= Time.deltaTime;
            rat.TimeElapsed += Time.deltaTime;
            if (rat.TimeLeft < 0.0f && _physicsService.CheckGround(rat.RatTransform.position, DOWN_RAYCAST_DISTANCE, out Vector3 hitPoint))
            {
                RotateTo(rat.RatTransform, rat.RatTransform.forward, rat.NextCoord, out bool canHop);
                if (rat.TimeElapsed >= MAX_HOP_FREQUENCY)
                {
                    canHop = true;
                }
                if (canHop)
                {
                    Hop(rat.RatRigidbody, rat.NextCoord, 1.0f);
                    rat.TimeLeft = HOP_FREQUENCY;
                    rat.NextCoord = NextCoord(rat.RatTransform, rat.RatStartPosition, rat.DangerousObjects, nextCoordFunc);
                    rat.TimeElapsed = 0.0f;
                }
            }
        }

        private void Flee(RatModel rat)
        {
            RotateTo(rat.RatTransform, rat.RatTransform.forward, rat.NextCoord, out bool canHop);
            rat.TimeLeft -= Time.deltaTime;
            if (rat.TimeLeft < 0.0f && _physicsService.CheckGround(rat.RatTransform.position, DOWN_RAYCAST_DISTANCE, out Vector3 hitPoint))
            {
                if (rat.DangerousObjects.Count > 0)
                {
                    rat.NextCoord = NextCoord(rat.RatTransform, rat.RatStartPosition, rat.DangerousObjects, FleeingNextCoord);
                }
                Hop(rat.RatRigidbody, rat.NextCoord, FLEE_ACCELERATION_FACTOR);
                rat.TimeLeft = HOP_FREQUENCY;
            }
        }

        private bool CheckForEnemiesInFieldOfView(Transform transform, List<Transform> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                var distToTarget = targets[i].position - transform.position;
                if (distToTarget.sqrMagnitude > RatStats.ViewRadius)
                {
                    targets.RemoveAt(i);
                }
            }

            var triggers = Physics.OverlapSphere(transform.position, RatStats.ViewRadius,
                LayerMask.GetMask(LayerMask.LayerToName(LayerManager.DefaultLayer), LayerMask.LayerToName(LayerManager.PlayerLayer)));
            var result = false;
            foreach (Collider target in triggers)
            {
                if (!target.CompareTag(TagManager.RAT))
                {
                    var dirToTarget = target.bounds.center - transform.position;
                    if (Vector3.Angle(transform.forward, dirToTarget.normalized) < RatStats.ViewAngle)
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
            //rigidbody.AddForce((direction * RatStats.MoveSpeed * acceleration + Vector3.up * RatStats.JumpHeight) * HOP_FORCE_MULTIPLIER);
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
            var distance = Mathf.Min((transform.position - startPosition).sqrMagnitude, RatStats.RunningRadius * RatStats.RunningRadius);
            var angle = AngleDeviationSqr(distance);
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
                dir = dir.normalized * RatStats.ViewRadius * RatStats.ViewRadius / dir.sqrMagnitude;
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
            var r = RatStats.RunningRadius;
            var f = STOP_RETURNING_DISTANCE_FACTOR;
            return (MAX_ANGLE_DEVIATION * f) / (f - 1.0f) * (-(distance / r) + 1);
        }

        private float AngleDeviationSqr(float distance) // parabolic, use with distance magnitude (variant 1, point on runningRadius/returnFactor)
        {
            var r = RatStats.RunningRadius;
            var f = STOP_RETURNING_DISTANCE_FACTOR;
            var a = (MAX_ANGLE_DEVIATION * f * f) / (r * r * (2.0f * f - f * f - 1.0f));
            var b = -(2.0f * a * r) / f;
            var c = (a * r * r * (2.0f - f)) / f;
            return a * distance * distance + b * distance + c;
        }

        private float AngleDeviationSqrPlain(float distance) // parabolic, use with distance sqrMagnitude (variant 2, point on startPos)
        {
            var r = RatStats.RunningRadius;
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


        #region EnemyData

        public override void TakeDamage(EnemyModel instance, Damage damage)
        {
            base.TakeDamage(instance, damage);

            if (instance.IsDead)
            {
                Debug.Log("You killed this Rat! Great!");
                var rat = instance as RatModel;
                rat.Rat.GetComponent<Renderer>().material.color = Color.green;
            }
        }

        #endregion
    }
}
