using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeastHunter
{

    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/Butterfly", order = 2)]
    public sealed class ButterflyData : ScriptableObject
    {


        #region Constants

        private const float TIME_UNTIL_CAN_CHANGE_STATE = 10.0f;
        private const float IDLE_ANIMATION_DURATION = 3.0f;
        private const float STOP_FLEEING_TIME = 2.0f;
        private const float DANGEROUS_OBJECTS_MAX_COUNT = 4.0f;
        private const float STOP_RETURNING_DISTANCE_FACTOR = 3.0f;

        private const float HOP_FREQUENCY = 0.5f;
        private const float MAX_HOP_FREQUENCY = 0.01f;
        private const float FLEE_ACCELERATION_FACTOR = 1.3f;
        private const float ROTATION_SPEED = 5.0f;
        private const float HOP_FORCE_MULTIPLIER = 40.0f;
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

        public ButterflyStruct ButterflyStruct;

        #endregion


        #region ClassLifeCycles

        public void OnEnable()
        {
            _physicsService = Services.SharedInstance.PhysicsService;
        }

        #endregion


        #region Metods
        public void Act(ButterflyModel butterfly)
        {
            if (_physicsService == null)
            {
                _physicsService = Services.SharedInstance.PhysicsService;
            }
            Flee(butterfly);
        }

        private void Flee(ButterflyModel butterfly)
        {
            butterfly.TimeLeft -= Time.deltaTime;
            butterfly.TimeElapsed += Time.deltaTime;
            if (butterfly.TimeLeft < 0.0f)
            {
                bool canHop = false;
                if (butterfly.TimeElapsed >= MAX_HOP_FREQUENCY)
                {
                    canHop = true;
                }
                if (canHop)
                {
                    butterfly.ButterflyRigidbody.AddForce(new Vector3(0, 150, 0));
                    Hop(butterfly.ButterflyRigidbody, butterfly.NextCoord);
                    butterfly.TimeLeft = HOP_FREQUENCY;
                    butterfly.NextCoord = new Vector3(Random.Range(-100, 100), Random.Range(10f, 80f), Random.Range(-100, 100));
                    butterfly.TimeElapsed = 0.0f;
                }
            }
            if (butterfly.ButterflyTransform.position.y <= -10 || butterfly.ButterflyTransform.position.y > 420)
            {
                butterfly.ButterflyRigidbody.velocity = new Vector3(0, 0, 0);
                butterfly.ButterflyTransform.position = new Vector3(492, 1, 481);
            }
        }

        

        private void Hop(Rigidbody rigidbody, Vector3 direction)
        {
            rigidbody.AddForce(direction);
        }
        
        #endregion
    }
}