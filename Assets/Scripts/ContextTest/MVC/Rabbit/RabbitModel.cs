using System;
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

        private float _currentHealth;
        private Vector3 _nextCoord;
        private GameObject _dangerousObject;
        private BehaviourState _rabbitState;
        
        private PhysicsService _physicsService;

        public RabbitData RabbitData;
        public RabbitStruct RabbitStruct;

        #endregion


        #region Properties

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
                RabbitTransform = prefab.transform;
                RabbitRigidbody = prefab.GetComponent<Rigidbody>();
                RabbitStartPosition = prefab.transform.position;
                _currentHealth = RabbitStruct.MaxHealth;
                _nextCoord = rabbitData.RandomNextCoord(prefab.transform, RabbitStartPosition);
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
            //RabbitData.CheckForEnemiesInFieldOfView(RabbitTransform, out GameObject go); //For debug only
            if ((_rabbitState != BehaviourState.Fleeing) && (RabbitData.CheckForEnemiesInFieldOfView(RabbitTransform, out _dangerousObject)))
            {
                _rabbitState = BehaviourState.Fleeing;
            }
            switch (_rabbitState)
            {
                case BehaviourState.Idling:
                    {
                        Idle();
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
                        if (_dangerousObject != null)
                            Flee();
                        if (_timeElapsedAfterStartFleeing > STOP_FLEEING_TIME)
                        {
                            if (_dangerousObject != null &&
                                ((RabbitTransform.position - _dangerousObject.transform.position).sqrMagnitude < RabbitStruct.ViewRadius * RabbitStruct.ViewRadius))
                            {
                                _timeElapsedAfterStartFleeing = 0;
                            }
                            else if (!RabbitData.CheckForEnemiesInRadius(RabbitTransform, out _dangerousObject))
                            {
                                _rabbitState = BehaviourState.Roaming;
                                _dangerousObject = null;
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
            Debug.Log(_rabbitState);
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

        private void Move(Func<Transform, Vector3, Vector3> nextCoordFunc)
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
                    _nextCoord = nextCoordFunc(RabbitTransform, RabbitStartPosition);
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
                var next = (RabbitTransform.position - _dangerousObject.transform.position).normalized;
                _nextCoord = new Vector3(next.x, 0.0f, next.z);
                RabbitData.Hop(RabbitRigidbody, _nextCoord, FLEE_ACCELERATION_FACTOR);
                _timeLeft = HOP_FREQUENCY;
            }
        }

        #endregion
    }
}
