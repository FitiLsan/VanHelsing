using UnityEngine;


namespace BeastHunter
{
    public abstract class BaseInteractiveObjectData : ScriptableObject
    {
        #region Field

        public GameObject Prefab;

        #endregion


        #region Metods

        public abstract void Interact(BaseInteractiveObjectModel interactiveObjectModel);

        public abstract void MakeInteractive(BaseInteractiveObjectModel interactiveObjectModel,
            ITrigger interactiveTrigger, Collider enteredCollider);

        public abstract void MakeNotInteractive(BaseInteractiveObjectModel interactiveObjectModel,
            ITrigger interactiveTrigger, Collider enteredCollider);

        #endregion
    }
}

