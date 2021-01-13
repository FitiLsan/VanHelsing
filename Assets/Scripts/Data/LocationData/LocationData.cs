using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/LocationData")]
    public sealed class LocationData : ScriptableObject
    {
        #region Fields

        [SerializeField] private Vector3 _playerSpawnPosition;
        [SerializeField] private Vector3 _bossSpawnPosition;
        [SerializeField] private List<SpawnPointData> _enemySpawnPointData;
        
        #endregion


        #region Properties

        public Vector3 PlayerSpawnPosition { get => _playerSpawnPosition; }
        public Vector3 BossSpawnPosition { get => _bossSpawnPosition; }
        public List<SpawnPointData> EnemySpawnPointData { get => _enemySpawnPointData; }

        #endregion

        public void AddEnemySpawnPointData()
        {
            _enemySpawnPointData.Add(new SpawnPointData());
        }

        public void AddEnemySpawnPointData(List<SpawnEntityData> spawnEntityDatas, Vector3 point, float radius, int number)
        {
            _enemySpawnPointData.Add(new SpawnPointData(spawnEntityDatas, point, radius, number));
        }
    }

}
