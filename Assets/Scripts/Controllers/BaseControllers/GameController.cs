using UnityEngine;


namespace BeastHunter
{
    public sealed class GameController : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private string NameofController;
        [SerializeField]
        private string NameofContext;

        private GameStateController _activeController;

        #endregion


        #region UnityMetods

        void Start()
        {
            GameContext context = new GameContext();
            Services services = Services.SharedInstance;
            services.Initialize(context);

            _activeController = new GameSystemsController(context);
            _activeController.Initialize();
            NameofController = _activeController.ToString();
            NameofContext = context.ToString();
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
        }

        #endregion
    }
}
