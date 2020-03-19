using UnityEngine;


namespace BeastHunter
{
    public sealed class SphereController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public SphereController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            _context._sphereModel.Initilize();
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            
        }

        #endregion        
    }
}


