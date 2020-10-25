using UnityEngine;

namespace BeastHunter
{
    public class ButterflyInitilizeController: IAwake
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
            ButterflyData butterflyData = Data.ButterflyData;
            GameObject butterflyObject = Object.Instantiate(butterflyData.Struct.Prefab);
            ButterflyModel butterflyModel = new ButterflyModel(butterflyObject, butterflyData);
            _context.ButterflyModel = butterflyModel;
        }

        #endregion
    }
}
