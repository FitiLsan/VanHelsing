using UnityEngine;


namespace BeastHunter
{
    public sealed class ButterflyController : IUpdate
    {
        #region Fields

        private GameContext _context;

        #endregion


        #region ClassLifeCycle

        public ButterflyController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            Debug.Log("update");
            _context.ButterflyModel.Execute();
        }

        #endregion
    }
}
