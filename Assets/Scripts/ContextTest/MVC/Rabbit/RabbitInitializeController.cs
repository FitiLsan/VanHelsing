using UnityEngine;

namespace BeastHunter
{
    public class RabbitInitializeController : IAwake
    {
        #region Fields

        private GameContext _context;

        #endregion


        #region ClassLifeCycles

        public RabbitInitializeController(GameContext context, Services services)
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
            _context.RabbitModel.Add(Rabbit);
        }

        #endregion
    }
}
