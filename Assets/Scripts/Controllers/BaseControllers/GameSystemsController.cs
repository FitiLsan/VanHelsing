namespace BeastHunter
{
    public sealed class GameSystemsController : GameStateController
    {
        #region ClassLifeCycles

        public GameSystemsController(GameContext context, GameControllerParametersData controllerData)
        {
            AddUpdateFeature(new MainControllers(context, controllerData));
            AddLateUpdateFeature(new MainLateControllers(context, controllerData));
            AddFixedUpdateFeature(new MainFixedControllers(context, controllerData));
        }

        #endregion
    }
}
