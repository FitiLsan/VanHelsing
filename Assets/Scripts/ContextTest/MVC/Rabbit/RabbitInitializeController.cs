using UnityEngine;

namespace BeastHunter
{
    public class RabbitInitializeController : IAwake
    {
        #region Fields

        private GameContext _context;

        #endregion


        #region ClassLifeCycles

        public RabbitInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var RabbitData = Data.RabbitData;
            GameObject instance = GameObject.Instantiate(RabbitData.RabbitStruct.Prefab);
            RabbitModel Rabbit = new RabbitModel(instance, RabbitData);
            _context.RabbitModel = Rabbit;
            //_context.RabbitModels.Add(Rabbit);

            //GameObject instance2 = GameObject.Instantiate(RabbitData.RabbitStruct.Prefab, new Vector3(2, 0.5f, 2), Quaternion.identity);
            //RabbitModel Rabbit2 = new RabbitModel(instance2, RabbitData);
            //_context.RabbitModels.Add(Rabbit2);
        }

        #endregion
    }
}
