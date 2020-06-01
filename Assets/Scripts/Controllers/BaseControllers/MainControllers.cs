namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            Add(new ButterflyInitilizeController(context));
            Add(new InitializeInteractableObjectController(context));
            Add(new ButterflyController(context));
        }

        #endregion
    }
}
