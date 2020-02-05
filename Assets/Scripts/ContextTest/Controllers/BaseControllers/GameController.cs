using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ContextTest
{
    public class GameController : MonoBehaviour
    {
        #region Feilds

        private GameStateController _activeController;

        #endregion


        #region UnityMetods

        void Start()
        {
            GameContext context = new GameContext();
            Services services = Services.SharedInstance;
            //services.Initialize(context);

            _activeController = new GameSystemsController(context, services);
            _activeController.Initialize();
        }
        private void Update()
        {
            _activeController.Tick(TickType.Update);
        }

        #endregion
    }
}
