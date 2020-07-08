using UnityEngine;


namespace BeastHunter
{
    public sealed class PlaceSearcherController : IAwake
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public PlaceSearcherController(GameContext context)
        {
            _context = context;
        }

        #endregion

        public void OnAwake()
        {
            PlaceSearcher placeSearcher = new PlaceSearcher(_context);
        }
    }
}