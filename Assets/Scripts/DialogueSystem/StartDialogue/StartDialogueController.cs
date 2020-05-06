namespace BeastHunter
{
    public sealed class StartDialogueController : IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region Properties

        public StartDialogueModel Model { get; private set; }

        #endregion


        #region ClassLifeCycle

        public StartDialogueController(GameContext context)
        {
            _context = context;   
        }

        #endregion


        #region Updating

        public void Updating()
        {
            _context.StartDialogueModel.Execute();         
        }
        
        #endregion

    }
}
