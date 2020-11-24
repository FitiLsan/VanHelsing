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
            Roaming = 1
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
        private Vector3 _nextPos;

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
            switch (rabbit.RabbitState)
            {
                case BehaviourState.Roaming:
                    {
                        Roam(rabbit);
                        rabbit.TimeElapsedAfterStateChange += Time.deltaTime;
                        break;
                    }
                default:
                    break;
            }
            // Debug.Log(rabbit.RabbitState);
        }

        private void Idle()
        {
            //Debug.Log("Idle animation");
        }

        private void Roam(RabbitModel rabbit)
        {
            Move(RandomNextCoord, rabbit);
        }

        private void Move(Func<Transform, Vector3, List<Transform>, Vector3> nextCoordFunc, RabbitModel rabbit)
        {
            rabbit.TimeLeft -= Time.deltaTime;
            rabbit.TimeElapsed += Time.deltaTime;

            _physicsService.CheckGround(rabbit.RabbitTransform.position, DOWN_RAYCAST_DISTANCE, out Vector3 hitPoint);

            float distance = rabbit.RabbitTransform.position.y - hitPoint.y;
            distance = distance < 1 ? 0 : 1 / distance;
            Debug.Log(distance);

            rabbit.RabbitRigidbody.velocity = new Vector3(Random.Range(-1f, 1f), distance - .1f, Random.Range(-1f, 1f));
        }

        private void RotateTo(Transform transform, Vector3 from, Vector3 to, out bool rotationFinished)
        {
            float singleStep = ROTATION_SPEED * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(from, to, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            float dot = Vector3.Dot(from.normalized, to.normalized);
            rotationFinished = Mathf.Approximately(dot, 1.0f);
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


        #region EnemyData

        public override void TakeDamage(EnemyModel instance, Damage damage)
        {
            base.TakeDamage(instance, damage);
            //Debug.Log("Rabbit got " + damage.PhysicalDamage + " damage");

            if (instance.IsDead)
            {
                // Debug.Log("You killed a bunny! You monster!");
                var rabbit = instance as RabbitModel;
                rabbit.Rabbit.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        #endregion
    }
}
