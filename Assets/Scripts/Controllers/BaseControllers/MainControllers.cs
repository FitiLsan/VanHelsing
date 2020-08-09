namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            Add(new CubeInitilizeController(context));
            Add(new SphereInitilizeController(context));
            Add(new InitializeInteractableObjectController(context));
            Add(new CubeController(context));
            Add(new SphereController(context));
            Add(new TargetController(context));
            Add(new CubeTargetController(context));
        }

        #endregion
    }
}
