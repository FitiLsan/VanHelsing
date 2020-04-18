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
            Add(new CharacterInputController(context));
            Add(new GiantMudCrabController(context));
            Add(new TargetController(context));
        }

        #endregion
    }
}
