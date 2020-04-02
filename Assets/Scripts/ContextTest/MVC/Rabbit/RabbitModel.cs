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


        #region Fields

        private const float TIME_UNTIL_CAN_CHANGE_STATE = 10.0f;
        private const float IDLE_ANIMATION_DURATION = 3.0f;
        private const float HOP_FREQUENCY = 1.0f;
        private const float MAX_HOP_FREQUENCY = 2.0f;
        private const float FLEE_ACCELERATION_FACTOR = 1.3f;

        private float _timeLeft = 1.0f;
        private float _timeElapsed = 0.0f;
        private float _timeElapsedAfterStateChange = 0.0f;
        private bool _isOnGround;
        private Vector3 _nextCoord;
        private GameObject _dangerousObject;
        private BehaviourState _rabbitState;

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
                _nextCoord = rabbitData.RandomNextCoord(prefab.transform, RabbitStartPosition);
                if (RabbitStruct.CanIdle)
                {
                    _rabbitState = BehaviourState.Idling;
                }
                else
                {
                    _rabbitState = BehaviourState.Roaming; 
                }
            }
            else
            {
                Debug.LogError("Invalid Rabbit prefab: no Rigidbody");
            }
        }

        #endregion


        #region Metods

        public void Initilaize()
        {
            // check for enemies (in field of wiew only)

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
                        if ((RabbitTransform.position - RabbitStartPosition).sqrMagnitude > RabbitStruct.RunningRadius * RabbitStruct.RunningRadius)
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
                        var moveDistance = RabbitStruct.RunningRadius / 3.0f; // stop return factor
                        if ((RabbitTransform.position - RabbitStartPosition).sqrMagnitude < moveDistance * moveDistance)
                        {
                            _rabbitState = BehaviourState.Roaming;
                        }
                        break;
                    }
                case BehaviourState.Fleeing:
                    {
                        Flee();
                        // stop flee logic (field of view radius)
                        // time after lost target
                        break;
                    }
                default:
                    break;
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

        private void Move(Func<Transform, Vector3, Vector3> nextCoordFunc) // Move to RabbitData? Pass private fields as parameters?
        {
            _timeLeft -= Time.deltaTime;
            _timeElapsed += Time.deltaTime;
            _isOnGround = RabbitData.CheckIfOnGround(RabbitTransform);
            if (_timeLeft < 0.0f && _isOnGround)
            {
                bool canHop;
                RabbitData.RotateTo(RabbitTransform, RabbitTransform.forward, _nextCoord, out canHop);
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

        private void Flee() // Merge to Move()? Don`t know how
        {
            //_timeElapsed += Time.deltaTime;
            bool canHop;
            RabbitData.RotateTo(RabbitTransform, RabbitTransform.forward, _nextCoord, out canHop);
            _isOnGround = RabbitData.CheckIfOnGround(RabbitTransform);
            if (_isOnGround)
            {
                //if (_timeElapsed >= 0.5f)
                //{
                //    canHop = true;
                //}
                //if (canHop)
                //{
                    RabbitData.Hop(RabbitRigidbody, _nextCoord, FLEE_ACCELERATION_FACTOR);
                    var next = (RabbitTransform.position - _dangerousObject.transform.position).normalized;
                    _nextCoord = new Vector3(next.x, 0.0f, next.z);
                //_timeElapsed = 0.0f;
                //}
            }
        }

        #endregion
    }
}
