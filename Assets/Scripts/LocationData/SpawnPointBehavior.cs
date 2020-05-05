using System.Collections.Generic;
using UnityEngine;

namespace Rescues
{
    [ExecuteInEditMode]
    public class SpawnPointBehavior : MonoBehaviour
    {
        #region Fields

        [SerializeField] private LocationData _locationData;
        [SerializeField] private bool _isScanScene;
        public SpawnPointStruct _spawnPointStruct;

        #endregion

        private void Update()
        {
            if(_isScanScene)
            {
                var spawnPointBehaviors = FindObjectsOfType<SpawnPointBehavior>();
                List<SpawnPointBehavior> listSpawnPointBehaviors = new List<SpawnPointBehavior>();

                for (int i = 0; i < spawnPointBehaviors.Length; i++)
                {
                    if(spawnPointBehaviors[i]._locationData == _locationData)
                    {
                        listSpawnPointBehaviors.Add(spawnPointBehaviors[i]);
                    }
                }
                _locationData.SetSpawnPoints(listSpawnPointBehaviors.ToArray());
                _isScanScene = false;
            }
        }
    }
}