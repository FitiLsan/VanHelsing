using UnityEngine;


namespace BeastHunter
{
    public sealed class TargetController : IAwake, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public TargetController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var sphers = _context.GetTriggers(InteractableObjectType.Cube);
            foreach (var trigger in sphers)
            {
                var targetBehaviour = trigger as TargetBehaviour;
                targetBehaviour.OnFilterHandler += OnFilterHandler;
                targetBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                targetBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var sphers = _context.GetTriggers(InteractableObjectType.Cube);
            foreach (var trigger in sphers)
            {
                var targetBehaviour = trigger as TargetBehaviour;
                targetBehaviour.OnFilterHandler -= OnFilterHandler;
                targetBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                targetBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region Methods

        private bool OnFilterHandler(Collider tagObject)
        {
            return tagObject.CompareTag(TagManager.SPHERE);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject, Collider other)
        {
            enteredObject.IsInteractable = true;
            Debug.Log("Enter");
        }

        private void OnTriggerExitHandler(ITrigger exitedObject, Collider other)
        {
            exitedObject.IsInteractable = false;
            Debug.Log("Exit");
        }

        #endregion
    }

}
