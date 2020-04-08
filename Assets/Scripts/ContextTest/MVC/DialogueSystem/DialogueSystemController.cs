namespace BeastHunter
{
    public sealed class DialogueSystemController : IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region Properties

        public DialogueSystemModel Model { get; private set; }

        #endregion


        #region ClassLifeCycle

        public DialogueSystemController(GameContext context, Services services)
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
