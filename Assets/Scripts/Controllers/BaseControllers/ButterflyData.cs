using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewButterflyModel", menuName = "CreateData/Butterfly", order = 2)]
    public sealed class ButterflyData : EnemyData
    {
        #region Fields

        public ButterflyStats ButterflyStats;
        public BehaviourStateButterfly BehaviourStateButterfly;

        [SerializeField, Range(0, 10), Header("Время смены состояния")] private float _timeOfTheStateChange;

        private PhysicsService _physicsService;

        #endregion


        #region Constants

        private const float MAX_ANGLE_DEVIATION = 40.0f;

        #endregion



        #region ClassLifeCycles

        public void OnEnable()
        {
            _physicsService = Services.SharedInstance.PhysicsService;
        }

        public void Act(ButterflyModel butterflyModel)
        {
            //TODO - почему тут каждый кадр слетает _physicsService?!?!?!?!?!?
            if (_physicsService == null)
            {
                _physicsService = Services.SharedInstance.PhysicsService;
            }
            switch (butterflyModel.ButterflyState)
            {
                case BehaviourStateButterfly.Idle:
                    Idle(butterflyModel);
                    break;
                case BehaviourStateButterfly.Fly:
                    Fly(butterflyModel);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException("Недопустимое состояние");
            }
        }

        private void Idle(ButterflyModel butterfly)
        {
            Debug.Log($"Бабочка сидит на месте");
            StartTimerToChangeState(butterfly, BehaviourStateButterfly.Fly);
            butterfly.NextCoord = RandomNextCoord(butterfly.ButterflyTransform, butterfly.ButterflyStartPosition);
            var vec = butterfly.ButterflyTransform.position + butterfly.NextCoord * Time.deltaTime * ButterflyStats.MoveSpeed;

            butterfly.ButterflyRigidbody.MovePosition(vec);
        }

        public Vector3 RandomNextCoord(Transform transform, Vector3 startPosition)
        {
            var direction = Random.Range(0.0f, 100.0f);
            var angle = 0.0f;  // TURN_FORWARD;
            if (direction >= 80.0f)
            {
                angle = 90; // TURN_LEFT;
            }
            else if (direction >= 60.0f)
            {
                angle = 270;    // TURN_RIGHT;
            }
            else if (direction >= 45.0f)
            {
                angle = 180;    // TURN_BACK;
            }
            angle += Random.Range(-MAX_ANGLE_DEVIATION, MAX_ANGLE_DEVIATION);
            angle *= Mathf.Deg2Rad;

            var forward = new Vector2(transform.forward.x, transform.forward.z);
            return new Vector3(RotateByAngle(forward, angle).x, transform.forward.y, RotateByAngle(forward, angle).y);
        }

        private Vector2 RotateByAngle(Vector2 vector, float angle)
        {
            return new Vector2(vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle), vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle));
        }

        private void Fly(ButterflyModel butterfly)
        {
            Debug.Log($"Бабочка взлетает");
            StartTimerToChangeState(butterfly, BehaviourStateButterfly.Idle);
            butterfly.ButterflyRigidbody.velocity = butterfly.NextCoord * Time.deltaTime;
        }

        private void StartTimerToChangeState(ButterflyModel butterfly, BehaviourStateButterfly state)
        {
            butterfly.TimeSinceTheState += Time.deltaTime;
            if (butterfly.TimeSinceTheState >= _timeOfTheStateChange)
            {
                Debug.Log($"Меняем состояние");
                butterfly.TimeSinceTheState = 0.0f;
                butterfly.ButterflyState = state;
            }
        }

        #endregion
    }
}