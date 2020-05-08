namespace BeastHunter
{
    public sealed class QuestController: IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region Properties

        public QuestModel Model { get; private set; }

        #endregion


        #region ClassLifeCycle

        public QuestController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            _context.QuestModel.Execute();
        }

        #endregion
    }
}
