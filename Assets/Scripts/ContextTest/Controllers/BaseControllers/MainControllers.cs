namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            Add(new CharacterInitilizeController(context));
            Add(new GiantMudCrabInitilizeController(context));
            Add(new InitializeInteractableObjectController(context));
            Add(new SphereInitilizeController(context));
            Add(new SphereController(context));
            Add(new GiantMudCrabController(context));
            Add(new TargetController(context));
            Add(new InputController(context));
            Add(new CharacterController(context));
            Add(new CharacterAnimationsController(context));
        }

        #endregion
    }
}
