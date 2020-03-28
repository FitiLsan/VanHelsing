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

        public Predicate<Collider2D> OnFilterHandler { get; set; }
        public Action<ITrigger> OnTriggerEnterHandler { get; set; }
        public Action<ITrigger> OnTriggerExitHandler { get; set; }
        public Action<ITrigger, InteractableObjectType> DestroyHandler { get; set; }
        public bool IsInteractable { get; set; }
        public GameObject GameObject => gameObject;
        public InteractableObjectType Type { get => _type; }

        #endregion


        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (OnFilterHandler.Invoke(other))
            {
                OnTriggerEnterHandler.Invoke(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (OnFilterHandler.Invoke(other))
            {
                OnTriggerExitHandler.Invoke(this);
            }
        }

        private void OnDisable()
        {
            DestroyHandler.Invoke(this, _type);
        }

        #endregion
    }
}
