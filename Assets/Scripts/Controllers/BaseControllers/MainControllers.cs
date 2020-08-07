namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            Add(new SurvivorInitializeController(context));
            Add(new ZombieInitializeController(context));
            Add(new InitializeInteractableObjectController(context));
            Add(new SurvivorController(context));
            Add(new ZombieController(context));
        }

        #endregion
    }
}
