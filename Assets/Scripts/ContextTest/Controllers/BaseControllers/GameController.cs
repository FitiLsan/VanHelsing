using UnityEngine;


namespace BeastHunter
{
    public sealed class GameController : MonoBehaviour
    {
        #region Fields

        private GameStateController _activeController;

        #endregion


        #region UnityMetods

        void Start()
        {
            GameContext context = new GameContext();
            Services services = Services.SharedInstance;
            services.Initialize(context);

            _activeController = new GameSystemsController(context, services);
            _activeController.Initialize();
        }
        private void Update()
        {
            _activeController.Updating(UpdateType.Update);
        }

        #endregion
    }
}
