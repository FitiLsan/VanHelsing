using Models.ConditionsAndActions.Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace Models.NPCScripts.Enemy
{
    public enum Speed
    {
        Walk,
        Run,
        Throw
    }

    public class EnemyMove
    {
        private readonly NavMeshAgent agent;
        private readonly BaseCharacterModel CharacterModel;
        private readonly Rigidbody rb;
        private float speed;

        public EnemyMove(NavMeshAgent agent, BaseCharacterModel CharacterModel, Rigidbody rb)
        {
            this.agent = agent;
            this.CharacterModel = CharacterModel;
            agent.speed = CharacterModel.Speed;
            this.rb = rb;
        }

        public void Move(Vector3 direction)
        {
            agent.speed = CharacterModel.Speed;
            agent.SetDestination(direction);
        }

        public void Move(Vector3 direction, Speed Speed)
        {
            switch (Speed)
            {
                case Speed.Walk:
                    agent.speed = CharacterModel.Speed;
                    break;

                case Speed.Run:
                    agent.speed = CharacterModel.RunSpeed;
                    break;

                case Speed.Throw:
                    agent.speed = CharacterModel.RunSpeed * 2;
                    break;
            }

            agent.SetDestination(direction);
        }

        public void Rotate(Vector3 direction)
        {
            direction.y = 0f;
            direction = direction.normalized;
            var newRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(newRotation);
        }

        public void Stop()
        {
            agent.isStopped = true;
        }

        public void Continue()
        {
            agent.isStopped = false;
        }
    }
}