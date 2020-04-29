using UnityEngine;

namespace BeastHunter
{
    public sealed class WolfModel
    {
        #region  PrivateData

        private enum BehaviourState
        {
            Patrol = 0,
            Agrresive = 1,
            Rest = 2,
        }

        #endregion

        #region Constants

        private const float STOP_DISTANCE = 1.0f;

        #endregion

        #region Fields

        private Vector3 _currentWayPoint;
        private PhysicsService _physicsService;
        private BehaviourState _behavioutState;
        private Vector3 _startAggroRangePosition;

        public WolfData WolfData;
        public WolfStruct WolfStruct;

        #endregion


        #region Properties

        public SphereCollider WolfCollider { get; }
        public Transform WolfTransfrom { get; }

        #endregion


        #region ClassLifeCycle

        public WolfModel(GameObject prefab, WolfData wolfData)
        {
            WolfData = wolfData;
            WolfStruct = wolfData.WolfStruct;
            WolfTransfrom = prefab.transform;
            prefab.name = "Wolf";
            WolfCollider = prefab.transform.GetComponent<SphereCollider>();
            _behavioutState = BehaviourState.Patrol;
            _currentWayPoint = new Vector3();
            _currentWayPoint = wolfData.GetRandomWayPoint();
        }

        #endregion


        #region Methods

        public void Initialize()
        {
            //if(WolfStruct.PatrolWaypointsList.Count==0)
            //{
            //    _behavioutState = BehaviourState.Patrol;
            //}
            //else
            //{
            //    _startAggroRangePosition = WolfTransfrom.position;
            //    _behavioutState = BehaviourState.Agrresive;
            //}

            switch (_behavioutState)
            {
                case BehaviourState.Patrol:
                    {
                        Patroling();
                    break;
                    }
                case BehaviourState.Agrresive:
                    {
                        if (WolfData.CheckDistance(_startAggroRangePosition, _currentWayPoint, WolfStruct.AggroRange))
                        {
                            GetCloserToTarget();
                        }
                        else
                            WolfData.Move(WolfTransfrom, _startAggroRangePosition, WolfStruct.RunSpeed);
                    break;
                    }

                case BehaviourState.Rest:
                    {
                        WolfData.FindTargetsInAggroRange(WolfTransfrom);
                    break;
                    }
                default:
                    break;
            }
        }


        public void GetCloserToTarget()
        {
            _currentWayPoint = WolfStruct.TargetsInAggroRange[0].transform.position;            

            if(WolfData.CheckDistance(WolfTransfrom.position,_currentWayPoint, STOP_DISTANCE))
            {
                Attack();
            }
            else
            {
                WolfTransfrom.LookAt(_currentWayPoint);
                WolfData.Move(WolfTransfrom, _currentWayPoint, WolfStruct.WalkSpeed);
            }
        }

        public void Attack()
        {

        }

        public void Patroling()
        {
            //WolfData.FindTargetsInAggroRange(WolfTransfrom);

            if (WolfData.CheckDistance(WolfTransfrom.position, _currentWayPoint, STOP_DISTANCE))
            {
                _currentWayPoint = WolfData.GetRandomWayPoint();
            }
            else
            {
                if (WolfData.IsLookingAtTarget(WolfTransfrom, _currentWayPoint))
                {
                    WolfData.Move(WolfTransfrom, _currentWayPoint, WolfStruct.WalkSpeed);
                }
                else
                {
                    //WolfTransfrom.LookAt(_currentWayPoint);
                    WolfData.RotateTo(WolfTransfrom, _currentWayPoint);
                }
            }
        }

        #endregion
    }
}

