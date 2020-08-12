namespace BeastHunter
{
    public sealed class ZombieController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public ZombieController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _context.ZombieModel.TargetDirection = _context.SurvivorModel.Transform.position;
            _context.ZombieModel.Execute();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            
        }

        #endregion
    }
}