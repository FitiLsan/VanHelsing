using UnityEngine;


namespace BeastHunter
{
    public sealed class ButterflyInitilizeController : IAwake
    {
        #region Field

        private GameContext _context;

        #endregion


        #region ClassLifeCycle

        public ButterflyInitilizeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var ButterflyData = Data.ButterflyData;
            GameObject instance = GameObject.Instantiate(ButterflyData.ButterflyStruct.Prefab);
            ButterflyModel butterfly = new ButterflyModel(instance, ButterflyData);
            _context.ButterflyModel = butterfly;
        }

        #endregion
    }
}
