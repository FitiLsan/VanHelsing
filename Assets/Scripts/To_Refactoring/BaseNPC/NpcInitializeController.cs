using UnityEngine;

namespace BeastHunter
{
    public abstract class NpcInitializeController : IAwake
    {
        #region Fields

        protected GameContext _context;

        #endregion


        #region ClassLifeCycles

        public NpcInitializeController(GameContext context)
        {
            _context = context;
        }

        public abstract void OnAwake();

        #endregion
    }
}
