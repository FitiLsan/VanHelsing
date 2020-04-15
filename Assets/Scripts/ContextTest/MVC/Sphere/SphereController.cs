using UnityEngine;


namespace BeastHunter
{
    public sealed class SphereController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public SphereController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _context.SphereModel.Initilize();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            
        }

        #endregion
    }
}


