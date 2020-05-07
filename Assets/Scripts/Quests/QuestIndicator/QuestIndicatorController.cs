namespace BeastHunter
{
  public sealed  class QuestIndicatorController : IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region Properties

        public QuestIndicatorModel Model { get; private set; }

        #endregion


        #region ClassLifeCycle

        public QuestIndicatorController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            foreach (QuestIndicatorModel questIndicatorModel in _context.QuestIndicatorModelList)
            {
                questIndicatorModel.Execute();
            }
        }

        #endregion
    }
}
