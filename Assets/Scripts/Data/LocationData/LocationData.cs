using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/LocationData")]
    public sealed class LocationData : ScriptableObject
    {
        #region Constants

        public const string PLAYER_SPAWNPOINT_NAME = "PlayerSpawnpoint";
        public const string BOSS_SPAWNPOINT_NAME = "BossSpawnpoint";
        public const string DO_RANDOMIZE_PLAYER_POSITION_FIELD_NAME = "_doRandomizePlayerPosition";
        public const string DO_RANDOMIZE_BOSS_POSITION_FIELD_NAME = "_doRandomizeBossPosition";
        public const string PLAYER_SPAWN_POSITIONS_FIELD_NAME = "_playerSpawnPositions";
        public const string BOSS_SPAWN_POSITIONS_FIELD_NAME = "_bossSpawnPositions";
        public const string ENEMY_SWAPN_POINT_DATA_FIELD_NAME = "_enemySpawnPointData";
        public const string INTERACTIVE_OBJECTS_SPAWN_DATA_FIELD_NAME = "_interactiveObjectSpawnData";

        #endregion


        #region Fields

        [SerializeField] private List<LocationPosition> _playerSpawnPositions;
        [SerializeField] private List<LocationPosition> _bossSpawnPositions;
        [SerializeField] private List<SpawnPointData> _enemySpawnPointData;
        [SerializeField] private List<SpawnInteractiveObjectData> _interactiveObjectSpawnData;

        [SerializeField] private bool _doRandomizePlayerPosition;
        [SerializeField] private bool _doRandomizeBossPosition;

        #endregion


        #region Properties

        public List<LocationPosition> PlayerSpawnPositions => _playerSpawnPositions;
        public List<LocationPosition> BossSpawnPositions => _bossSpawnPositions;
        public List<SpawnPointData> EnemySpawnPointData => _enemySpawnPointData;
        public List<SpawnInteractiveObjectData> InteractiveObjectSpawnData => _interactiveObjectSpawnData;

        public bool DoRandomizePlayerPosition => _doRandomizePlayerPosition;
        public bool DoRandomizeBossPosition => _doRandomizeBossPosition;

        #endregion


        #region Methods

        public void ClearLists()
        {
            _playerSpawnPositions = new List<LocationPosition>();
            _bossSpawnPositions = new List<LocationPosition>();
            _enemySpawnPointData = new List<SpawnPointData>();
            _interactiveObjectSpawnData = new List<SpawnInteractiveObjectData>();
        }

        public void AddPlayerPosition(LocationPosition position = null)
        {
            _playerSpawnPositions.Add(position == null ? 
                new LocationPosition(Vector3.zero, Vector3.zero, Vector3.zero) : position);
        }

        public void AddBossPosition(LocationPosition position = null)
        {
            _bossSpawnPositions.Add(position == null ? 
                new LocationPosition(Vector3.zero, Vector3.zero, Vector3.zero) : position);
        }

        public void AddEnemySpawnPointData()
        {
            _enemySpawnPointData.Add(new SpawnPointData());
        }

        public void AddEnemySpawnPointData(List<SpawnEntityData> spawnEntityDatas, LocationPosition position, 
            float radius, int number)
        {
            _enemySpawnPointData.Add(new SpawnPointData(spawnEntityDatas, position, radius, number));
        }

        public void AddInteractiveObjectData(SpawnInteractiveObjectData spawnData = null)
        {
            _interactiveObjectSpawnData.Add(spawnData == null ? new SpawnInteractiveObjectData(
                new LocationPosition(Vector3.zero, Vector3.zero, Vector3.zero), InteractiveObjectType.None) : spawnData);
        }

        public void PlaceObjectOnScene(GameObject objectOnScene, LocationPosition position)
        {
            objectOnScene.transform.position = position.Position;
            objectOnScene.transform.eulerAngles = position.Eulers;
            objectOnScene.transform.localScale = position.Scale;
        }

        #endregion
    }
}
