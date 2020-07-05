using UnityEngine;

namespace BeastHunter
{
    public class BossInitializeController : IAwake
    {
        #region Fields

        private GameContext _context;

        #endregion


        #region ClassLifeCycles

        public BossInitializeController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var BossData = Data.BossData;
            GameObject instance = GameObject.Instantiate(BossData.prefab);
            BossModel Boss = new BossModel(instance, BossData);
            _context.BossModel = Boss;
        }

        #endregion
    }
}
