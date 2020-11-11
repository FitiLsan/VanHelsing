using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewInteractionObject", menuName = "CreateInteractionObject/CreateInteractionObject", order = 0)]
    public class InteractionObjectData : ScriptableObject
    {
        #region Field

        public GameObject Prefab;
        public float InteractionRadius;

        #endregion


        #region Metods

        public virtual void Interact(bool IsInteractable) { }

        #endregion
    }
}

