using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using Random = UnityEngine.Random;


namespace BeastHunter
{
    public sealed class LocationInitializeController : IController
    {
        #region Fields

        private readonly GameContext _context;
        private readonly LocationData _locationData;
        private readonly InitializationService _initializationService;

        #endregion


        #region ClassLifeCycles

        public LocationInitializeController(GameContext context, bool doCreateCharacter, bool doCreateBoss, 
            bool doCreateMobs, bool doCreateInteractiveObjects)
        {
            _context = context;
            _locationData = Data.LocationData;
            _initializationService = Services.SharedInstance.InitializationService;

            RemoveSpawnPoints();

            if (doCreateCharacter) IntializePlayer();
            if (doCreateBoss) InitializeBoss();
            if (doCreateMobs) InitializeEnemies();
            if (doCreateInteractiveObjects) InitializeInteractiveObjects();
        }

        #endregion


        #region Methods

        private void RemoveSpawnPoints()
        {
            var spawnPoints = GameObject.FindGameObjectsWithTag(TagManager.SPAWNPOINT);
            foreach (var point in spawnPoints)
            {
                GameObject.Destroy(point);
            }
        }

        private void IntializePlayer()
        {
            if (_locationData.PlayerSpawnPositions.Count > 0)
            {
                var characterData = Data.CharacterData;

                LocationPosition playerPosition = _locationData.DoRandomizePlayerPosition ? _locationData.PlayerSpawnPositions.
                    ReturnRandom() : _locationData.PlayerSpawnPositions[0];

                GameObject instance = GameObject.Instantiate(characterData.CharacterCommonSettings.Prefab);
                _context.CharacterModel = new CharacterModel(instance, characterData, playerPosition);
            }
        }

        private void InitializeBoss()
        {
            if (_locationData.BossSpawnPositions.Count > 0)
            {
                var bossData = Data.BossData;

                LocationPosition bossPosition = _locationData.DoRandomizeBossPosition ? _locationData.BossSpawnPositions.
                    ReturnRandom() : _locationData.BossSpawnPositions[0];

                GameObject instance = GameObject.Instantiate(bossData.Prefab);
                _context.NpcModels.Add(instance.GetInstanceID(), new BossModel(instance, bossData, bossPosition, _context));
            }
        }

        private void InitializeEnemies()
        {
            foreach (var spawnPointData in _locationData.EnemySpawnPointData)
            {
                RecalculateChances(spawnPointData);
                float randomSpawnChance = Random.Range(0.0f, 1.0f);
                List<float> enemySpawnChanses = spawnPointData.SpawnDataList.Select(x => x.SpawningChance).ToList();
                List<float> cumulatedEnemySpawnChanses = enemySpawnChanses.ToArray().CumulateValues().ToList();
                int chosenEnemyIndex = cumulatedEnemySpawnChanses.FindIndex(x => x >= randomSpawnChance);

                SpawnEntityData spawnEnemyData = spawnPointData.SpawnDataList[chosenEnemyIndex];

                DataType dataType = spawnEnemyData.SpawningDataType;
                EnemyData enemyData = spawnEnemyData.SpawningEnemyData;

                _initializationService.InitializeEnemy(enemyData, spawnPointData.SpawnPosition, spawnPointData.SpawnRadius, spawnPointData.NumberToSpawn);
            }
        }

        private void InitializeInteractiveObjects()
        {
            for (int i = 0; i < _locationData.InteractiveObjectSpawnData.Count; i++)
            {
                InteractiveObjectType type = _locationData.InteractiveObjectSpawnData[i].Type;
                LocationPosition position = _locationData.InteractiveObjectSpawnData[i].ObjectPosition;


                _initializationService.InitializeInteractiveObject(type, position);
            }
        }

        private void RecalculateChances(SpawnPointData spawnPointData)
        {
            float sum = spawnPointData.SpawnDataList.Sum(data => data.SpawningChance);

            foreach (var data in spawnPointData.SpawnDataList)
            {
                data.SpawningChance /= sum;
            }
        }

        #endregion
    }
}
