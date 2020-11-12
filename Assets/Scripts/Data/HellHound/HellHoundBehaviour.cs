using System;
using UnityEngine;

namespace BeastHunter
{
    class HellHoundBehaviour: InteractableObjectBehavior
    {
        #region Fields

        private HellHoundModel model;
        private Animator animator;


        #endregion


        #region ClassLifeCycles

        public void Initialize(HellHoundModel hellHoundModel)
        {
            model = hellHoundModel;
            animator = GetComponent<Animator>();

            model.OnDead += IsDead;
        }

        #endregion


        #region Methods

        private void IsDead()
        {
            animator.SetBool("IsDead", true);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
        }

        #endregion
    }
}
