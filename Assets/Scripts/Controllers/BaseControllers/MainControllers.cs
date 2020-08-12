namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            Add(new CubeInitilizeController(context));
            Add(new InitializeInteractableObjectController(context));
            Add(new CubeController(context));
            Add(new TargetController(context));
        }

        #endregion
    }
}
