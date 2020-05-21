using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class RabbitModel
    {
        #region PrivateData

        private enum BehaviourState
        {
            None      = 0,
            Idling    = 1,
            Roaming   = 2,
            Fleeing   = 3,
            Returning = 4
        }

        #endregion


        #region Constants

        private const float TIME_UNTIL_CAN_CHANGE_STATE = 10.0f;
        private const float IDLE_ANIMATION_DURATION = 3.0f;
        private const float STOP_FLEEING_TIME = 2.0f;
        private const float HOP_FREQUENCY = 1.0f;
        private const float MAX_HOP_FREQUENCY = 2.0f;
        private const float FLEE_ACCELERATION_FACTOR = 1.3f;

        #endregion


        #region Fields
               
        private float _timeLeft = 1.0f;
        private float _timeElapsed = 0.0f;
        private float _timeElapsedAfterStateChange = 0.0f;
        private float _timeElapsedAfterStartFleeing = 0.0f;

        private Vector3 _nextCoord;
        public List<Transform> _dangerousObjects;
        private BehaviourState _rabbitState;
        
        private PhysicsService _physicsService;

        public float CurrentHealth;
        public bool IsDead;
        public RabbitData RabbitData;
        public RabbitStruct RabbitStruct;

        #endregion


        #region Properties

        public GameObject Rabbit { get; }
        public Transform RabbitTransform { get; }
        public Rigidbody RabbitRigidbody { get; }
        public Vector3 RabbitStartPosition { get; }

        #endregion


        #region ClassLifeCycle

        public RabbitModel(GameObject prefab, RabbitData rabbitData)
        {
            if (prefab.GetComponent<Rigidbody>() != null)
            {
                RabbitData = rabbitData;
                RabbitStruct = rabbitData.RabbitStruct;
                Rabbit = prefab;
                RabbitTransform = prefab.transform;
                RabbitRigidbody = prefab.GetComponent<Rigidbody>();
                RabbitStartPosition = prefab.transform.position;
                CurrentHealth = RabbitStruct.MaxHealth;
                IsDead = false;
                _dangerousObjects = new List<Transform>();
                _nextCoord = rabbitData.RandomNextCoord(RabbitTransform, RabbitStartPosition, _dangerousObjects);
                if (RabbitStruct.CanIdle)
                {
                    _rabbitState = BehaviourState.Idling;
                }
                else
                {
                    _rabbitState = BehaviourState.Roaming; 
                }

                _physicsService = Services.SharedInstance.PhysicsService;
            }
            else
            {
                Debug.LogError("Invalid Rabbit prefab: no Rigidbody");
            }
        }

        #endregion


        #region Metods

        public void Execute()
        {
            if(!RabbitStruct.IsDead)
            {
                if ((_rabbitState != BehaviourState.Fleeing) && _dangerousObjects.Count > 0)
                {
                    _rabbitState = BehaviourState.Fleeing;
                }
                switch (_rabbitState)
                {
                    case BehaviourState.Idling:
                        {
                            Idle();
                            RabbitData.CheckForEnemiesInFieldOfView(RabbitTransform, _dangerousObjects);
                            _timeElapsedAfterStateChange += Time.deltaTime;
                            if (_timeElapsedAfterStateChange > IDLE_ANIMATION_DURATION && UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f)
                            {
                                _rabbitState = BehaviourState.Roaming; // On idle animation end
                                _timeElapsedAfterStateChange = 0.0f;
                            }

                            break;
                        }
                    case BehaviourState.Roaming:
                        {
                            Roam();
                            _timeElapsedAfterStateChange += Time.deltaTime;
                            var distanceFromStart = new Vector2((RabbitTransform.position - RabbitStartPosition).x, (RabbitTransform.position - RabbitStartPosition).z);
                            if (distanceFromStart.sqrMagnitude > RabbitStruct.RunningRadius * RabbitStruct.RunningRadius)
                            {
                                _rabbitState = BehaviourState.Returning;
                            }
                            else if (RabbitStruct.CanIdle && _timeElapsedAfterStateChange > TIME_UNTIL_CAN_CHANGE_STATE && UnityEngine.Random.Range(0.0f, 1.0f) > 0.95f)
                            {
                                _timeElapsedAfterStateChange = 0.0f;
                                _rabbitState = BehaviourState.Idling;
                            }
                            break;
                        }
                    case BehaviourState.Returning:
                        {
                            Return();
                            var moveDistance = RabbitStruct.RunningRadius / RabbitData.STOP_RETURNING_DISTANCE_FACTOR;
                            if ((RabbitTransform.position - RabbitStartPosition).sqrMagnitude < moveDistance * moveDistance)
                            {
                                _rabbitState = BehaviourState.Roaming;
                            }
                            break;
                        }
                    case BehaviourState.Fleeing:
                        {
                            _timeElapsedAfterStartFleeing += Time.deltaTime;
                            if (_dangerousObjects.Count >= 0)
                            {
                                Flee();
                            }
                            if (_timeElapsedAfterStartFleeing > STOP_FLEEING_TIME)
                            {
                                if (_dangerousObjects.Count > 0)
                                {
                                    _timeElapsedAfterStartFleeing = 0;
                                }
                                else
                                {
                                    _rabbitState = BehaviourState.Roaming;
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
                Debug.Log(_rabbitState);
            }
        }

        private void Idle()
        {
            Debug.Log("Idle animation");
        }

        private void Roam() 
        {
            Move(RabbitData.RandomNextCoord);
        }

        private void Return()
        {
            Move(RabbitData.ReturningNextCoord);
        }

        private void Move(Func<Transform, Vector3, List<Transform>, Vector3> nextCoordFunc)
        {
            _timeLeft -= Time.deltaTime;
            _timeElapsed += Time.deltaTime;
            if (_timeLeft < 0.0f && _physicsService.CheckGround(RabbitTransform.position, 1.0f, out Vector3 hitPoint))
            {
                RabbitData.RotateTo(RabbitTransform, RabbitTransform.forward, _nextCoord, out bool canHop);
                if (_timeElapsed >= MAX_HOP_FREQUENCY)
                {
                    canHop = true;
                }
                if (canHop)
                {
                    RabbitData.Hop(RabbitRigidbody, _nextCoord, 1.0f);
                    _timeLeft = HOP_FREQUENCY;
                    _nextCoord = RabbitData.NextCoord(RabbitTransform, RabbitStartPosition, _dangerousObjects, nextCoordFunc);
                    _timeElapsed = 0.0f;
                }
            }
        }

        private void Flee()
        {
            RabbitData.RotateTo(RabbitTransform, RabbitTransform.forward, _nextCoord, out bool canHop);
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0.0f && _physicsService.CheckGround(RabbitTransform.position, 1.0f, out Vector3 hitPoint))
            {
                if (_dangerousObjects.Count > 0)
                {
                    _nextCoord = RabbitData.NextCoord(RabbitTransform, RabbitStartPosition, _dangerousObjects, RabbitData.FleeingNextCoord);
                }
                RabbitData.Hop(RabbitRigidbody, _nextCoord, FLEE_ACCELERATION_FACTOR);
                _timeLeft = HOP_FREQUENCY;
            }
        }

        #endregion
    }
}
