namespace BeastHunter
{

    public sealed class QuestJournalController : IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public QuestJournalController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            _context.QuestJournalModel.Execute();
        }

        #endregion        
    }
}
