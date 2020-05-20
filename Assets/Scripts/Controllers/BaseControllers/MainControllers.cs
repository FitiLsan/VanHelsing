namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            Add(new SphereInitilizeController(context));
            Add(new SphereController(context));
        }

        #endregion
    }
}
