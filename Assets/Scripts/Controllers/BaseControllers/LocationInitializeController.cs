using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BeastHunter
{
    public sealed class LocationInitializeController : IController
    {
        #region Fields

        private readonly GameContext _context;
        private readonly LocationData _locationData;
        private readonly List<CreateEnemyModel> _enemiesFactoryList;

        #endregion


        #region ClassLifeCycles

        public LocationInitializeController(GameContext context)
        {
            _context = context;
            _locationData = Data.LocationData;
            _enemiesFactoryList = new List<CreateEnemyModel>();

            AddFactories();
            RemoveSpawnPoints();

            IntializePlayer();
            InitializeEnemies();
        }

        #endregion


        #region Methods

        private void AddFactories()
        {
            // _enemiesFactoryList.Add(new CreateRabbitModel());
            _enemiesFactoryList.Add(new CreateButterflyModel(() => _context.CharacterModel.CharacterTransform.gameObject));
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

            LocationPosition playerPosition = _locationData.PlayerSpawnPosition;

            GameObject instance = GameObject.Instantiate(characterData._characterCommonSettings.Prefab);
            _context.CharacterModel = new CharacterModel(instance, characterData, playerPosition.Position);
        }


        private void InitializeEnemies()
        {
            foreach (var spawnPointData in _locationData.EnemySpawnPointData)
            {
                SpawnEntityData spawnEnemyData = spawnPointData.SpawnData;

                DataType dataType = spawnEnemyData.SpawningDataType;
                EnemyData enemyData = spawnEnemyData.SpawningEnemyData;
                Func<GameObject, EnemyData, EnemyModel> CreateModel = null;

                var Factory = _enemiesFactoryList.FirstOrDefault(f => f.CanCreateModel(dataType));
                if (Factory == null) throw new Exception($"No factory found for datatype {dataType}");

                CreateModel = Factory.CreateModel;

                GameObject instance = GameObject.Instantiate(enemyData.BaseStats.Prefab, spawnPointData.SpawnPosition.Position, Quaternion.identity);
                EnemyModel enemy = CreateModel(instance, enemyData);
                _context.EnemyModels.Add(instance.GetInstanceID(), enemy);
            }
        }

        #endregion
    }
}