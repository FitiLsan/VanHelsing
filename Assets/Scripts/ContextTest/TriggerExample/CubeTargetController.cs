using UnityEngine;


namespace BeastHunter
{
    public sealed class CubeTargetController : IAwake, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public CubeTargetController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var cubes = _context.GetTriggers(InteractableObjectType.Cube);
            foreach (var trigger in cubes)
            {
                var targetBehaviour = trigger as CubeTargetBehaviour;
                targetBehaviour.OnFilterHandler += OnFilterHandler;
                targetBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                targetBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var cubes = _context.GetTriggers(InteractableObjectType.Cube);
            foreach (var trigger in cubes)
            {
                var targetBehaviour = trigger as CubeTargetBehaviour;
                targetBehaviour.OnFilterHandler -= OnFilterHandler;
                targetBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                targetBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region Methods

        private bool OnFilterHandler(Collider tagObject)
        {
            Debug.Log(tagObject.tag);
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