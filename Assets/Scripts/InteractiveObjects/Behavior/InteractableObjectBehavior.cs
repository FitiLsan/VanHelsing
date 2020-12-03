using System;
using UnityEngine;


namespace BeastHunter
{
    public class InteractableObjectBehavior : MonoBehaviour, ITrigger
    {
        #region Fields

        [SerializeField] protected InteractableObjectType _type;
        protected Action<int, Damage> _onTakeDamageHandler;
        protected Action<int, InteractableObjectBehavior, Damage> _onDealDamageHandler;

        #endregion


        #region Properties

        public InteractableObjectType Type { get => _type; }
        public GameObject GameObject => gameObject;

        public Predicate<Collider> OnFilterHandler { get; set; }
        public Action<ITrigger, Collider> OnTriggerEnterHandler { get; set; }
        public Action<ITrigger, Collider> OnTriggerExitHandler { get; set; }
        public Action<ITrigger, InteractableObjectType> DestroyHandler { get; set; }

        public bool IsInteractable { get; set; }

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


        #region DealDamage

        public void SetDealDamageEvent(Action<int, InteractableObjectBehavior, Damage> action)
        {
            if (action != null)
            {
                _onDealDamageHandler += action;
            }
        }

        public void DealDamageEvent(InteractableObjectBehavior enemy, Damage damage)
        {
            _onDealDamageHandler?.Invoke(GameObject.GetInstanceID(), enemy, damage);
        }

        public void DeleteDealDamageEvent(Action<int, InteractableObjectBehavior, Damage> action)
        {
            if (action != null)
            {
                _onDealDamageHandler -= action;
            }
        }

        #endregion
    }
}
