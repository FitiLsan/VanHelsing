﻿namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context, Services services)
        {
            Add(new InitializeInteractableObjectController(context, services));
            Add(new SphereInitilizeController(context, services));
            Add(new SphereController(context, services));
            Add(new CharacterInitilizeController(context, services));
            Add(new CharacterInputController(context, services));
            Add(new TargetController(context, services));
        }

        #endregion
    }
}
