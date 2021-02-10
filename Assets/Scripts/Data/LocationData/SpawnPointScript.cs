using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class SpawnPointScript : MonoBehaviour
    {
        #region Constants

        private const int MAX_ENTITIES_TO_SPAWN = 20;

        #endregion


        #region Fields

        [SerializeField] public float spawnRadius;
        [SerializeField] [Range(1, MAX_ENTITIES_TO_SPAWN)] public int numberToSpawn = 1;
        [SerializeField] public List<SpawnEntityData> spawnEntities;

        #endregion


        #region Methods

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

        #endregion
    }
}
