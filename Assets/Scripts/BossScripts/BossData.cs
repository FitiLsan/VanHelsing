using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "BossData")]
    public sealed class BossData : ScriptableObject
    {
        #region PrivateData

        public BossSettings BossSettings;
        public EnemyStats BaseStats;
        public BossStats BossStats;

        #endregion


        #region Fields

        private Vector3 _movementVector;

        #endregion


        #region Metods

        public void Act(BossIdlePattern idlePattern, BossModel model)
        {
            switch (idlePattern)
            {
                case BossIdlePattern.MoveForward: MoveForward(model.BossTransform, 5f);
                    break;
                case BossIdlePattern.Rotate: Rotate(model.BossTransform, 5f);
                    break;
                default:
                    break;
            }
        }

        //public void Act(BossBehaviorType, BossModel) ----- общий enum со всеми стейтами - нет ограничений и в Idle можно будет запихнуть атаку


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

        private void Rotate(Transform prefabTransform, float rotateSpeed)
        {
            prefabTransform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }

        #endregion
    }
}
