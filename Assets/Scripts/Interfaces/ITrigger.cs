using System;
using UnityEngine;


namespace BeastHunter
{
    public interface ITrigger : IInteractable
    {
        #region Properties

        InteractableObjectType Type { get; }
        GameObject GameObject { get; }

        Predicate<Collider> OnFilterHandler { get; set; }
        Action<ITrigger, Collider> OnTriggerEnterHandler { get; set; }
        Action<ITrigger, Collider> OnTriggerExitHandler { get; set; }
        Action<ITrigger, InteractableObjectType> DestroyHandler { get; set; }

        #endregion
    }
}
