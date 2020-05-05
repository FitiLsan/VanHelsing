namespace BeastHunter
{
    public sealed class QuestInitializeController : IAwake
    {
        #region Fields

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public QuestInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            QuestModel Quest = new QuestModel(new DbQuestStorage(new SaveManager(new ProgressDatabaseWrapper())), _context);
            _context.QuestModel = Quest;
        }

        #endregion
    }
}
