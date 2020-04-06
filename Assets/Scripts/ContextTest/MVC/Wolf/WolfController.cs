using UnityEngine;


namespace BeastHunter
{
    public class WolfController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifrCycle

        public WolfController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            _context._wolfModel.Initialize();
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            
        }

        #endregion
    }
}
