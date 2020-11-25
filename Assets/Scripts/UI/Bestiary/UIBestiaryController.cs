namespace BeastHunter
{
    public sealed class UIBestiaryController : IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion

        #region ClassLifeCycle

        public UIBestiaryController(GameContext context)
        {
            _context = context;
        }

        #endregion

        #region IUpdate

        public void Updating()
        {
            _context.UIBestiaryModel.Execute();
        }

        #endregion
    }
}
