using UnityEngine;


namespace BeastHunter
{
    public sealed class InitializeInteractableObjectController : IAwake
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public InitializeInteractableObjectController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var triggers = Object.FindObjectsOfType<InteractableObjectBehavior>();

            foreach (var trigger in triggers)
            {
                _context.AddTriggers(trigger.Type, trigger);
            }
        }

        #endregion
    }
}
