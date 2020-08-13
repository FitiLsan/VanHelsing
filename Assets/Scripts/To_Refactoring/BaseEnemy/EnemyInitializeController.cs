using UnityEngine;

namespace BeastHunter
{
    public abstract class EnemyInitializeController : IAwake
    {
        #region Fields

        protected GameContext _context;

        #endregion


        #region ClassLifeCycles

        public EnemyInitializeController(GameContext context)
        {
            _context = context;
        }

        public abstract void OnAwake();

        #endregion
    }
}
