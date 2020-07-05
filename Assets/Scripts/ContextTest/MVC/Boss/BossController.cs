using UnityEngine;

namespace BeastHunter
{
    public sealed class BossController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public BossController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _context.BossModel.Execute();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {

        }

        #endregion
    }
}
