namespace BeastHunter
{
    public abstract class InteractiveObjectInitializeController : IController
    {
        #region Fields

        protected GameContext _context;

        #endregion


        #region Methods

        public InteractiveObjectInitializeController(GameContext context)
        {
            _context = context;
            Initialize();
        }

        protected abstract void Initialize();

        #endregion
    }
}
