using UnityEngine;


namespace BeastHunter
{
    public sealed class ButterflyInitilizeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public ButterflyInitilizeController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var ButterflyData = Data.ButterflyData;
            GameObject instance = GameObject.Instantiate(ButterflyData.ButterflyStruct.Prefab);
            ButterflyModel Butterfly = new ButterflyModel(instance, ButterflyData);
            _context.ButterflyModel = Butterfly;
        }

        #endregion
    }
}