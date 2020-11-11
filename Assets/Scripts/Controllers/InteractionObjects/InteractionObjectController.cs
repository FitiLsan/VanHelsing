using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class InteractionObjectController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public InteractionObjectController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var interactionObject = _context.GetListInteractable();

            foreach (var trigger in interactionObject)
            {
                var interactionObjectBehaviour = trigger as InteractionObjectBehaviour;
                interactionObjectBehaviour.OnFilterHandler += OnFilterHandler;
                interactionObjectBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                interactionObjectBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            }

            foreach (var model in _context.InteractionModels)
            {
                model.Value.OnAwake();
            }
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            foreach (var model in _context.InteractionModels.Values)
            {
                model.Execute();
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var npc = _context.GetListInteractable();

            foreach (var trigger in npc)
            {
                var interactionObjectBehaviour = trigger as InteractableObjectBehavior;
                interactionObjectBehaviour.OnFilterHandler -= OnFilterHandler;
                interactionObjectBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                interactionObjectBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }

            foreach (var model in _context.InteractionModels)
            {
                model.Value.OnTearDown();
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