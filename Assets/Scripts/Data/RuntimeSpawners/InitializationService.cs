using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeastHunter
{
    public sealed class InitializationService : IService
    {
        #region Fields

        private readonly GameContext _context;
        private readonly List<CreateEnemyModel> _enemiesFactoryList;
        private readonly List<CreateInteractiveObjectModel> _interactiveObjectsFactoryList;

        #endregion


        #region ClassLifeCycles

        public InitializationService(GameContext context)
        {
            _context = context;
            _enemiesFactoryList = new List<CreateEnemyModel>();
            _interactiveObjectsFactoryList = new List<CreateInteractiveObjectModel>();

            AddFactories();
        }

        #endregion


        #region Methods

        private void AddFactories()
        {
            _enemiesFactoryList.Add(new CreateRabbitModel());
            _enemiesFactoryList.Add(new CreateHellHoundModel());
            _enemiesFactoryList.Add(new CreateTwoHeadedSnakeModel());
            _enemiesFactoryList.Add(new CreateDummyModel());

            _interactiveObjectsFactoryList.Add(new CreateEntitySpawnerModel());
            _interactiveObjectsFactoryList.Add(new CreateTorchModel());
            _interactiveObjectsFactoryList.Add(new CreateHideBushModel());
            _interactiveObjectsFactoryList.Add(new CreateBouldersModel());
            _interactiveObjectsFactoryList.Add(new CreateFallingTreeModel());
            _interactiveObjectsFactoryList.Add(new CreateBallistaModel(_context));
        }

        public void InitializeEnemy(EnemyData data, LocationPosition position, float radius = 0.0f, int number = 1)
        {
            Vector3 parentPosition = Vector3.zero;
            InitializeEnemy(data, position, parentPosition, radius, number);
        }


        public void InitializeEnemy(EnemyData data, LocationPosition position, Vector3 parentPosition, float radius = 0.0f, int number = 1)
        {
            Func<GameObject, EnemyData, EnemyModel> CreateModel = null;
            var Factory = _enemiesFactoryList.FirstOrDefault(f => f.CanCreateModel(data.DataType));
            if (Factory == null)
                throw new Exception($"No factory found for datatype {data.DataType}");
            CreateModel = Factory.CreateModel;
            Vector3 globalPosition = position.Position + parentPosition;
            for (int i = 0; i < number; i++)
            {
                Vector3 spawnRadius = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
                Vector3 spawnPoint = Services.SharedInstance.PhysicsService.GetGroundedPosition(globalPosition +
                    spawnRadius, 1000f);
                GameObject instance = GameObject.Instantiate(data.Prefab, spawnPoint,
                        Quaternion.Euler(position.Eulers));
                EnemyModel enemy = CreateModel(instance, data);
                _context.NpcModels.Add(instance.GetInstanceID(), enemy);
            }     
        }

        public void InitializeInteractiveObject(InteractiveObjectType dataType, LocationPosition position)
        {
            Vector3 parentPosition = Vector3.zero;
            InitializeInteractiveObject(dataType, position, parentPosition);
        }

        public void InitializeInteractiveObject(InteractiveObjectType dataType, LocationPosition position, Vector3 parentPosition)
        {
            BaseInteractiveObjectData interactiveObjectData = null;

            var Factory = _interactiveObjectsFactoryList.FirstOrDefault(f =>
            f.CanCreateModel(dataType));
            if (Factory == null)
                throw new Exception($"No factory found for datatype {dataType}");

            switch (dataType)
            {
                case InteractiveObjectType.None:
                    break;
                case InteractiveObjectType.EntitySpawner:
                    interactiveObjectData = Data.EntitySpawnerData;
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
                case InteractiveObjectType.Ballista:
                    interactiveObjectData = Data.BallistaData;
                    break;
                default:
                    break;
            }

            Func<GameObject, BaseInteractiveObjectData, BaseInteractiveObjectModel> CreateModel = null;
            CreateModel = Factory.CreateModel;
            Vector3 spawnPoint = Services.SharedInstance.PhysicsService.GetGroundedPosition(position.Position + parentPosition, 1000f);
            GameObject instance = GameObject.Instantiate(interactiveObjectData.Prefab, spawnPoint,
                Quaternion.Euler(position.Eulers));
            BaseInteractiveObjectModel interactiveObject = CreateModel(instance, interactiveObjectData);
            _context.InteractiveObjectModels.Add(instance.GetInstanceID(), interactiveObject);
        }

        #endregion
    }
}
