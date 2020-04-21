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
            foreach(var wolf in _context.WolfModel)
            {
                wolf.Patroling();
            }
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {

        }

        #endregion
    }
}
