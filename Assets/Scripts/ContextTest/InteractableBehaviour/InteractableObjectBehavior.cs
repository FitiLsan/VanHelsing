using System;
using UnityEngine;


namespace BeastHunter
{
    public abstract class InteractableObjectBehavior : MonoBehaviour, ITrigger
    {
        #region Fields

        [SerializeField] private InteractableObjectType _type;

        #endregion


        #region Properties

        public bool IsInteractable { get; set; }

        public Predicate<Collider> OnFilterHandler { get; set; }
        public Action<ITrigger, Collider> OnTriggerEnterHandler { get; set; }
        public Action<ITrigger, Collider> OnTriggerExitHandler { get; set; }
        public Action<ITrigger, InteractableObjectType> DestroyHandler { get; set; }
        public GameObject GameObject => gameObject;
        public InteractableObjectType Type { get => _type; }

        #endregion


        #region UnityMethods

        private void OnTriggerEnter(Collider other)
        {
            if (OnFilterHandler != null && OnFilterHandler.Invoke(other) && OnTriggerEnterHandler != null)
            {
                OnTriggerEnterHandler.Invoke(this, other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (OnFilterHandler != null && OnFilterHandler.Invoke(other) && OnTriggerExitHandler != null)
            {
                OnTriggerExitHandler.Invoke(this, other);
            }
        }

        private void OnDisable()
        {
            DestroyHandler.Invoke(this, _type);
        }

        #endregion


        #region Methods

        public void SetType(InteractableObjectType type)
        {
            _type = type;
        }

        #endregion
    }
}
