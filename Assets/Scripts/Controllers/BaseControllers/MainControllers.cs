namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            AddInitializeControllers(context);
            Add(new InitializeInteractableObjectController(context));
            AddControllers(context);
        }

        #endregion


        #region Methods

        private void AddInitializeControllers(GameContext context)
        {
            Add(new LocationInitializeController(context));
        }

        private void AddControllers(GameContext context)
        {
            Add(new EnemyController(context));
            Add(new TargetController(context));
            Add(new InputController(context));
            Add(new CharacterController(context));
        }

        #endregion
    }
}
