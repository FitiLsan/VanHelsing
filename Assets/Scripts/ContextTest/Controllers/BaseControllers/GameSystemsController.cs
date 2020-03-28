namespace BeastHunter
{
    public sealed class GameSystemsController : GameStateController
    {
        #region ClassLifeCycles

        public GameSystemsController(GameContext context, Services services)
        {
            AddUpdateFeature(new MainControllers(context, services));
            AddLateUpdateFeature(new MainLateControllers(context, services));
            AddFixedUpdateFeature(new MainFixedControllers(context, services));
        }

        #endregion
    }
}
