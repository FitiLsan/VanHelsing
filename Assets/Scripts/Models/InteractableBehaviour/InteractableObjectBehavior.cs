using System;
using UnityEngine;


namespace BeastHunter
{
    public class InteractableObjectBehavior : MonoBehaviour, ITrigger, ITakeDamage
    {
        #region Fields

        [SerializeField] private InteractableObjectType _type;
        private Action<int, string> _onDoSmthHandler;

        #endregion


        #region Properties

        public Predicate<Collider> OnFilterHandler { get; set; }
        public Action<Damage> OnTakeDamageHandler { get; set; }
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
            if(DestroyHandler != null)
            {
                DestroyHandler.Invoke(this, _type);
            }
        }

        public void TakeDamage(Damage damage)
        {
            if (OnTakeDamageHandler != null)
            {
                OnTakeDamageHandler.Invoke(damage);
            }
        }

        #endregion


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
            if (_onDoSmthHandler != null)
            {
                _onDoSmthHandler.Invoke(GameObject.GetInstanceID(), how);
            }
        }

        public void DeleteDoSmthEvent(Action<int, string> action)
        {
            if (action != null)
            {
                _onDoSmthHandler -= action;
            }
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
