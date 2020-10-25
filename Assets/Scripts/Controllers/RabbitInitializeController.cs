using UnityEngine;

namespace BeastHunter
{
    public class RabbitInitializeController : EnemyInitializeController
    {
        #region ClassLifeCycles

        public RabbitInitializeController(GameContext context) : base(context)
        { }

        #endregion


        #region IAwake

        public override void OnAwake()
        {
            var RabbitData = Data.RabbitData;
            GameObject instance = GameObject.Instantiate(RabbitData.BaseStats.Prefab);
            RabbitModel rabbit = new RabbitModel(instance, RabbitData);
            _context.NpcModels.Add(instance.GetInstanceID(), rabbit);

            //GameObject instance2 = GameObject.Instantiate(RabbitData.RabbitStruct.Prefab, new Vector3(2, 0.5f, 2), Quaternion.identity);
            //RabbitModel Rabbit2 = new RabbitModel(instance2, RabbitData);
            //_context.RabbitModels.Add(Rabbit2);
        }

        #endregion
    }
}
