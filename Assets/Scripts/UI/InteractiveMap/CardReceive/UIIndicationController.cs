namespace BeastHunter
{
    public sealed class UIIndicationController : IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public UIIndicationController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            _context.UIIndicationModel.Execute();
        }

        #endregion
    }
}