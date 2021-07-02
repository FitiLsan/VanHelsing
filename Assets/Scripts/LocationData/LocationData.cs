using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/LocationData")]
    public sealed class LocationData : ScriptableObject
    {
        #region Constants

        public const string PLAYER_SPAWNPOINT_NAME = "PlayerSpawnpoint";
        public const string PLAYER_SPAWN_POSITIONS_FIELD_NAME = "_playerSpawnPositions";
        public const string ENEMY_SWAPN_POINT_DATA_FIELD_NAME = "_enemySpawnPointData";

        #endregion


        #region Fields

        [SerializeField] private LocationPosition _playerSpawnPosition;
        [SerializeField] private List<SpawnPointData> _enemySpawnPointData;

        #endregion


        #region Properties

        public LocationPosition PlayerSpawnPosition => _playerSpawnPosition;
        public List<SpawnPointData> EnemySpawnPointData => _enemySpawnPointData;

        #endregion


        #region Methods

        public void AddEnemySpawnPointData()
        {
            _enemySpawnPointData.Add(new SpawnPointData());
        }

        public void AddEnemySpawnPointData(SpawnEntityData spawnEntityData, LocationPosition position)
        {
            _enemySpawnPointData.Add(new SpawnPointData(spawnEntityData, position));
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