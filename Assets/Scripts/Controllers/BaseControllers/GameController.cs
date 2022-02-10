using UnityEngine;


namespace BeastHunter
{
    public sealed class GameController : MonoBehaviour
    {
        #region Fields

        private GameStateController _activeController;
        [SerializeField] private GameControllerParametersData _parametersData;

        #endregion


        #region UnityMetods

        private void Awake()
        {
            GameContext context = new GameContext(_parametersData);
            Services.SharedInstance.InitializeGameServices(context);
            _parametersData.CheckParametersCorrectInput();
            _activeController = new GameSystemsController(context, _parametersData);
            _activeController.Initialize();
        }

        private void Update()
        {
            _activeController.Updating(UpdateType.Update);
        }

        private void FixedUpdate()
        {
            _activeController.Updating(UpdateType.Fixed);
        }

        private void LateUpdate()
        {
            _activeController.Updating(UpdateType.Late);
        }

        private void OnDestroy()
        {
            _activeController.TearDown();
            Services.SharedInstance.DisposeGameServices();
        }

        #endregion
    }
}
