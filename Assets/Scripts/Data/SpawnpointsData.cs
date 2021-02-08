using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewSpawnpointsData", menuName = "CreateData/SpawnpointsData")]
    public sealed class SpawnpointsData : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject _playerSpawnpointPrefab;
        [SerializeField] private GameObject _bossSpawnpointPrefab;
        [SerializeField] private GameObject _enemySpawnpointPrefab;
        [SerializeField] private GameObject _interactiveObjectSpawnpointPrefab;

        #endregion


        #region Properties

        public GameObject PlayerSpawnpointPrefab => _playerSpawnpointPrefab;
        public GameObject BossSpawnpointPrefab => _bossSpawnpointPrefab;
        public GameObject EnemySpawnpointPrefab => _enemySpawnpointPrefab;
        public GameObject InteractiveObjectSpawnpointPrefab => _interactiveObjectSpawnpointPrefab;

        #endregion
    }
}

