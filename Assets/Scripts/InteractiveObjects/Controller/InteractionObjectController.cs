using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BeastHunter
{
    public class InteractionObjectController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;
        private readonly List<IAwake> _interactableObjectModelsWithIAwake;
        private readonly List<IUpdate> _interactableObjectModelsWithIUpdate;
        private readonly List<ITearDown> _interactableObjectModelsWithITearDown;
        private readonly List<InteractableObjectBehavior> _interactableObjectModelsWithInteractiveBehavior;

        #endregion


        #region ClassLifeCycles

        public InteractionObjectController(GameContext context)
        {
            _context = context;
            _interactableObjectModelsWithIAwake = _context.InteractableObjectModels.Values.
                Where(x => x is IAwake).Cast<IAwake>().ToList();
            _interactableObjectModelsWithIUpdate = _context.InteractableObjectModels.Values.
                Where(x => x is IUpdate).Cast<IUpdate>().ToList();
            _interactableObjectModelsWithITearDown = _context.InteractableObjectModels.Values.
                Where(x => x is ITearDown).Cast<ITearDown>().ToList();
            _interactableObjectModelsWithInteractiveBehavior = _context.InteractableObjectModels.Values.
                Where(x => x is IHaveInteractiveBehavior).Cast<IHaveInteractiveBehavior>().
                    Select(x => x.InteractiveBehavior).ToList();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            foreach (var interactiveBehavior in _interactableObjectModelsWithInteractiveBehavior)
            {
                interactiveBehavior.OnFilterHandler += OnFilterHandler;
                interactiveBehavior.OnTriggerEnterHandler += OnTriggerEnterHandler;
                interactiveBehavior.OnTriggerExitHandler += OnTriggerExitHandler;
            }
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            foreach (IUpdate modelWitIhUpdate in _interactableObjectModelsWithIUpdate)
            {
                modelWitIhUpdate.Updating();
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            foreach (var interactiveBehavior in _interactableObjectModelsWithInteractiveBehavior)
            {
                interactiveBehavior.OnFilterHandler -= OnFilterHandler;
                interactiveBehavior.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                interactiveBehavior.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region Methods

        private bool OnFilterHandler(Collider tagObject)
        {
            return tagObject.CompareTag(TagManager.PLAYER);
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