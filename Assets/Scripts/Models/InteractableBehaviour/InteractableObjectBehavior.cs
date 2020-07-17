using System;
using UnityEngine;


namespace BeastHunter
{
    public class InteractableObjectBehavior : MonoBehaviour, ITrigger
    {
        #region Fields

        [SerializeField] private InteractableObjectType _type;
        private Action<int, string> _onDoSmthHandler;
        private Action<int, Damage> _onTakeDamageHandler;
        private Action<int, InteractableObjectBehavior, Damage> _onDealDamageHandler;
        //private _onGenerateInventryHandler;

        #endregion


        #region Properties

        public Predicate<Collider> OnFilterHandler { get; set; }
        public Action<ITrigger, Collider> OnTriggerEnterHandler { get; set; }
        public Action<ITrigger, Collider> OnTriggerExitHandler { get; set; }
        public Action<ITrigger, InteractableObjectType> DestroyHandler { get; set; }
        public GameObject GameObject => gameObject;
        public InteractableObjectType Type { get => _type; }
        public BaseStatsClass Stats { get; set; }
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

        private void OnDisable()
        {
            DestroyHandler?.Invoke(this, _type);
        }

        #endregion


        #region Methods

        public void SetType(InteractableObjectType type)
        {
            _type = type;
        }


        #region DoSmth

        public void SetDoSmthEvent(Action<int, string> action)
        {
            if (action != null)
            {
                _onDoSmthHandler += action;
            }
        }

        public void DoSmthEvent(string how)
        {
            _onDoSmthHandler?.Invoke(GameObject.GetInstanceID(), how);
        }

        public void DeleteDoSmthEvent(Action<int, string> action)
        {
            if (action != null)
            {
                _onDoSmthHandler -= action;
            }
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

        #endregion
    }
}
