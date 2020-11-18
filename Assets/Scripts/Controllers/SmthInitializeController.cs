using UnityEngine;

namespace BeastHunter
{
    public class SmthInitializeController : IAwake
    {
        #region Fields

        protected GameContext _context;

        #endregion


        #region ClassLifeCycles

        public SmthInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var locationData = Resources.Load<LocationData>("Test");
            //var data = locationData.OneEnemySpawnData.SpawningEnemyData;
            //var gameObject = locationData.OneEnemySpawnData.SpawningEnemyData.BaseStats.Prefab;
            //var spawnPoint = locationData.OneEnemySpawnData.SpawnPoint;
            //var radius = locationData.OneEnemySpawnData.SpawnRadius;
            //Vector3 spawnRadius = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
            //if (locationData.OneEnemySpawnData.NumberToSpawn > 1)
            //{

            //    for (int i = 0; i < locationData.OneEnemySpawnData.NumberToSpawn; i++)
            //    {
            //        GameObject instance = GameObject.Instantiate(gameObject, spawnPoint + spawnRadius, Quaternion.identity);
            //        RabbitModel enemy = new RabbitModel(instance, (RabbitData)data);
            //        _context.NpcModels.Add(instance.GetInstanceID(), enemy);
            //    }

            //}
            //else
            //{
            //    GameObject instance = GameObject.Instantiate(gameObject, spawnPoint, Quaternion.identity);
            //    RabbitModel enemy = new RabbitModel(instance, (RabbitData)data);
            //    //EnemyModel enemy = CreateModel(instance, data);
            //    _context.NpcModels.Add(instance.GetInstanceID(), enemy);
            //}

            //RabbitModel Rabbit2 = new RabbitModel(instance2, RabbitData);
        }

        #endregion

        //private void CreateModel<T>(out T model, GameObject instance, LocationData locationData) where T : EnemyData, new()
        //{
        //    model = new T(instance, (T)locationData.OneEnemySpawnData.SpawningEnemyData);
        //}
    }
}
