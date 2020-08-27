namespace BeastHunter
{
    public sealed class TrapController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public TrapController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            foreach (var trap in _context.TrapModels)
            {
                trap.Value.Execute();
            }
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _context.TrapModels = new System.Collections.Generic.Dictionary<int, TrapModel>();
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {

        }

        #endregion

    }

}
