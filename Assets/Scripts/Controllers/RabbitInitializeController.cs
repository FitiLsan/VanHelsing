using UnityEngine;

namespace BeastHunter
{
    public class RabbitInitializeController : EnemyInitializeController
    {
        #region Fields

        private int number;

        #endregion

        #region ClassLifeCycles

        public RabbitInitializeController(GameContext context, int number) : base(context)
        {
            this.number = number;
        }

        #endregion


        #region IAwake

        public override void OnAwake()
        {
            var RabbitData = Data.RabbitData;
            var SpawnPoint = GameObject.Find(RabbitData.BaseStats.SpawnPointName);
            //GameObject instance = GameObject.Instantiate(RabbitData.BaseStats.Prefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
            //RabbitModel rabbit = new RabbitModel(instance, RabbitData);
            //_context.EnemyModels.Add(instance.GetInstanceID(), rabbit);

            //GameObject instance2 = GameObject.Instantiate(RabbitData.RabbitStruct.Prefab, new Vector3(2, 0.5f, 2), Quaternion.identity);
            //RabbitModel Rabbit2 = new RabbitModel(instance2, RabbitData);
            //_context.RabbitModels.Add(Rabbit2);

            for(int i=0; i<number; i++)
            {
                GameObject instance = GameObject.Instantiate(RabbitData.BaseStats.Prefab,
                                        SpawnPoint.transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3)), SpawnPoint.transform.rotation);
                RabbitModel rabbit = new RabbitModel(instance, RabbitData);
                _context.EnemyModels.Add(instance.GetInstanceID(), rabbit);
            }
        }

        #endregion
    }
}
