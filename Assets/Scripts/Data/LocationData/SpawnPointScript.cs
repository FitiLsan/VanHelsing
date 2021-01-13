using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace BeastHunter
{
    //[ExecuteInEditMode]
    public class SpawnPointScript : MonoBehaviour
    {
        private const int MAX_ENTITIES_TO_SPAWN = 20;

        [SerializeField] public float spawnRadius;
        [SerializeField] [Range(1, MAX_ENTITIES_TO_SPAWN)] public int numberToSpawn = 1;
        [SerializeField] public List<SpawnEntityData> spawnEntities;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
