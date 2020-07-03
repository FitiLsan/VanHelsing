using UnityEngine;


namespace BeastHunter
{
    public sealed class PlayerBehavior : InteractableObjectBehavior
    {
        #region Fields

        private Animator _animatorController;
        private CharacterModel _characterModel;
        private Vector3 _lookAtTarget;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _animatorController = gameObject.GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (_lookAtTarget != null && _lookAtTarget != Vector3.zero)
            {
                _animatorController.SetLookAtPosition(_lookAtTarget+Vector3.up*1.5f);
                _animatorController.SetLookAtWeight(1f, 0.5f, 1f, 1f, 1f);
            }
        }

        #endregion


        #region Methods

        public void SetModel(CharacterModel model)
        {
            _characterModel = model;
        }

        public void SetLookAtTarget(Vector3 target)
        {
            _lookAtTarget = target;
        }

        #endregion
    }
}

