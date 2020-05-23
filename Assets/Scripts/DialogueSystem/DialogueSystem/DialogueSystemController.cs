namespace BeastHunter
{
    public sealed class DialogueSystemController : IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public DialogueSystemController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region Updating

        public void Updating()
        {
        }

        #endregion        
    }
}
