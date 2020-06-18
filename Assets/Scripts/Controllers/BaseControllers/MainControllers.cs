namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            Add(new InitializeInteractableObjectController(context));
        }

        #endregion
    }
}
