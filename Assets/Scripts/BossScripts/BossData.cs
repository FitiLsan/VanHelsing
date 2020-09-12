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
            if(agent.CalculatePath(pointTo, new NavMeshPath()))
            {
                agent.SetDestination(pointTo);
                agent.speed = speed;
            }
        }

        #endregion
    }
}
