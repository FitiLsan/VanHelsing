using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeastHunter
{
    public sealed class LocationInitializeController : IAwake
    {
        #region Fields

        private List<CreateEnemyModel> _factoryList;
        private GameContext _context;

        #endregion


        #region ClassLifeCycles

        public LocationInitializeController(GameContext context)
        {
            _context = context;
            _factoryList = new List<CreateEnemyModel>();
            AddFactories();
        }

        #endregion


        #region Methods

        private void AddFactories()
        {
            _factoryList.Add(new CreateRabbitModel());
        }

        private void RemoveSpawnPoints()
        {
            var spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint"); //TAG MANAGER
            foreach (var point in spawnPoints)
            {
                GameObject.Destroy(point);
            }
        }

        private void InitializeEnemies(LocationData locationData)
        {
            foreach (var spawnPointData in locationData.EnemySpawnPointData)
            {
                var chancesSum = spawnPointData.SpawnDataList.Sum(data => data.SpawningChance);
                if (!Mathf.Approximately(chancesSum, 100.0f))
                    RecalculateChances(spawnPointData.SpawnDataList, chancesSum);
                float chance = Random.Range(0.0f, 100.0f);
                SpawnEntityData spawnEnemyData;
                int k = 0;
                float chanceThreshhold = 0;
                do
                {
                    chanceThreshhold += spawnPointData.SpawnDataList[k].SpawningChance;
                    spawnEnemyData = spawnPointData.SpawnDataList[k];
                    k++;
                } while (chance >= chanceThreshhold && k <= spawnPointData.SpawnDataList.Count);

                var dataType = spawnEnemyData.SpawningDataType;
                var enemyData = spawnEnemyData.SpawningEnemyData;
                var gameObject = enemyData.BaseStats.Prefab;
                var spawnPoint = spawnPointData.SpawnPoint;
                var radius = spawnPointData.SpawnRadius;

                Func<GameObject, EnemyData, EnemyModel> CreateModel = null;

                try
                {
                    var Factory = _factoryList.FirstOrDefault(f => f.CanCreateModel(dataType));
                    if (Factory == null)
                        throw new Exception($"No factory found for datatype {dataType}");

                    CreateModel = Factory.CreateModel;
                    if (spawnPointData.NumberToSpawn > 1)
                    {

                        for (int i = 0; i < spawnPointData.NumberToSpawn; i++)
                        {
                            Vector3 spawnRadius = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
                            GameObject instance = GameObject.Instantiate(gameObject, spawnPoint + spawnRadius, Quaternion.identity);
                            EnemyModel enemy = CreateModel(instance, enemyData);
                            _context.NpcModels.Add(instance.GetInstanceID(), enemy);
                        }

                    }
                    else
                    {
                        GameObject instance = GameObject.Instantiate(gameObject, spawnPoint, Quaternion.identity);
                        EnemyModel enemy = CreateModel(instance, enemyData);
                        _context.NpcModels.Add(instance.GetInstanceID(), enemy);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        private void RecalculateChances(List<SpawnEntityData> spawnDatas, float sum)
        {
            //chances = new List<float>(spawnDatas.Count());
            //float sum = spawnDatas.Sum(data => data.SpawningChance);
            foreach (var data in spawnDatas)
            {
                //chances.Add(data.SpawningChance / sum);
                data.SpawningChance /= sum;
            }
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var locationData = Data.LocationData;
            RemoveSpawnPoints();
            InitializeEnemies(locationData);            
        }

        #endregion
    }
}
