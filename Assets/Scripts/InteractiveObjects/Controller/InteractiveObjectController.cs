using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BeastHunter
{
    public class InteractiveObjectController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;
        private readonly List<IAwake> _interactableObjectModelsWithIAwake;
        private readonly List<IUpdate> _interactableObjectModelsWithIUpdate;
        private readonly List<ITearDown> _interactableObjectModelsWithITearDown;
        private readonly List<InteractableObjectBehavior> _interactableObjectModelsWithInteractiveBehavior;

        #endregion


        #region ClassLifeCycles

        public InteractiveObjectController(GameContext context)
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

            foreach (var modelWithIAwake in _interactableObjectModelsWithIAwake)
            {
                modelWithIAwake.OnAwake();
            }
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            foreach (IUpdate modelWitIhIUpdate in _interactableObjectModelsWithIUpdate)
            {
                modelWitIhIUpdate.Updating();
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

            foreach (var modelWithITearDown in _interactableObjectModelsWithITearDown)
            {
                modelWithITearDown.TearDown();
            }
        }

        #endregion


        #region Methods

        private bool OnFilterHandler(Collider enteredObject)
        {
            return enteredObject.GetComponentInChildren<PlayerBehavior>() != null && !enteredObject.isTrigger;
        }

        private void OnTriggerEnterHandler(ITrigger interactiveObject, Collider enteredObject)
        {
            interactiveObject.IsInteractable = true;
            _context.InteractableObjectModels[interactiveObject.GameObject.GetInstanceID()].
                InteractiveObjectData.MakeInteractive(_context.InteractableObjectModels[interactiveObject.
                    GameObject.GetInstanceID()], interactiveObject, enteredObject);
        }

        private void OnTriggerExitHandler(ITrigger interactiveObject, Collider exitedObject)
        {
            interactiveObject.IsInteractable = false;
            _context.InteractableObjectModels[interactiveObject.GameObject.GetInstanceID()].
                InteractiveObjectData.MakeNotInteractive(_context.InteractableObjectModels[interactiveObject.
                    GameObject.GetInstanceID()], interactiveObject, exitedObject);
        }

        #endregion
    }
}