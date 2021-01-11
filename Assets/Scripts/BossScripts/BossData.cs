using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "BossData")]
    public sealed class BossData : EnemyData
    {
        #region Fields

        [SerializeField] private BossSettings _bossSettings;
        [SerializeField] private GameObject _movementPrefab;
        private Vector3 _movementVector;

        #endregion


        #region Properties

        public BossSettings BossSettings => _bossSettings;
        public GameObject MovementPrefab => _movementPrefab;

        #endregion


        #region Metods

        public override void Act(EnemyModel bossModel)
        {
            (bossModel as BossModel).BossStateMachine.Execute();
        }

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
