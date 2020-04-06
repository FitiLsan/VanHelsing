namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context, Services services)
        {
            Add(new SphereInitilizeController(context, services));
            Add(new SphereController(context, services));
            Add(new InitializeInteractableObjectController(context, services));
            Add(new WolfController(context, services));
            Add(new WolfInitializeController(context, services));
        }

        #endregion
    }
}
