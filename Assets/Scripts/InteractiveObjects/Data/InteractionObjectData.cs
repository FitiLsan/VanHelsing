using UnityEngine;


namespace BeastHunter
{
    public abstract class InteractionObjectData : ScriptableObject
    {
        #region Field

        public GameObject Prefab;

        #endregion


        #region Metods

        public abstract void Interact(InteractionObjectModel interactiveObjectModel);

        #endregion
    }
}

