using UnityEngine;


namespace BeastHunter
{
    public sealed class ButterFlyInitilizeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public ButterFlyInitilizeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var ButterFlyData = Data.ButterFlyData;
            GameObject instance = GameObject.Instantiate(ButterFlyData.ButterFlyStruct.Prefab);
            ButterFlyModel ButterFly = new ButterFlyModel(instance, ButterFlyData);
            _context.ButterFlyModel = ButterFly;
        }

        #endregion
    }
}

