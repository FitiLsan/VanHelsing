using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Rescues
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "Data/NPC/LocationData")]
    public sealed class LocationData : ScriptableObject
    {
        #region Fields

        [SerializeField] private SpawnPointStruct[] SpawnPoints;

        #endregion

        #region Methods

        public void SetSpawnPoints(SpawnPointBehavior[] spawnPointBehaviors)
        {
            SpawnPoints = new SpawnPointStruct[spawnPointBehaviors.Length];

            for(int i = 0; i < spawnPointBehaviors.Length; i++)
            {
                SpawnPoints[i] = spawnPointBehaviors[i]._spawnPointStruct;
            }
        }

        public SpawnPointStruct[] GetSpawnPoint()
        {
            return SpawnPoints;
        }

        #endregion
    }
}