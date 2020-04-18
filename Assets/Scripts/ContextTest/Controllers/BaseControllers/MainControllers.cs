namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context, Services services)
        {
            Add(new CharacterInitilizeController(context));
            Add(new InitializeInteractableObjectController(context, services));
            Add(new SphereInitilizeController(context, services));
            Add(new SphereController(context, services));
            Add(new InputController(context));
            Add(new CharacterController(context));
            Add(new CharacterAnimationsController(context));
            Add(new TargetController(context, services));
        }

        #endregion
    }
}
