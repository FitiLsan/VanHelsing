using UnityEngine;


namespace BeastHunter
{
    public class ButterflyController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region Methods

        public ButterflyController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _context.ButterflyModel.Initilize();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            
        }

        #endregion
    }
}
