using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "BossData", menuName = "Enemy/BossData")]
    public sealed class BossData : EnemyData
    {
        #region PrivateData

        public BossSettings _bossSettings;
        public BossModel BossModel;
        public BossSkillStruct BossSkills;

        #endregion


        //#region Fields

        //private Vector3 _movementVector;
        //private Quaternion _targetRotation;

        //#endregion


        #region Metods

        public override void Act(EnemyModel enemyModel)
        {
            (enemyModel as BossModel).BossStateMachine.Execute();
        }

        //public void MoveForward(Transform prefabTransform, float moveSpeed)
        //{
        //    _movementVector = Vector3.forward * moveSpeed * Time.deltaTime;
        //    prefabTransform.Translate(_movementVector, Space.Self);
        //}

        //public void Move(Transform prefabTransform, float moveSpeed, Vector3 direction)
        //{
        //    _movementVector = direction * moveSpeed * Time.deltaTime;
        //    prefabTransform.Translate(_movementVector, Space.Self);
        //}

        //public void NavMeshMoveTo(NavMeshAgent agent, Vector3 pointTo, float speed)
        //{
        //    if (agent.CalculatePath(pointTo, new NavMeshPath()))
        //    {
        //        agent.SetDestination(pointTo);
        //        agent.speed = speed;
        //    }
        //}

        //public Quaternion RotateTo(Transform prefabTransform, Transform targetTransform, float angleSpeed, bool immediately = false)
        //{
        //    var deltaTime = Time.deltaTime;
        //    if (immediately == true)
        //    {
        //        deltaTime = Mathf.Infinity;
        //    }
        //    var targetRotation = GetTargetRotation(prefabTransform.position, targetTransform.position).normalized;
            
        //    return Quaternion.RotateTowards(prefabTransform.rotation,  targetRotation, angleSpeed * deltaTime);
        //}

        //public bool CheckIsNearTarget(Vector3 prefabPosition, Vector3 targetPosition, float distanceRange)
        //{
        //    var isNear = Mathf.Sqrt((prefabPosition - targetPosition).sqrMagnitude) <= distanceRange;
        //    return isNear;
        //}

        //public bool CheckIsNearTarget(Vector3 prefabPosition, Vector3 targetPosition, float distanceRange, out float distance)
        //{
        //    distance = Mathf.Sqrt((prefabPosition - targetPosition).sqrMagnitude);
        //    var isNear = distance <= distanceRange;
        //    return isNear;
        //}

        //public bool CheckIsNearTarget(Vector3 prefabPosition, Vector3 targetPosition, float distanceRangeMin, float distanceRangeMax)
        //{
        //    var distance = Mathf.Sqrt((prefabPosition - targetPosition).sqrMagnitude);
        //    var isNear = distance >= distanceRangeMin & distance <= distanceRangeMax;

        //    return isNear;
        //}

        //public bool CheckIsLookAtTarget(Quaternion prefabRotation, Quaternion targetRotation, float angleRange)
        //{
        //    var isLook = Quaternion.Angle(prefabRotation, targetRotation) <= angleRange;
        //    return isLook;
        //}

        //public Quaternion GetTargetRotation(Vector3 prefabPosition, Vector3 targetPosition)
        //{  
        //    var _targetDirection = (targetPosition - prefabPosition).normalized;
        //    _targetRotation = Quaternion.LookRotation(_targetDirection);
        //    return _targetRotation;
        //}

        //public float GetTargetDistance(Vector3 prefabPosition, Vector3 targetPosition)
        //{
        //    var distance = Mathf.Sqrt((prefabPosition - targetPosition).sqrMagnitude);
        //    return distance;
        //}

        //public int AngleDirection(Vector3 forward, Vector3 targetDirection, Vector3 up)
        //{
        //    Vector3 perpendicular = Vector3.Cross(forward, targetDirection);
        //    float direction = Vector3.Dot(perpendicular, up);

        //    if (direction > 0f)
        //    {
        //        return 1;
        //    }
        //    else if (direction < 0f)
        //    {
        //        return -1;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //public void SetNavMeshAgent(NavMeshAgent bossNavAgent, Vector3 targetPosition, float speed)
        //{
        //    SetNavMeshAgentDestination(bossNavAgent, targetPosition);
        //    SetNavMeshAgentSpeed(bossNavAgent, speed);
        //}

        //public void SetNavMeshAgentDestination(NavMeshAgent bossNavAgent, Vector3 targetPosition)
        //{
        //    bossNavAgent.SetDestination(targetPosition);
        //}
        //public void SetNavMeshAgentSpeed(NavMeshAgent bossNavAgent, float speed)
        //{
        //    var realSpeed = Mathf.Clamp(speed - BossModel.CurrentStats.BaseStats.SpeedModifier, 0, float.PositiveInfinity);
        //    bossNavAgent.speed = realSpeed;
        //}

        #endregion
    }
}
