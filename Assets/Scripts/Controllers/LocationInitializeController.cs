using System;
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
        private readonly List<CreateEnemyModel> _enemiesFactoryList;
        private readonly List<CreateInteractiveObjectModel> _interactiveObjectsFactoryList;

        #endregion


        #region ClassLifeCycles

        public LocationInitializeController(GameContext context, bool doCreateCharacter, bool doCreateBoss, 
            bool doCreateMobs, bool doCreateInteractiveObjects)
        {
            _context = context;
            _locationData = Data.LocationData;
            _enemiesFactoryList = new List<CreateEnemyModel>();
            _interactiveObjectsFactoryList = new List<CreateInteractiveObjectModel>();

            AddFactories();
            RemoveSpawnPoints();

            if (doCreateCharacter) IntializePlayer();
            if (doCreateBoss) InitializeBoss();
            if (doCreateMobs) InitializeEnemies();
            if (doCreateInteractiveObjects) InitializeInteractiveObjects();
        }

        #endregion


        #region Methods

        private void AddFactories()
        {
            _enemiesFactoryList.Add(new CreateRabbitModel());
            _enemiesFactoryList.Add(new CreateHellHoundModel());
            _enemiesFactoryList.Add(new CreateTwoHeadedSnakeModel());
            _enemiesFactoryList.Add(new CreateDummyModel());

            _interactiveObjectsFactoryList.Add(new CreateTorchModel());
            _interactiveObjectsFactoryList.Add(new CreateHideBushModel());
            _interactiveObjectsFactoryList.Add(new CreateBouldersModel());
            _interactiveObjectsFactoryList.Add(new CreateFallingTreeModel());
        }

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
            var characterData = Data.CharacterData;

            LocationPosition playerPosition = _locationData.DoRandomizePlayerPosition ? _locationData.PlayerSpawnPositions.
                ReturnRandom() : _locationData.PlayerSpawnPositions[0];

            GameObject instance = GameObject.Instantiate(characterData.CharacterCommonSettings.Prefab);
            _context.CharacterModel = new CharacterModel(instance, characterData, playerPosition);
        }

        private void InitializeBoss()
        {
            var bossData = Data.BossData;

            LocationPosition bossPosition = _locationData.DoRandomizeBossPosition ? _locationData.BossSpawnPositions.
                ReturnRandom() : _locationData.BossSpawnPositions[0];

            GameObject instance = GameObject.Instantiate(bossData.Prefab);
            _context.NpcModels.Add(instance.GetInstanceID(), new BossModel(instance, bossData, bossPosition, _context));
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
                Func<GameObject, EnemyData, EnemyModel> CreateModel = null;

                var Factory = _enemiesFactoryList.FirstOrDefault(f => f.CanCreateModel(dataType));
                if (Factory == null) throw new Exception($"No factory found for datatype {dataType}");

                CreateModel = Factory.CreateModel;

                for (int i = 0; i < spawnPointData.NumberToSpawn; i++)
                {
                    Vector3 spawnRadius = new Vector3(Random.Range(-spawnPointData.SpawnRadius, spawnPointData.SpawnRadius), 0,
                        Random.Range(-spawnPointData.SpawnRadius, spawnPointData.SpawnRadius));
                    Vector3 spawnPoint = Services.SharedInstance.PhysicsService.GetGroundedPosition(spawnPointData.SpawnPosition.Position +
                        spawnRadius, 1000f);
                    GameObject instance = GameObject.Instantiate(enemyData.Prefab, spawnPoint, 
                        Quaternion.Euler(spawnPointData.SpawnPosition.Eulers));
                    EnemyModel enemy = CreateModel(instance, enemyData);
                    _context.NpcModels.Add(instance.GetInstanceID(), enemy);
                }
            }
        }

        private void InitializeInteractiveObjects()
        {
            for (int i = 0; i < _locationData.InteractiveObjectSpawnData.Count; i++)
            {
                SimpleInteractiveObjectData interactiveObjectData = null;

                var Factory = _interactiveObjectsFactoryList.FirstOrDefault(f => 
                f.CanCreateModel(_locationData.InteractiveObjectSpawnData[i].Type));
                if (Factory == null)
                    throw new Exception($"No factory found for datatype {_locationData.InteractiveObjectSpawnData[i].Type}");

                switch (_locationData.InteractiveObjectSpawnData[i].Type)
                {
                    case InteractiveObjectType.None:
                        break;
                    case InteractiveObjectType.Torch:
                        interactiveObjectData = Data.TorchObjectData;
                        break;
                    case InteractiveObjectType.HideBush:
                        interactiveObjectData = Data.HideBushData;
                        break;
                    case InteractiveObjectType.Boulders:
                        interactiveObjectData = Data.BoulderObjectData;
                        break;
                    case InteractiveObjectType.FallingTree:
                        interactiveObjectData = Data.FallingTreeData;
                        break;
                    default:
                        break;
                }

                Func<GameObject, SimpleInteractiveObjectData, SimpleInteractiveObjectModel> CreateModel = null;
                CreateModel = Factory.CreateModel;
                Vector3 spawnPoint = Services.SharedInstance.PhysicsService.GetGroundedPosition(_locationData.
                    InteractiveObjectSpawnData[i].ObjectPosition.Position, 1000f);
                GameObject instance = GameObject.Instantiate(interactiveObjectData.Prefab, spawnPoint,
                    Quaternion.Euler(_locationData.InteractiveObjectSpawnData[i].ObjectPosition.Eulers));
                SimpleInteractiveObjectModel interactiveObject = CreateModel(instance, interactiveObjectData);
                _context.InteractableObjectModels.Add(instance.GetInstanceID(), interactiveObject);
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
