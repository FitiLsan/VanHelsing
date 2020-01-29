using UnityEngine;
using UnityEngine.AI;

namespace Models.NPCScripts.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class NPCMove : MonoBehaviour
    {
        private NavMeshAgent agent;
        private float speed;

        private void Awake()
        {
            speed = 5f;
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;
        }

        public void Move(Vector3 direction)
        {
            agent.speed = speed;
            agent.SetDestination(direction);
        }

        public void Move(Vector3 direction, float speed)
        {
            agent.speed = speed;
            agent.SetDestination(direction);
        }
    }
}