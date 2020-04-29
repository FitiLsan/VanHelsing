using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/Wolf", order = 0)]
    public sealed class WolfData : ScriptableObject
    {
        #region Fields

        public WolfStruct WolfStruct;
        public List<Collider> EnemyInAggroRange;

        private float _rotationSpeed = 50f;

        #endregion

        #region Methods

        public void Move(Transform transform, Vector3 target, float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed);
        }

        public void FindTargetsInAggroRange(Transform transform)
        {

            WolfStruct.TargetsInAggroRange.Clear();
            Collider[] targets = Physics.OverlapSphere(transform.position, WolfStruct.AggroRange, WolfStruct.TargetMask);
            foreach (Collider target in targets)
            {
                if (!target.CompareTag(TagManager.WOLF))
                {
                    EnemyInAggroRange.Add(target);
                }                
            }
            for (int i = 0; i < targets.Length; i++)
            {
                if(!CheckDistance(transform.position, targets[i].gameObject.transform.position, WolfStruct.AggroRange))
                {
                    //EnemyInAggroRange.Clear();
                }
            }
        }

        public bool IsLookingAtTarget(Transform transform, Vector3 target)
        {
            Vector3 dirToTarget = (target - transform.position).normalized;
            float angle = Vector3.Angle(dirToTarget, transform.forward);
            return angle < 1.0f;
        }

        public void RotateTo(Transform transform, Vector3 to)
        {
            Vector3 targetDirection = to - transform.position;
            float singleStep = _rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        public Vector3 GetRandomWayPoint()
        {
            return WolfStruct.PatrolWaypointsList[Random.Range(0, WolfStruct.PatrolWaypointsList.Count)];
        }

        public bool CheckDistance(Vector3 currentPosition, Vector3 currentTargetPosition, float distance)
        {
            float X = (currentPosition.x - currentTargetPosition.x) * (currentPosition.x - currentTargetPosition.x);
            float Y = (currentPosition.y - currentTargetPosition.y) * (currentPosition.y - currentTargetPosition.y);
            float Z = (currentPosition.z - currentTargetPosition.z) * (currentPosition.z - currentTargetPosition.z);

            float squareDistance = X + Y + Z;

            float squareStopDistance = distance * distance;

            if (squareDistance <= squareStopDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }

}

