using UnityEngine;


namespace BeastHunter
{
    public sealed class ButterflyController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public ButterflyController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            _context._butterflyModel.Initilize();
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {

        }

        #endregion        
    }
}