namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            Add(new ButterflyInitilizeController(context));
            Add(new ButterflyController(context));

            //Example Sphere
            //Add(new SphereInitilizeController(context));
            //Add(new InitializeInteractableObjectController(context));
            //Add(new SphereController(context));
            //Add(new TargetController(context));
        }

        #endregion
    }
}
