using System;
using UnityEngine;


namespace BeastHunter
{
    public class InteractableObjectBehavior : MonoBehaviour, ITrigger
    {
        #region Fields

        [SerializeField] protected InteractableObjectType _type;
        protected Action<int, Damage> _onTakeDamageHandler;

        #endregion


        #region Properties

        public InteractableObjectType Type { get => _type; }
        public GameObject GameObject => gameObject;

        public Predicate<Collider> OnFilterHandler { get; set; }
        public Action<ITrigger, Collider> OnTriggerEnterHandler { get; set; }
        public Action<ITrigger, Collider> OnTriggerExitHandler { get; set; }
        public Action<ITrigger, Collider> OnTriggerStayHandler { get; set; }
        public Action<InteractableObjectBehavior, Collision> OnCollisionEnterHandler { get; set; }
        public Action<ITrigger, InteractableObjectType> DestroyHandler { get; set; }

        public bool IsInteractable { get; set; }
        public float TempDamage { get; set; }
       
        #endregion


        #region UnityMethods

        private void OnTriggerEnter(Collider other)
        {
            if (OnFilterHandler != null && OnFilterHandler.Invoke(other))
            {
                OnTriggerEnterHandler?.Invoke(this, other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (OnFilterHandler != null && OnFilterHandler.Invoke(other))
            {
                OnTriggerExitHandler?.Invoke(this, other);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (OnFilterHandler != null && OnFilterHandler.Invoke(other))
            {
                OnTriggerStayHandler?.Invoke(this, other);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (OnFilterHandler != null && OnFilterHandler.Invoke(collision.collider))
            {
                OnCollisionEnterHandler?.Invoke(this, collision);
            }
        }

        private void OnDestroy()
        {
            DestroyHandler?.Invoke(this, _type);
        }

        #endregion


        #region TakeDamage

        public void SetTakeDamageEvent(Action<int, Damage> action)
        {
            if (action != null)
            {
                _onTakeDamageHandler += action;
            }
        }

        public void TakeDamageEvent(Damage damage)
        {
            _onTakeDamageHandler?.Invoke(GameObject.GetInstanceID(), damage);
        }

        public void DeleteTakeDamageEvent(Action<int, Damage> action)
        {
            if (action != null)
            {
                _onTakeDamageHandler -= action;
            }
        }

        #endregion
    }
}
