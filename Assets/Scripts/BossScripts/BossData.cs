using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "BossData")]
    public sealed class BossData : ScriptableObject
    {
        #region PrivateData

        public BossSettings _bossSettings;
        public EnemyStats _bossStats;

        #endregion


        #region Fields

        private Vector3 _movementVector;
        private Quaternion _targetRotation;

        #endregion


        #region Metods

        public void MoveForward(Transform prefabTransform, float moveSpeed)
        {
            _movementVector = Vector3.forward * moveSpeed * Time.deltaTime;
            prefabTransform.Translate(_movementVector, Space.Self);
        }

        public void Move(Transform prefabTransform, float moveSpeed, Vector3 direction)
        {
            _movementVector = direction * moveSpeed * Time.deltaTime;
            prefabTransform.Translate(_movementVector, Space.Self);
        }

        public void MoveTo(NavMeshAgent agent, Vector3 pointTo, float speed)
        {
            if (agent.CalculatePath(pointTo, new NavMeshPath()))
            {
                agent.SetDestination(pointTo);
                agent.speed = speed;
            }
        }

        public Quaternion RotateTo(Quaternion prefabRotation, Vector3 prefabPosition, Vector3 targetPosition, float angleSpeed)
        {
            return Quaternion.RotateTowards(prefabRotation, GetTargetRotation(prefabPosition, targetPosition), angleSpeed * Time.deltaTime);
        }

        public bool CheckIsNearTarget(Vector3 prefabPosition, Quaternion prefabRotation, Vector3 targetPosition, float distanceRange, float angleRange)
        {
            GetTargetRotation(prefabPosition, targetPosition);
            var isNear = Mathf.Sqrt((prefabPosition - targetPosition).sqrMagnitude) <= distanceRange &
                Quaternion.Angle(prefabRotation, _targetRotation) <= angleRange;
            var isNearQ = Quaternion.Angle(prefabRotation, _targetRotation);
            return isNear;
        }

        public Quaternion GetTargetRotation(Vector3 prefabPosition, Vector3 targetPosition)
        {
            var _targetDirection = (targetPosition - prefabPosition).normalized;
            _targetRotation = Quaternion.LookRotation(_targetDirection);
            return _targetRotation;
        }

        #endregion
    }
}
